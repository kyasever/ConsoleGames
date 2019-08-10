namespace Destroy
{
    using System;
    using System.Collections;
    using System.Collections.Generic;


    /// <summary>
    /// 场景中所有实体的类型
    /// </summary>
    public class GameObject
    {
        #region 初始化与回收
        /// <summary>
        /// 创建一个游戏物体 (相当于在当前场景中实例化)
        /// </summary>
        public GameObject(string name = "GameObject", string tag = "None", GameObject parent = null,Vector2 localPosition = default(Vector2), List<Vector2> posList = default(List<Vector2>))
        {
            if (posList == null)
                PosList = new List<Vector2>() { Vector2.Zero };
            else
                PosList = posList;

            Name = name;
            Tag = tag;
            IsActive = true;

            //初始化坐标
            Vector2 pos;
            if (localPosition == null)
                pos = Vector2.Zero;
            else
                pos = localPosition;

            //初始化父子关系
            if(parent == null)
            {
                this.parent = null;
                this.localPosition = pos;
            }
            else
            {
                this.Parent = parent;
                this.LocalPosition = pos;
            }

            Childs = new List<GameObject>();

            if (SceneManager.ActiveScene == null)
                throw new Exception($"未初始化场景!");

            AddToScene(SceneManager.ActiveScene);

            Components = new List<Component>();
            ComponentDict = new Dictionary<Type, Component>();
        }

        /// <summary>
        /// 3.20测试添加
        /// 提供一种新的更便捷的创建物体的思路.直接返回具体脚本而不是游戏物体
        /// </summary>
        public static T CreateWith<T>(string name = "GameObject", string tag = "None", GameObject parent = null ,Vector2 localPosition = default(Vector2), List<Vector2> posList = default(List<Vector2>)) where T : Component, new()
        {
            GameObject obj = new GameObject(name, tag, parent, localPosition, posList);
            T com = obj.AddComponent<T>();
            return com;
        }
        /// <summary>
        /// 销毁一个游戏物体
        /// </summary>
        public static void Destroy(GameObject gameObject)
        {
            //进行这个设置的时候,就已经关掉了所有组件了.对于系统来说相当于已经被移除
            gameObject.SetActive(false);
            //在最后移除它
            RuntimeEngine.GetSystem<DeleteSystem>().GameObjectsToDelete.Add(gameObject);
        }

        /// <summary>
        /// 延迟销毁一个游戏物体
        /// </summary>
        public static void Destroy(GameObject gameObject, float delayTime)
        {
            RuntimeEngine.GetSystem<InvokeSystem>().AddDelayAction(
                () => Destroy(gameObject), delayTime);
        }

        /// <summary>
        /// ?检测是否为空
        /// </summary>
        public static implicit operator bool(GameObject exists)
        {
            if (exists != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 场景管理相关
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; private set; }


        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// 设置自己所有组件的active以及所有子物体组件的active
        /// </summary>
        public void SetActive(bool value)
        {
            if (IsActive != value)
            {
                IsActive = value;

                if (Childs.Count != 0)
                {
                    foreach (GameObject child in Childs)
                    {
                        child.SetActive(value);
                    }
                }

                foreach (Component component in Components)
                {
                    component.Enable = value;
                }
            }
        }

        /// <summary>
        /// 这个游戏物体属于哪个scene
        /// </summary>
        public Scene Scene { get; private set; }


        /// <summary>
        /// 在当前场景中根据名字寻找游戏物体, 若有多个同名物体也只返回一个。
        /// </summary>
        public GameObject Find(string name)
        {
            GameObject result = null;
            Scene.GameObjects.ForEach(gameObject => { if (gameObject.Name == name) result = gameObject; });
            return result;
        }

        /// <summary>
        /// 在当前场景中根据标签寻找游戏物体, 若有多个则返回多个。
        /// </summary>
        public List<GameObject> FindWithTag(string tag)
        {
            List<GameObject> gameObjects = null;
            if (Scene.GameObjectsWithTag.ContainsKey(tag))
                gameObjects = Scene.GameObjectsWithTag[tag];
            return gameObjects;
        }

        internal void AddToScene(Scene scene)
        {
            Scene = scene;
            //将自己添加到列表里面去
            scene.GameObjects.Add(this);
            //将自己添加到tag字典里面去
            if (scene.GameObjectsWithTag.ContainsKey(Tag))
                scene.GameObjectsWithTag[Tag].Add(this);
            else
                scene.GameObjectsWithTag.Add(Tag, new List<GameObject>() { this });
        }

        #endregion

        #region 坐标和父子关系相关,整合自Transform
        private GameObject parent;
        /// <summary>
        /// 父物体
        /// </summary>
        public GameObject Parent
        {
            get => parent;
            set
            {
                if (value == null)
                    return;

                //修改本地坐标
                LocalPosition = Position - value.Position;

                //先把自己从之前的父物体的子对象集合中移除
                if (parent != null)
                    parent.Childs.Remove(this);

                //再加到新的父物体集合里面
                parent = value;
                parent.Childs.Add(this);
            }
        }

        /// <summary>
        /// 子物体的数量
        /// </summary>
        public int ChildCount { get => Childs.Count; }
        /// <summary>
        /// 该游戏物体的子物体集合
        /// </summary>
        [HideInInspector]
        public List<GameObject> Childs { get; private set; }

        /// <summary>
        /// 当发生坐标改变时产生的回调事件,将自己的Position告诉组件,组件自行管理
        /// 参数一 发生改变之前所处的位置 参数二 发生改变之后所处的位置
        /// </summary>
        public event Func<Vector2, Vector2, bool> ChangePositionEvent;

        /// <summary>
        /// 世界坐标
        /// </summary>
        public Vector2 Position
        {
            get => GetWorldPosition(this);
            set
            {
                if (parent == null)
                    LocalPosition = value;
                else
                    LocalPosition = value - parent.Position;
            }
        }

        private Vector2 localPosition;
        /// <summary>
        /// 本地坐标
        /// </summary>
        public Vector2 LocalPosition
        {
            get => localPosition; set
            {
                if (ChangePositionEvent != null && localPosition != value)
                {
                    ChangePositionEvent.Invoke(Position, GetWorldPosition(this, value));
                }
                localPosition = value;
            }
        }


        /// <summary>
        /// 通过递归查找物体的最终节点并相加,获得世界坐标
        /// </summary>
        private Vector2 GetWorldPosition(GameObject gameObject)
        {
            if (gameObject.parent != null)
            {
                return gameObject.LocalPosition + GetWorldPosition(gameObject.parent);
            }
            else
                return gameObject.LocalPosition;
        }

        //用于临时检测可能的位置,加入这个物体LocalPosition到了这个位置,那么它的WorldPos
        private Vector2 GetWorldPosition(GameObject gameObject, Vector2 lPos)
        {
            if (gameObject.parent != null)
            {
                return lPos + GetWorldPosition(gameObject.parent);
            }
            else
                return lPos;
        }
        #endregion

        #region 组件化相关
        /// <summary>
        /// 获取组件个数
        /// </summary>
        public int ComponentCount => Components.Count;

        /// <summary>
        /// 存成列表形式的组件
        /// </summary>
        [HideInInspector]
        internal List<Component> Components { get; set; }


        /// <summary>
        /// 存成字典形式的组件
        /// </summary>
        [HideInInspector]
        internal Dictionary<Type, Component> ComponentDict { get; set; }

        /// <summary>
        /// 添加指定组件
        /// </summary>
        public T AddComponent<T>() where T : Component, new()
        {
            Type root = TypeRootConverter.GetComponentRoot(typeof(T));
            if (root == null)
                throw new Exception("未知错误");

            if (ComponentDict.ContainsKey(root))
            {
                string rootName = nameof(root);
                throw new Exception($"你已经添加了是/继承{rootName}类型的组件{ComponentDict[root]}, " +
                    $"无法继续添加是/继承{rootName}类型的组件.");
            }

            T instance = new T();
            instance.GameObject = this;
            //执行方法, 向系统注册
            Components.Add(instance);
            ComponentDict.Add(root, instance);

            //初始化组件
            instance.Initialize();
            //添加到系统
            instance.OnAdd();

            return instance;
        }

        /// <summary>
        /// 获取指定组件
        /// </summary>
        public T GetComponent<T>() where T : Component
        {
            Type root = TypeRootConverter.GetComponentRoot(typeof(T));
            if (root == null)
                throw new Exception("未知错误");

            if (ComponentDict.ContainsKey(root))
            {
                return ComponentDict[root] as T;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取指定的类型及其子类的集合
        /// </summary>
        public List<T> GetComponents<T>() where T : Component
        {
            Type type = typeof(T);
            List<T> list = new List<T>();

            foreach (Component each in Components)
            {
                Type t = each.GetType();
                if (type == t || t.IsSubclassOf(type))
                {
                    list.Add(each as T);
                }
            }
            return list;
        }

        #endregion

        #region 表明物体的形状,整合原本的Mesh组件

        /// <summary>
        /// Mesh对象的列表
        /// </summary>
        public List<Vector2> PosList { get; private set; }


        private void Sort()
        {
            //使用HashSet去重复. 之后排序
            HashSet<Vector2> set = new HashSet<Vector2>();
            foreach (Vector2 v in PosList)
            {
                set.Add(v);
            }
            List<Vector2> newList = new List<Vector2>();
            foreach (Vector2 v in set)
            {
                newList.Add(v);
            }
            newList.Sort();
            PosList = newList;
        }

        /// <summary>
        /// 进行多点初始化
        /// </summary>
        public void InitMesh(List<Vector2> list)
        {
            PosList = list;
            //更改Mesh的时候进行重新排序
            Sort();
            //触发一下更改坐标函数,这样在更改mesh的时候就可以通知其他系统进行刷新了,如果其关注了mesh
            //等下要改为具有独立的事件
            ChangePositionEvent?.Invoke(Position, Position);
        }

        #endregion
    }
}
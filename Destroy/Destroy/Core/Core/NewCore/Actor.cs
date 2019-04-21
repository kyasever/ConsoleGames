using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destroy.New
{
    /*
    组件分层 IComponent - Component - Scripts
    ICom 没有任何实现, 只是一个约束
    <!-- RawCom 空白的继承ICom, 暂时用不上. -->
    Com通过Actor中介获取来自其他组件的方法
    Scripts在Com的基础上增加Update
    物体分层 GameObject - Actor
    GameObject理论上来讲只包含基础信息组件容器,通过GetCom来获得组件,不包含其他东西
    Actor包含默认组件的引用,充当组件的中介者.默认集成Transform Collider Renderer 不集成其他组件
    */
    /// <summary>
    /// 组件分层 IComponent - Component - Scripts
    /// </summary>
    public interface IComponent
    {
        /// <summary>
        /// 暂时的开关,不会产生移除或添加.如果系统不处理的话相当于没用
        /// </summary>
        [HideInInspector]
        bool Enable { get; set; }

        /// <summary>
        /// 游戏物体的引用
        /// </summary>
        [HideInInspector]
        GameObject GameObject { get; set; }

        /// <summary>
        /// 在该组件被添加到游戏物体时做初始化调用 (在该方法中可以使用GetComponent)
        /// </summary>
        void Initialize();

        /// <summary>
        /// 组件被添加的时候要将自己的引用加入系统. 必须重载
        /// </summary>
        void OnAdd();

        /// <summary>
        /// 组件被移除的时候要将自己的引用从系统中去掉
        /// </summary>
        void OnRemove();
    }

    /// <summary>
    /// 具体的接口,已经获得了各项引用 本身只实现自身的功能,但是可以通过Actor转接调用别的组件直接实现方法
    /// </summary>
    public class Component : IComponent
    {
        protected Actor Actor;
        protected Transform Transform => Actor.Transform;
        protected Collider Collider => Actor.Collider;
        protected Renderer Renderer => Actor.Renderer;
        protected Vector2 Position => Transform.Position;


        private bool enable;
        public bool Enable { get => enable; set => enable = value; }
        private GameObject gameObject;
        public GameObject GameObject { get => gameObject; set => gameObject = value; }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void OnAdd()
        {
            throw new NotImplementedException();
        }

        public void OnRemove()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 变换 包含父子关系和坐标变化.坐标相关
    /// 默认带有,不使用AddCom来添加这个
    /// </summary>
    public class Transform : Component
    {
        #region 父子关系 Parent

        private Transform parent;
        /// <summary>
        /// 父物体
        /// </summary>
        public Transform Parent
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
        /// 该游戏物体的子物体集合
        /// </summary>
        public List<Transform> Childs { get; private set; }

        #endregion

        #region 坐标 Position
        /// <summary>
        /// 世界坐标
        /// </summary>
        public override Vector2 Position
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

        /// <summary>
        /// 通过递归查找物体的最终节点并相加,获得世界坐标
        /// </summary>
        private Vector2 GetWorldPosition(Transform transform)
        {
            if (transform.parent != null)
            {
                return transform.LocalPosition + GetWorldPosition(transform.parent);
            }
            else
                return transform.LocalPosition;
        }


        private Vector2 localPosition;
        /// <summary>
        /// 本地坐标
        /// </summary>
        public Vector2 LocalPosition
        {
            get => localPosition;
            set
            {
                localPosition = value;
            }
        }

        #endregion
    }

    /// <summary>
    /// 对啊 Component.Position 实际上是获取了GameObject.Transform.Position.
    /// new GameObject的时候直接挂好了所有的组件,组件默认
    /// </summary>
    public class GameObject
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; private set; }

        #region 激活状态 Active

        private bool active = true;
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool Active
        {
            get { return active; }
            set
            {
                active = value;
                SetActive(value);
            }
        }

        /// <summary>
        /// 设置自己所有组件的active以及所有子物体组件的active
        /// </summary>
        private void SetActive(bool value)
        {
            if (Active != value)
            {
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
        #endregion


        /// <summary>
        /// 当发生坐标改变时产生的回调事件,将自己的Position告诉组件,组件自行管理
        /// 参数一 发生改变之前所处的位置 参数二 发生改变之后所处的位置
        /// </summary>
        public event Action<Vector2, Vector2> ChangePositionEvnet;

        /// <summary>
        /// 碰撞回调事件.
        /// </summary>
        public Action<Collision> OnCollisionEvent;



        /// <summary>
        /// 这个游戏物体属于哪个scene
        /// </summary>
        public Scene Scene { get; private set; }

        /// <summary>
        /// 获取组件个数
        /// </summary>
        public int ComponentCount => Components.Count;

        /// <summary>
        /// 获取子物体个数
        /// </summary>
        public int ChildCount => Childs.Count;

        /// <summary>
        /// 存成列表形式的组件
        /// </summary>
        internal List<Component> Components { get; set; }

        /// <summary>
        /// 存成字典形式的组件
        /// </summary>
        internal Dictionary<Type, Component> ComponentDict { get; set; }


        /// <summary>
        /// 创建一个游戏物体 (相当于在当前场景中实例化)
        /// </summary>
        public GameObject(string name = "GameObject", string tag = "None", GameObject parent = null)
        {
            Name = name;
            Tag = tag;
            Active = true;

            this.parent = null;
            this.localPosition = Vector2.Zero;
            if (parent != null)
            {
                this.Parent = parent;
                this.LocalPosition = Vector2.Zero;
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
        public static T CreateWith<T>(string name = "GameObject", string tag = "None", GameObject parent = null) where T : Component, new()
        {
            GameObject obj = new GameObject(name, tag, parent);
            T com = obj.AddComponent<T>();
            return com;
        }


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

        /// <summary>
        /// 在指定场景中根据名字寻找游戏物体, 若有多个同名物体也只返回一个。
        /// </summary>
        public static GameObject Find(string sceneName, string name)
        {
            Scene scene = null;

            if (SceneManager.Scenes.ContainsKey(sceneName))
                scene = SceneManager.Scenes[sceneName];

            if (scene == null)
                return null;

            GameObject result = null;
            scene.GameObjects.ForEach(gameObject => { if (gameObject.Name == name) result = gameObject; });
            return result;
        }

        /// <summary>
        /// 在指定场景中根据标签寻找游戏物体, 若有多个则返回多个。
        /// </summary>
        public static List<GameObject> FindWithTag(string sceneName, string tag)
        {
            Scene scene = null;

            if (SceneManager.Scenes.ContainsKey(sceneName))
                scene = SceneManager.Scenes[sceneName];
            if (scene == null)
                return null;

            List<GameObject> gameObjects = null;

            if (scene.GameObjectsWithTag.ContainsKey(tag))
                gameObjects = scene.GameObjectsWithTag[tag];

            return gameObjects;
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
        /// 将一个游戏物体的引用加入DontDestroyOnLoad
        /// </summary>
        public static void DontDestroyOnLoad(GameObject gameObject)
        {
            if (!SceneManager.DontDestroyOnLoad.GameObjects.Contains(gameObject))
                gameObject.AddToScene(SceneManager.DontDestroyOnLoad);

            if (gameObject.ChildCount != 0)
            {
                foreach (var go in gameObject.Childs)
                {
                    DontDestroyOnLoad(go);
                }
            }
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




    }

    /// <summary>
    /// 接口版本的GameObject 通过内联go和com实现 本身作为一个Com挂在物体上
    /// </summary>
    public class Actor
    {
        // Position Layer 
        public Transform Transform;
        public Collider Collider;
        public Renderer Renderer;
        private Mesh meshCom;
    }
}

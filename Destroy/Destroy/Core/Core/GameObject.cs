namespace Destroy
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public enum ObjectType
    {
        Actor, UI
    }

    /// <summary>
    /// Destroy更新后的设计哲学
    /// 大幅度减少暴露的接口. 只允许开发者操作Script一个类
    /// Actor应该是一个组件而不是游戏物体本身. 但是又是可new组件.
    /// </summary>
    public class Actor : GameObject
    {
        public Actor(string name = "GameObject", string tag = "None", GameObject parent = null) : base(name, tag, parent)
        {
            Collider.ColliderList = new List<Vector2>() { new Vector2(0, 0) };
            ObjectType = ObjectType.Actor;
        }

        public static T CreateWith<T>(string name = "GameObject", string tag = "None", GameObject parent = null) where T : Component, new()
        {
            Actor obj = new Actor(name, tag, parent);
            T com = obj.AddComponent<T>();
            return com;
        }

        public override void OnStart()
        {
            //将collider加入物理系统
            Collider = new Collider();
            Collider.GameObject = this;
            RuntimeEngine.GetSystem<CollisionSystem>().ColliderCollection.Add(Collider);

            //将Renderer加入渲染系统,其他单独处理
            Renderer = new Renderer();
            Renderer.GameObject = this;
            Renderer.Depth = int.MaxValue;
            RuntimeEngine.GetSystem<RendererSystem>().ActorRendererCollection.Add(Renderer);
        }

        public override void OnDestroy()
        {
            RuntimeEngine.GetSystem<CollisionSystem>().ColliderCollection.Remove(Collider);
            RuntimeEngine.GetSystem<RendererSystem>().ActorRendererCollection.Remove(Renderer);
        }
    }

    /// <summary>
    /// UIObject 不需要把Collider挂到collisionSystem中,但是需要保留有Collider
    /// 如果脚本继承了IClickEvent 那么其就会被挂到点击回调系统中
    /// </summary>
    public class UIObject : GameObject
    {
        public UIObject(string name = "GameObject", string tag = "None", GameObject parent = null, int depth = -1) : base(name, tag, parent)
        {
            Collider.ColliderList = new List<Vector2>() { new Vector2(0, 0) };
            Renderer.Depth = depth;
            ObjectType = ObjectType.UI;
        }

        public static T CreateWith<T>(string name = "GameObject", string tag = "None", GameObject parent = null) where T : Component, new()
        {
            UIObject obj = new UIObject(name, tag, parent);
            T com = obj.AddComponent<T>();
            return com;
        }

        public override void OnStart()
        {
            //将collider加入物理系统
            Collider = new Collider();
            Collider.GameObject = this;
            RuntimeEngine.GetSystem<EventHandlerSystem>().UICollection.Add(Collider);

            //将Renderer加入渲染系统,其他单独处理
            Renderer = new Renderer();
            Renderer.GameObject = this;
            Renderer.Depth = -1;
            RuntimeEngine.GetSystem<RendererSystem>().UIRendererCollection.Add(Renderer);
        }

        public override void OnDestroy()
        {
            RuntimeEngine.GetSystem<EventHandlerSystem>().UICollection.Remove(Collider);
            RuntimeEngine.GetSystem<RendererSystem>().UIRendererCollection.Remove(Renderer);
        }

    }

    /// <summary>
    /// 对啊 Component.Position 实际上是获取了GameObject.Transform.Position.
    /// new GameObject的时候直接挂好了所有的组件,组件默认
    /// </summary>
    public class GameObject
    {
        //默认包含的三个组件
        public Transform Transform;
        public Collider Collider;
        public Renderer Renderer;
        public ObjectType ObjectType;

        /// <summary>
        /// 父物体
        /// </summary>
        [HideInInspector]
        public GameObject Parent
        {
            get
            {
                return Transform.Parent?.GameObject;
            }
            set => Transform.Parent = value.Transform;
        }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; private set; }


        /// <summary>
        /// 这个游戏物体属于哪个scene
        /// </summary>
        public Scene Scene { get; private set; }

        /// <summary>
        /// 创建时调用,进行默认组件添加和初始化操作
        /// </summary>
        public virtual void OnStart()
        {

        }

        /// <summary>
        /// 被销毁时调用,进行组件解绑操作
        /// </summary>
        public virtual void OnDestroy()
        {

        }

        #region 激活状态 Active
        /// <summary>
        /// 获取子物体个数
        /// </summary>
        public int ChildCount => Transform.Childs.Count;

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
        public void SetActive(bool value)
        {
            if (Active != value)
            {
                Active = value;

                if (Transform.Childs.Count != 0)
                {
                    foreach (Transform child in Transform.Childs)
                    {
                        child.GameObject.SetActive(value);
                    }
                }
                foreach (Component component in ComponentDict.Values)
                {
                    component.Enable = value;
                    Renderer.Enable = value;
                    Collider.Enable = value;
                    Transform.Enable = value;
                }
            }
        }
        #endregion

        #region 初始化

        /// <summary>
        /// 创建一个游戏物体 (相当于在当前场景中实例化)
        /// </summary>
        public GameObject(string name = "GameObject", string tag = "None", GameObject parent = null)
        {
            //关于游戏物体本身的特性
            Name = name;
            Tag = tag;
            Active = true;

            //游戏物体在场景中
            if (SceneManager.ActiveScene == null)
            {
                Debug.Error("未初始化场景");
            }
            AddToScene(SceneManager.ActiveScene);

            //包含的组件
            ComponentDict = new Dictionary<Type, Component>();

            //直接添加Transform
            Transform = new Transform();
            Transform.GameObject = this;

            OnStart();

            if (parent != null)
            {
                this.Parent = parent;
                Transform.LocalPosition = Vector2.Zero;
            }
        }
        #endregion

        #region 组件化
        /// <summary>
        /// 获取组件个数
        /// </summary>
        public int ComponentCount => ComponentDict.Values.Count;

        /// <summary>
        /// 存成字典形式的组件
        /// </summary>
        internal Dictionary<Type, Component> ComponentDict { get; set; }

        /// <summary>
        /// 添加指定组件
        /// </summary>
        public virtual T AddComponent<T>() where T : Component, new()
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
        #endregion

        /// <summary>
        /// 销毁一个游戏物体
        /// </summary>
        public static void Destroy(GameObject gameObject)
        {
            //进行这个设置的时候,就已经关掉了所有组件了.对于系统来说相当于已经被移除
            gameObject.SetActive(false);
            //在最后移除它 可能没有移除子物体
            RuntimeEngine.GetSystem<DeleteSystem>().GameObjectsToDelete.Add(gameObject);
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
}
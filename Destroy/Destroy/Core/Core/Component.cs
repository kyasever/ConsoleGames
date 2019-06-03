namespace Destroy
{
    using System;
    using System.Collections.Generic;

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

    public interface IComponent
    {
        GameObject GameObject { get; set; }
        bool Enable { get; set; }
    }

    public class RawComponent : IComponent
    {

        #region ICom接口
        protected GameObject gameObject;
        public GameObject GameObject { get => gameObject; set => gameObject = value; }
        protected bool enable = true;
        public bool Enable { get => enable; set => enable = value; }
        #endregion

        protected Transform Transform => GameObject.Transform;
        protected Collider Collider => GameObject.Collider;
        protected Renderer Renderer => GameObject.Renderer;

        #region From Transfrom
        [HideInInspector]
        public virtual Vector2 Position { get => Transform.Position; set => Transform.Position = value; }

        /// <summary>
        /// 获取本地坐标
        /// </summary>
        [HideInInspector]
        public virtual Vector2 LocalPosition
        {
            get => Transform.LocalPosition;
            set => Transform.LocalPosition = value;
        }

        [HideInInspector]
        public GameObject Parent
        {
            get
            {
                return Transform.Parent?.GameObject;
            }
            set => Transform.Parent = value.Transform;
        }
        #endregion

    }

    /// <summary>
    /// 具体的接口,已经获得了各项引用 本身只实现自身的功能,但是可以通过Actor转接调用别的组件直接实现方法
    /// </summary>
    public class Component : RawComponent
    {
        public ObjectType ObjectType { get => gameObject.ObjectType; }

        public virtual void Initialize() { }

        public virtual void OnAdd()
        {

        }

        public virtual void OnRemove()
        {

        }

        /// <summary>
        /// 添加指定组件
        /// </summary>
        public T AddComponent<T>() where T : Component, new()
        {
            return GameObject.AddComponent<T>();
        }

        /// <summary>
        /// 获取指定组件
        /// </summary>
        public T GetComponent<T>() where T : Component
        {
            return GameObject.GetComponent<T>();
        }


        #region From Collider
        /// <summary>
        /// 碰撞回调事件.
        /// </summary>
        public Action<Collision> OnCollisionEvent
        {
            get => Collider.OnCollisionEvent;
            set
            {
                Collider.OnCollisionEvent = value;
            }
        }

        /// <summary>
        /// 碰撞体包含的点的列表,通常情况下来说保持和Mesh的数据相同
        /// </summary>
        public List<Vector2> ColliderList { get => Collider.ColliderList; set { Collider.ColliderList = value; } }

        /// <summary>
        /// 当进入组件时产生的事件
        /// </summary>
        public Action OnMoveInEvent
        {
            get => Collider.OnMoveInEvent;
            set
            {
                Collider.OnMoveInEvent = value;
            }
        }

        /// <summary>
        /// 当离开组件时产生的事件
        /// </summary>
        public Action OnMoveOutEvent
        {
            get => Collider.OnMoveOutEvent;
            set
            {
                Collider.OnMoveOutEvent = value;
            }
        }

        /// <summary>
        /// 当点击组件时产生的事件
        /// </summary>
        public Action OnClickEvent
        {
            get => Collider.OnClickEvent;
            set
            {
                Collider.OnClickEvent = value;
            }
        }

        /// <summary>
        /// 使碰撞体数据和渲染数据保持同步
        /// </summary>
        public void RefreshCollider()
        {
            ColliderList = new List<Vector2>();
            foreach(var v in RendererPoints)
            {
                ColliderList.Add(v.Key);
            }
        }

        #endregion

        #region From Renderer

        public Dictionary<Vector2, RenderPoint> RendererPoints => Renderer.RendererPoints;

        /// <summary>
        /// 清空渲染数据
        /// </summary>
        public void ClearRenderer()
        {
            Renderer.Refresh();
        }

        public void DrawString(string str, int MaxWidth = int.MaxValue, int MinWidth = 0)
        {
            Renderer.DrawString(str, Config.DefaultForeColor, Config.DefaultBackColor, Vector2.Zero, MaxWidth, MinWidth);
        }

        public void DrawString(string str, Color foreColor, Color backColor, int MaxWidth = int.MaxValue, int MinWidth = 0)
        {
            Renderer.DrawString(str, foreColor, backColor, Vector2.Zero, MaxWidth, MinWidth);
        }

        public void DrawString(string str, Vector2 StartPosition, int MaxWidth = int.MaxValue, int MinWidth = 0)
        {
            Renderer.DrawString(str, Config.DefaultForeColor, Config.DefaultBackColor, StartPosition, MaxWidth, MinWidth);
        }

        public void DrawString(string str, Color foreColor, Color backColor, Vector2 StartPosition ,int MaxWidth = int.MaxValue, int MinWidth = 0)
        {
            Renderer.DrawString(str, foreColor, backColor, StartPosition, MaxWidth, MinWidth);
        }

        /// <summary>
        /// 第一个参数 从Vector2/Line/Rectangle 中选取一个,代表要渲染的形状
        /// 第二个参数 从Void(没有参数)/RenderPoint/Color 中选取一个,代表使用制表符/指定的渲染点/背景色中选取一个,代表渲染的填充物
        /// </summary>
        public void Draw(params object[] args)
        {
            Renderer.Draw(args);
        }
        #endregion

        #region From SceneManager
        /// <summary>
        /// 销毁一个游戏物体
        /// </summary>
        public static void DestroyObject(GameObject gameObject)
        {
            SceneManager.DestroyObject(gameObject);
        }

        /// <summary>
        /// 延迟销毁一个游戏物体
        /// </summary>
        public static void DestroyObject(GameObject gameObject, float delayTime)
        {
            RuntimeEngine.GetSystem<InvokeSystem>().AddDelayAction(
                () => DestroyObject(gameObject), delayTime);
        }

        /// <summary>
        /// 在当前场景中查找一个物体
        /// </summary>
        public GameObject Find(string name)
        {
            return SceneManager.Find(name);
        }

        /// <summary>
        /// 在当前场景中根据标签寻找游戏物体, 若有多个则返回多个。
        /// </summary>
        public List<GameObject> FindWithTag(string tag)
        {
            return SceneManager.FindWithTag(tag);
        }

        /// <summary>
        /// 将一个游戏物体的引用加入DontDestroyOnLoad
        /// </summary>
        public void DontDestroyOnLoad(GameObject gameObject)
        {
            SceneManager.DontDestroyOnLoad(gameObject);
        }
        #endregion

    }
}
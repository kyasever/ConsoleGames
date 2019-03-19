namespace Destroy
{
    using System.Collections.Generic;

    /*
     *  by kyasever 12/19
     *  组件没有名字,只有它的类型.反正也不允许多挂
     *  组件不继承object Enable是暂时的开关,可以由系统进行关注
     *  gameobject使用active
     */

    /// <summary>
    /// 所有组件的基类,不要new Component对象,使用AddComponent
    /// </summary>
    public abstract class Component
    {
        /// <summary>
        /// 暂时的开关,不会产生移除或添加.如果系统不处理的话相当于没用
        /// </summary>
        [HideInInspector]
        public bool Enable { get; set; } = true;

        /// <summary>
        /// 游戏物体
        /// </summary>
        [HideInInspector]
        public GameObject GameObject { get; internal set; } = null;

        /// <summary>
        /// 构造方法
        /// </summary>
        public Component()
        {
        }

        /// <summary>
        /// 在该组件被添加到游戏物体时做初始化调用 (在该方法中可以使用GetComponent)
        /// </summary>
        internal virtual void Initialize() { }

        /// <summary>
        /// 组件被添加的时候要将自己的引用加入系统. 必须重载
        /// </summary>
        internal virtual void OnAdd() { }

        /// <summary>
        /// 组件被移除的时候要将自己的引用从系统中去掉
        /// </summary>
        internal virtual void OnRemove() { }

        /// <summary>
        /// 使用?检测
        /// </summary>
        public static implicit operator bool(Component exists)
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

        #region 从GameObject处获得方法


        /// <summary>
        /// 获取组件个数
        /// </summary>
        [HideInInspector]
        public int ComponentCount => GameObject.ComponentCount;

        /// <summary>
        /// 获取子物体个数
        /// </summary>
        [HideInInspector]    
        public int ChildCount => GameObject.ChildCount;

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

        //移除可以考虑使用Enable = False或者删掉这个游戏物体

        /// <summary>
        /// 获取指定的类型及其子类的集合
        /// </summary>
        public List<T> GetComponents<T>() where T : Component => GameObject.GetComponents<T>();

        /// <summary>
        /// 获取世界坐标,重载这个东西,然后加塞
        /// </summary>
        [HideInInspector]
        public virtual Vector2Int Position
        {
            get => GameObject.Position;
            set => GameObject.Position = value;
        }

        /// <summary>
        /// 获取本地坐标
        /// </summary>
        [HideInInspector]
        public virtual Vector2Int LocalPosition
        {
            get => GameObject.LocalPosition;
            set => GameObject.LocalPosition = value;
        }

        #endregion
    }
}
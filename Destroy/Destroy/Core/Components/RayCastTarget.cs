namespace Destroy
{
    using System;
    using System.Collections.Generic;


    /*
     *典型用法 RayCastTarget rayCastTarget = AddComponent<RayCastTarget>();
     * rayCastTarget.OnClickEvent += OnClick;
     * 
     * TODO 整合进script
     */

    /// <summary>
    /// 通过挂载这个组件来接收UI点击事件.
    /// </summary>
    public class RayCastTarget : Component
    {
        /// <summary>
        /// 当进入组件时产生的事件
        /// </summary>
        public Action OnMoveInEvent;
        /// <summary>
        /// 当离开组件时产生的事件
        /// </summary>
        public Action OnMoveOutEvent;
        /// <summary>
        /// 当点击组件时产生的事件
        /// </summary>
        public Action OnClickEvent;

        /// <summary>
        /// 保存一个来自Mesh的引用.有点偷懒
        /// </summary>
        public List<Vector2> colliderList;

        private EventHandlerSystem system;

        internal override void Initialize()
        {
            system = RuntimeEngine.GetSystem<EventHandlerSystem>();

            if (system == null)
            {
                Debug.Error("没有事件系统,请检查EventHandlerSystem");
                return;
            }

            Mesh mesh = GetComponent<Mesh>();
            if (mesh == null)
                mesh = AddComponent<Mesh>();
            colliderList = mesh.PosList;
        }

        internal override void OnAdd()
        {
            system.AddToSystem(this, Position);
            GameObject.ChangePositionEvnet += MoveTo;
        }

        internal override void OnRemove()
        {
            system.RemoveFromSystem(this, Position);
            GameObject.ChangePositionEvnet -= MoveTo;
        }

        /// <summary>
        /// 发生移动时在系统中对应的产生移动
        /// </summary>
        private bool MoveTo(Vector2 from, Vector2 to)
        {
            system.RemoveFromSystem(this, from);
            system.AddToSystem(this, to);
            return true;
        }
    }
}

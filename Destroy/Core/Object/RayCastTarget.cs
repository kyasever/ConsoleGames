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
    /// 其实这种实现方法是尤其优越性的.不过应该考虑一下更改mesh会产生的问题
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

        private EventHandlerSystem system;

        internal override void Initialize()
        {
            system = RuntimeEngine.GetSystem<EventHandlerSystem>();

            if (system == null)
            {
                Debug.Error("没有事件系统,请检查EventHandlerSystem");
                return;
            }
        }

        internal override void OnAdd()
        {
            system.AddToSystem(this, Position);
            GameObject.ChangePositionEvent += MoveTo;
        }

        internal override void OnRemove()
        {
            system.RemoveFromSystem(this, Position);
            GameObject.ChangePositionEvent -= MoveTo;
        }

        /// <summary>
        /// 发生移动时在系统中对应的产生移动
        /// </summary>
        private void MoveTo(Vector2 from, Vector2 to)
        {
            system.RemoveFromSystem(this, from);
            system.AddToSystem(this, to);
        }
    }
}

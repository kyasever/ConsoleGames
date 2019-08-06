namespace Destroy
{
    using System;
    using System.Collections.Generic;

    /*
     * 1/21 kyaseer
     * 现在MoveMent和Collider组合作为一个组件,废弃原来的Rigid系统,扔进legacy 只用这一种物理
    */

    /// <summary>
    /// 碰撞体组件,一般来说默认按着Mesh来
    /// </summary>
    public class Collider : Component
    {
        /// <summary>
        /// 碰撞回调事件.
        /// </summary>
        public Action<Collision> OnCollisionEvent;

        internal override void OnAdd()
        {
            RuntimeEngine.GetSystem<CollisionSystem>().AddToSystem(this);
            GameObject.ChangePositionEvent += OnMove;
        }

        private bool OnMove(Vector2 from, Vector2 to)
        {
            RuntimeEngine.GetSystem<CollisionSystem>().MoveInSystem(this, from, to);
            return true;
        }

        internal override void OnRemove()
        {
            RuntimeEngine.GetSystem<CollisionSystem>().RemoveFromSystem(this);
            GameObject.ChangePositionEvent -= OnMove;
        }
    }
}
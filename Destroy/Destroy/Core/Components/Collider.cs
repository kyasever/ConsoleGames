namespace Destroy
{
    using System;
    using System.Collections.Generic;

    /*
     * 1/21 kyaseer
     * 现在MoveMent和Collider组合作为一个组件,废弃原来的Rigid系统,扔进legacy 只用这一种物理
    */


    /// <summary>
    /// 碰撞体组件
    /// </summary>
    public class Collider : RawComponent
    {
        /// <summary>
        /// 碰撞回调事件.
        /// </summary>
        public Action<Collision> OnCollisionEvent { get; set; }

        /// <summary>
        /// 碰撞体包含的点的列表,通常情况下来说保持和Mesh的数据相同
        /// </summary>
        public List<Vector2> ColliderList { get; set; }

        /// <summary>
        /// 当进入组件时产生的事件
        /// </summary>
        public Action OnMoveInEvent { get; set; }

        /// <summary>
        /// 当离开组件时产生的事件
        /// </summary>
        public Action OnMoveOutEvent { get; set; }

        /// <summary>
        /// 当点击组件时产生的事件
        /// </summary>
        public Action OnClickEvent { get; set; }
    }
}
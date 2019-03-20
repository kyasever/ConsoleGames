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
        /// 碰撞体包含的点的列表,通常情况下来说保持和Mesh的数据相同
        /// </summary>
        public List<Vector2> ColliderList { get; private set; }

        internal override void OnAdd()
        {
                RuntimeEngine.GetSystem<CollisionSystem>().AddToSystem(this);
            GameObject.ChangePositionEvnet += OnMove;
        }

        private bool OnMove(Vector2 from, Vector2 to)
        {
            RuntimeEngine.GetSystem<CollisionSystem>().MoveInSystem(this, from, to);
            return true;
        }

        internal override void OnRemove()
        {
                RuntimeEngine.GetSystem<CollisionSystem>().RemoveFromSystem(this);


            GameObject.ChangePositionEvnet -= OnMove;
        }

        internal override void Initialize()
        {
            Mesh mesh = GetComponent<Mesh>();
            if (mesh == null)
            {
                mesh = AddComponent<Mesh>();
                Debug.Warning("没有Mesh组件,自动生成一个Mesh");
            }
            //将对象复制到此组件内. 更改Mesh中的list同样会更改这个
            ColliderList = mesh.PosList;
            return;
        }
    }
}
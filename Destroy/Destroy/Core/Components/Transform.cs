using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destroy
{
    /// <summary>
    /// 变换 包含父子关系和坐标变化.坐标相关
    /// 默认带有,不使用AddCom来添加这个
    /// </summary>
    public class Transform : RawComponent
    {
        #region 父子关系 Parent

        private Transform parent;

        /// <summary>
        /// 父物体
        /// </summary>
        [HideInInspector]
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
        [HideInInspector]
        public List<Transform> Childs { get; private set; } = new List<Transform>();

        #endregion

        #region 坐标 Position
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
}

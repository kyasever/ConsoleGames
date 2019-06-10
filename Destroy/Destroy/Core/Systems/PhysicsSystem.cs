/*
 * TODO: 加入Mesh旋转功能.Rotate 然后就可以做一个俄罗斯方块了.使用全物理模拟
 * 
 * 12/13
 * 优化了多点碰撞和循环碰撞问题. 
 * 更新了一套超吊的质量运算系统.
 * 通过递归检测,现在可以随便推了,怎么推都行
 * 4/23
 * 经过了不知道多少个版本之后,现在碰撞系统被换成了最简洁的样子
 */
namespace Destroy
{
    using System.Collections.Generic;

    /// <summary>
    /// 发生碰撞的碰撞信息.
    /// </summary>
    public class Collision
    {
        /// <summary>
        /// 发生碰撞的点的坐标
        /// </summary>
        public Vector2 HitPos => ColliderList[0].Key;
        /// <summary>
        /// 获取第一个产生碰撞的物体
        /// </summary>
        public Collider OtherCollier => ColliderList[0].Value;

        public int CollisionCount => ColliderList.Count;
        /// <summary>
        /// 发生碰撞的地点和碰到的碰撞体
        /// </summary>
        public List<KeyValuePair<Vector2, Collider>> ColliderList = new List<KeyValuePair<Vector2, Collider>>();
    }

    /// <summary>
    /// 物理检测 待补完
    /// </summary>
    public static class Physics
    {
        /// <summary>
        /// 射线检测,输入要的结构,和点集或者射线,返回是否发生了碰撞
        /// </summary>
        public static bool RayCast(Collision collision)
        {
            return false;
        }
    }

    /// <summary>
    /// 轻量级碰撞检测系统,只会在移动的时候进行通知操作,不会管多余的事情.需要用Movement组件向其发送请求
    /// </summary>
    public class CollisionSystem : DestroySystem
    {
        public List<Collider> ColliderCollection = new List<Collider>();

        /// <summary>
        /// 每一个点都可以放很多碰撞体组件
        /// </summary>
        public Dictionary<Vector2, List<Collider>> Colliders { get; private set; } = new Dictionary<Vector2, List<Collider>>();

        /// <summary>
        /// 将所有Collider都加入图中. 然后遍历
        /// </summary>
        public override void Update()
        {
            //每一个碰撞体将要接收到的碰撞结果
            Dictionary<Collider, Collision> result = new Dictionary<Collider, Collision>();
            //所有地点的集合
            Colliders = new Dictionary<Vector2, List<Collider>>();
            //将所有碰撞体都加入储存列表中 并产生碰撞信息
            foreach (Collider collider in ColliderCollection)
            {
                if (!collider.Enable)
                    continue;
                foreach (Vector2 dis in collider.ColliderList)
                {
                    Vector2 pos = collider.Position + dis;
                    if (Colliders.ContainsKey(pos))
                    {
                        foreach (Collider otherCollider in Colliders[pos])
                        {
                            SafeAddResult(collider, otherCollider, pos);
                            SafeAddResult(otherCollider, collider, pos);
                        }
                        Colliders[pos].Add(collider);
                    }
                    else
                    {
                        Colliders.Add(pos, new List<Collider>() { collider });
                    }
                }
            }
            void SafeAddResult(Collider thisCollider, Collider otherCollider, Vector2 pos)
            {
                if (!result.ContainsKey(thisCollider))
                    result.Add(thisCollider, new Collision());
                result[thisCollider].ColliderList.Add(new KeyValuePair<Vector2, Collider>(pos, otherCollider));
            }
            //通知所有产生碰撞的物体
            foreach (var pair in result)
            {
                pair.Key.OnCollisionEvent?.Invoke(pair.Value);
            }
        }
    }
}
/*
 * TODO: 加入Mesh旋转功能.Rotate 然后就可以做一个俄罗斯方块了.使用全物理模拟
 * 
 * 12/13
 * 优化了多点碰撞和循环碰撞问题. 
 * 更新了一套超吊的质量运算系统.
 * 通过递归检测,现在可以随便推了,怎么推都行
 */
namespace Destroy
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 发生碰撞的碰撞信息.
    /// </summary>
    public class Collision
    {
        /// <summary>
        /// 发生碰撞的点的坐标
        /// </summary>
        public Vector2 HitPos { get; set; }
        /// <summary>
        /// 发生碰撞的碰撞体的数量
        /// </summary>
        public int CollisionCount => ColliderList.Count;
        /// <summary>
        /// 获取第一个产生碰撞的物体
        /// </summary>
        public Collider OtherCollier => ColliderList[0];
        /// <summary>
        /// 获取完整的产生碰撞的碰撞体列表
        /// </summary>
        public List<Collider> ColliderList { get; private set; }
        /// <summary>
        /// 使用一个碰撞体初始化
        /// </summary>
        public Collision(Collider other,Vector2 hitpos)
        {
            HitPos = hitpos;
            ColliderList = new List<Collider>() { other };
        }
        /// <summary>
        /// 使用很多个碰撞体初始化
        /// </summary>
        public Collision(List<Collider> list,Vector2 hitpos)
        {
            HitPos = hitpos;
            ColliderList = list;
        }
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
        /// <summary>
        /// 每一个点都可以放很多碰撞体组件
        /// </summary>
        public Dictionary<Vector2, List<Collider>> Colliders { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public CollisionSystem()
        {
            needUpdate = false;
            Colliders = new Dictionary<Vector2, List<Collider>>();
        }

        /// <summary>
        /// 将一个碰撞体加入系统
        /// </summary>
        public void AddToSystem(Collider collider)
        {
            AddToSystem(collider, collider.Position);
        }

        /// <summary>
        /// 将一个碰撞体加入系统,手动制定加入的位置
        /// </summary>
        public void AddToSystem(Collider collider,Vector2 position)
        {
            //一个碰撞体在一次加入中只会与另一个碰撞体碰撞一次 不能与自己碰撞
            Dictionary<Collider, Vector2> chash = new Dictionary<Collider, Vector2>();
            chash.Add(collider, position);

            //两个大碰撞体多点接触不会触发多次,可能发生多次位置不同的接触产生多次回调
            foreach (Vector2 dis in collider.GameObject.PosList)
            {
                Vector2 pos = position + dis;
                if (Colliders.ContainsKey(pos))
                {
                    List<Collider> onCollisionList = new List<Collider>();
                    foreach (Collider c in Colliders[pos])
                    {
                        if (!chash.ContainsKey(c))
                        {
                            //不会和它第二次碰撞
                            chash.Add(c,pos);
                            onCollisionList.Add(c);
                        }
                    }
                    //说明发生了新的碰撞
                    if (onCollisionList.Count > 0)
                    {
                        //执行回调方法
                        collider.OnCollisionEvent?.Invoke(new Collision(onCollisionList,chash[onCollisionList[0]]));
                    }
                    //将这个顺利加入系统
                    Colliders[pos].Add(collider);
                }
                else
                {
                    //这个点是空的,直接加入系统
                    Colliders.Add(pos, new List<Collider>() { collider });
                }
            }
        }

        /// <summary>
        /// 将一个碰撞体移除系统
        /// </summary>
        public void RemoveFromSystem(Collider collider)
        {
            RemoveFromSystem(collider, collider.Position);
        }

        /// <summary>
        /// 将一个碰撞体移除系统,手动制定位置
        /// </summary>
        public void RemoveFromSystem(Collider collider,Vector2 position)
        {
            foreach (Vector2 dis in collider.GameObject.PosList)
            {
                Vector2 pos = position + dis;
                if (Colliders.ContainsKey(pos))
                {
                    Colliders[pos].Remove(collider);
                    if (Colliders[pos].Count == 0)
                    {
                        Colliders.Remove(pos);
                    }
                }
            }
        }

        /// <summary>
        /// 判断一个碰撞体是否可以在系统中移动,判断的时候会产生碰撞回调但是不会移动
        /// </summary>
        public bool CanMoveInPos(Collider collider, Vector2 from,Vector2 to)
        {
            //一个碰撞体在一次加入中只会与另一个碰撞体碰撞一次 不能与自己碰撞
            Dictionary<Collider, Vector2> chash = new Dictionary<Collider, Vector2>();
            chash.Add(collider, from);

            bool canMoveFlag = true;

            //两个大碰撞体多点接触不会触发多次,可能发生多次位置不同的接触产生多次回调
            foreach (Vector2 colliderDis in collider.GameObject.PosList)
            {
                Vector2 pos = to + colliderDis;
                if (Colliders.ContainsKey(pos))
                {
                    List<Collider> onCollisionList = new List<Collider>();
                    foreach (Collider c in Colliders[pos])
                    {
                        if (!chash.ContainsKey(c))
                        {
                            //不会和它第二次碰撞
                            chash.Add(c,pos);
                            onCollisionList.Add(c);
                        }
                    }
                    //说明发生了新的碰撞
                    if (onCollisionList.Count > 0)
                    {
                        //执行回调方法
                        collider.OnCollisionEvent?.Invoke(new Collision(onCollisionList, chash[onCollisionList[0]]));
                    }
                    //这里仅做碰撞检测,在移动的尝试中如果发生了碰撞,那么会进行碰撞回调,但是可能不会进行实际的移动
                    canMoveFlag = false;
                }
            }
            return canMoveFlag;
        }

        /// <summary>
        /// 让一个碰撞体在系统中移动
        /// </summary>
        public void MoveInSystem(Collider collider, Vector2 from,Vector2 to)
        {
            RemoveFromSystem(collider,from);
            AddToSystem(collider, to);
        }

    }

}
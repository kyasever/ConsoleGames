using Destroy.Winform;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Destroy
{
    /// <summary>
    /// 寻路算法的返回值
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// 搜索是否成功
        /// </summary>
        public bool Success = true;
        /// <summary>
        /// 结果路径
        /// </summary>
        public List<Vector2> ResultList;
        /// <summary>
        /// 搜索一共搜了多少个点,用于评估算法的效率
        /// </summary>
        public int SearchAeraCount;
    }

    /// <summary>
    /// NavMesh工具类,提供算法支持 <see langword="static"/>
    /// 应该还会有一个NavMeshAgent,但不会有System. 这个静态类提供算法Agent负责调用
    /// 在战棋游戏里有战棋的Agent,原理差不多
    /// </summary>
    public static class Navigation
    {
        /// <summary>
        /// 默认搜索方法
        /// </summary>
        public static SearchResult Search(Vector2 start, Vector2 end)
        {
            return Search(start, end, CanMoveInPhysics);
        }

        /// <summary>
        /// 通常的一种搜寻判定,所有物理系统的物体均不可穿过
        /// </summary>
        public static bool CanMoveInPhysics(Vector2 v)
        {
            Dictionary<Vector2, List<Collider>> dic = RuntimeEngine.GetSystem<CollisionSystem>().Colliders;
            if (dic.ContainsKey(v))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 搜寻判定,所有的点均可以穿过
        /// </summary>
        public static bool CanMoveInAll(Vector2 v)
        {
            return true;
        }

        #region 搜索辅助的数据结构

        /// <summary>
        /// 通过字典实现的保存路径搜索信息的数据结构
        /// 相当于Dictionary 去掉了大部分功能,并增加了默认值
        /// </summary>
        public class RouteDic
        {
            private Dictionary<Vector2, int> SearchDic = new Dictionary<Vector2, int>();

            /// <summary>
            /// 这其实是一个可变列表
            /// 一定能通过key取到值,如果不存在则返回最大值
            /// 一定能给key赋值,如果不存在则创建新对
            /// </summary>
            public int this[Vector2 key]
            {
                get
                {
                    if (SearchDic.ContainsKey(key))
                    {
                        return SearchDic[key];
                    }
                    else
                    {
                        return int.MaxValue;
                    }

                }
                set
                {
                    if (SearchDic.ContainsKey(key))
                    {
                        SearchDic[key] = value;
                    }
                    else
                    {
                        SearchDic.Add(key, value);
                    }
                }
            }

            /// <summary>
            /// 验证
            /// </summary>
            public bool ContainsKey(Vector2 key)
            {
                return SearchDic.ContainsKey(key);
            }

            /// <summary>
            /// 数量
            /// </summary>
            public int Count => SearchDic.Count;

            #region 完整的实现IDictionary接口,感觉没什么必要,反正也用不上就不继承了
            /*
            public ICollection<Vector2Int> Keys => ((IDictionary<Vector2Int, int>)SearchDic).Keys;

            public ICollection<int> Values => ((IDictionary<Vector2Int, int>)SearchDic).Values;

            public int Count => ((IDictionary<Vector2Int, int>)SearchDic).Count;

            public bool IsReadOnly => ((IDictionary<Vector2Int, int>)SearchDic).IsReadOnly;

            public void Add(KeyValuePair<Vector2Int, int> item)
            {
                ((IDictionary<Vector2Int, int>)SearchDic).Add(item);
            }

            public void Clear()
            {
                ((IDictionary<Vector2Int, int>)SearchDic).Clear();
            }

            public bool Contains(KeyValuePair<Vector2Int, int> item)
            {
                return ((IDictionary<Vector2Int, int>)SearchDic).Contains(item);
            }

            public bool ContainsKey(Vector2Int key)
            {
                return ((IDictionary<Vector2Int, int>)SearchDic).ContainsKey(key);
            }

            public void CopyTo(KeyValuePair<Vector2Int, int>[] array, int arrayIndex)
            {
                ((IDictionary<Vector2Int, int>)SearchDic).CopyTo(array, arrayIndex);
            }

            public IEnumerator<KeyValuePair<Vector2Int, int>> GetEnumerator()
            {
                return ((IDictionary<Vector2Int, int>)SearchDic).GetEnumerator();
            }

            public bool Remove(Vector2Int key)
            {
                return ((IDictionary<Vector2Int, int>)SearchDic).Remove(key);
            }

            public bool Remove(KeyValuePair<Vector2Int, int> item)
            {
                return ((IDictionary<Vector2Int, int>)SearchDic).Remove(item);
            }

            public bool TryGetValue(Vector2Int key, out int value)
            {
                return ((IDictionary<Vector2Int, int>)SearchDic).TryGetValue(key, out value);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IDictionary<Vector2Int, int>)SearchDic).GetEnumerator();
            }
            */
            #endregion
        }

        /// <summary>
        /// 用于存储一个路径点的信息,包含一个坐标和这个点的数字,主要用于比较和排序
        /// 相当于KeyValuePair 并增加了排序Icomparable
        /// </summary>
        public class RouteData : IComparable
        {
            /// <summary>
            /// 位置
            /// </summary>
            public Vector2 vector;
            /// <summary>
            /// 值
            /// </summary>
            public int depth;

            /// <summary>
            /// 使用坐标和数字初始化
            /// </summary>
            public RouteData(Vector2 v, int i)
            {
                vector = v;
                depth = i;
            }

            /// <summary>
            /// 使用路径字典初始化
            /// </summary>
            public RouteData(Vector2 v, RouteDic dic)
            {
                vector = v;
                depth = dic[v];
            }

            /// <summary>
            /// 实现IComparable
            /// </summary>
            public int CompareTo(object obj)
            {
                return depth.CompareTo(((RouteData)obj).depth);
            }
        }

        #endregion

        /// <summary>
        /// 基于距离判定的优化DFS算法 起点终点,返回路径点列表
        /// </summary>
        /// <param name="start">起始点</param>
        /// <param name="stop">终点</param>
        /// <param name="canMoveFunc">一个方法用于指示目标点是否是可以穿过的</param>
        public static SearchResult Search(Vector2 start, Vector2 stop, Func<Vector2, bool> canMoveFunc)
        {
            SearchResult result = new SearchResult();
            //保存路径信息
            RouteDic SearchDic = new RouteDic();

            //首先校验一下终点是否可行
            if (!canMoveFunc(stop))
            {
                result.Success = false;
                result.ResultList = new List<Vector2>() { stop };
                result.SearchAeraCount = SearchDic.Count;
                return result;
            }
            
            //待检测点队列
            List<Vector2> queue = new List<Vector2>();

            SearchDic[start] = 0;
            queue.Add(start);

            Vector2 v = start;
            bool flag = true;

            //添加点进队列
            bool AddNextRoute(Vector2 ov)
            {
                if (canMoveFunc(v + ov) && !SearchDic.ContainsKey(v + ov))
                {
                    Vector2 np = v + ov;
                    SearchDic[np] = SearchDic[v] + 1;
                    if (np == stop)
                    {
                        flag = false;
                    }
                    else if (np.Distance(stop) <= v.Distance(stop))
                        queue.Insert(0, np);
                    else
                        queue.Add(np);
                }
                else if ((v + ov) == stop)
                {
                    flag = false;
                }
                return true;
            };

            while (flag)
            {
                if (queue.Count == 0)
                {
                    result.Success = false;
                    result.ResultList = new List<Vector2>() { stop };
                    result.SearchAeraCount = SearchDic.Count;
                    return result;
                }
                else
                {
                    //取第一个点,值最小的点
                    v = queue[0];
                    //添加周围的点
                    AddNextRoute(new Vector2(1, 0));
                    AddNextRoute(new Vector2(-1, 0));
                    AddNextRoute(new Vector2(0, 1));
                    AddNextRoute(new Vector2(0, -1));
                    //去掉这个点
                    queue.Remove(v);
                }
            }
            //结果路径
            List<Vector2> path = new List<Vector2>();
            path.Add(stop);
            while (true)
            {
                v = path.First();

                if (v == start)
                {
                    break;
                }
                //将周围最小的一个点加入下一步
                List<RouteData> minSet = new List<RouteData>();
                minSet.Add(new RouteData(v + Vector2.Left, SearchDic));
                minSet.Add(new RouteData(v + Vector2.Right, SearchDic));
                minSet.Add(new RouteData(v + Vector2.Up, SearchDic));
                minSet.Add(new RouteData(v + Vector2.Down, SearchDic));
                minSet.Sort();
                path.Insert(0, minSet[0].vector);
            }

            result.ResultList = path;
            result.SearchAeraCount = SearchDic.Count;
            return result;
        }

        /// <summary>
        /// 返回distanse范围内所有可以通过的点
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="expandWidth">搜索范围</param>
        /// <param name="canMoveFunc">点是否是可以通过的</param>
        /// <returns>点列表</returns>
        public static List<Vector2> ExpandAera(Vector2 startPos, int expandWidth, Func<Vector2, bool> canMoveFunc)
        {
            RouteDic SearchDic = new RouteDic();
            //索引队列
            List<Vector2> queue = new List<Vector2>();
            List<Vector2> avaliablePos = new List<Vector2>(); 

            Vector2 start = startPos;

            SearchDic[start] = 0;
            queue.Add(start);

            Vector2 p = start;
            bool flag = true;

            bool AddNextRoute(Vector2 ov)
            {
                if (canMoveFunc(p + ov) && !SearchDic.ContainsKey(p + ov))
                {
                    if (SearchDic[p] + 1 > expandWidth)
                    {
                        //queue.Remove(p);
                        flag = false;
                        return false;
                    }
                    SearchDic[p + ov] = SearchDic[p] + 1;
                    queue.Add(p + ov);
                    avaliablePos.Add(p + ov);
                }
                return true;
            };

            while (flag)
            {
                if (queue.Count == 0)
                {
                    avaliablePos.Add(start);
                    return avaliablePos;
                }
                else
                {
                    //初始化当前这个点
                    //取第一个点,值最小的点
                    p = queue[0];
                    AddNextRoute(new Vector2(1, 0));
                    AddNextRoute(new Vector2(-1, 0));
                    AddNextRoute(new Vector2(0, 1));
                    AddNextRoute(new Vector2(0, -1));

                    //去掉这个点
                    queue.Remove(p);
                }
            }
            avaliablePos.Add(start);
            return avaliablePos;
        }

    }
}

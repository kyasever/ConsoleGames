using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Destroy;

namespace Wizard2
{
    public static class ExtandVector2Int
    {
        /// <summary>
        /// 给Vector2扩展一个Distanse方法,两个点的距离
        /// </summary>
        public static int Distanse(this Vector2Int _Vector2Int, Vector2Int otherV)
        {
            return Math.Abs(otherV.X - _Vector2Int.X) + Math.Abs(otherV.Y - _Vector2Int.Y);
        }
    }

    public class SearchResult
    {
        public bool Success = true;
        public List<Vector2Int> resultList;
        public int SearchAera;
    }

    public class NavMesh
    {
        public static SearchResult Search(Vector2Int start, Vector2Int end)
        {
            return Search(start, end, CanMoveInPhysics);
        }

        /// <summary>
        /// 通常的一种搜寻判定,所有物理系统的物体均不可穿过
        /// </summary>
        public static bool CanMoveInPhysics(Vector2Int v)
        {
            var dic = RuntimeEngine.GetSystem<CollisionSystem>().Colliders;
            if (dic.ContainsKey(v))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool CanMoveInAll(Vector2Int v)
        {
            return true;
        }

        #region 搜索辅助的数据结构
        public class RouteDic : IDictionary<Vector2Int, int>
        {
            Dictionary<Vector2Int, int> SearchDic = new Dictionary<Vector2Int, int>();

            public int this[Vector2Int key]
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
                set => ((IDictionary<Vector2Int, int>)SearchDic)[key] = value;
            }

            public ICollection<Vector2Int> Keys => ((IDictionary<Vector2Int, int>)SearchDic).Keys;

            public ICollection<int> Values => ((IDictionary<Vector2Int, int>)SearchDic).Values;

            public int Count => ((IDictionary<Vector2Int, int>)SearchDic).Count;

            public bool IsReadOnly => ((IDictionary<Vector2Int, int>)SearchDic).IsReadOnly;

            public void Add(Vector2Int key, int value)
            {
                SearchDic.Add(key, value);
            }

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
        }

        public class RouteSort : IComparable
        {
            public Vector2Int vector;
            public int depth;

            public RouteSort(Vector2Int v, int i)
            {
                vector = v;
                depth = i;
            }

            public RouteSort(Vector2Int v, RouteDic dic)
            {
                vector = v;
                depth = dic[v];
            }

            public int CompareTo(object obj)
            {
                return depth.CompareTo(((RouteSort)obj).depth);
            }
        }

        #endregion

        /// <summary>
        /// 基于距离判定的优化DFS算法 起点终点,返回路径点列表
        /// </summary>
        /// <param name="start">起始点</param>
        /// <param name="stop">终点</param>
        /// <param name="canMoveFunc">一个方法用于指示目标点是否是可以穿过的</param>
        public static SearchResult Search(Vector2Int start, Vector2Int stop, Func<Vector2Int, bool> canMoveFunc)
        {
            SearchResult result = new SearchResult();
            RouteDic SearchDic = new RouteDic();

            //当不存在key就为无穷大,存在key就是有值,不限边界
            //bfs = new int[x, y];

            //索引队列
            List<Vector2Int> queue = new List<Vector2Int>();

            SearchDic.Add(start, 0);
            queue.Add(start);

            Vector2Int p = start;
            bool flag = true;


            bool AddNextRoute(Vector2Int ov)
            {
                if (canMoveFunc(p + ov) && !SearchDic.ContainsKey(p + ov))
                {
                    Vector2Int np = p + ov;
                    SearchDic.Add(np, SearchDic[p] + 1);
                    if (np == stop)
                    {
                        flag = false;
                    }
                    else if (np.Distanse(stop) <= p.Distanse(stop))
                        queue.Insert(0, np);
                    else
                        queue.Add(np);
                }
                else if ((p + ov) == stop)
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
                    result.resultList = new List<Vector2Int>() { stop };
                    result.SearchAera = SearchDic.Count;
                    return result;
                }
                else
                {
                    //初始化当前这个点
                    //取第一个点,值最小的点
                    p = queue[0];
                    AddNextRoute(new Vector2Int(1, 0));
                    AddNextRoute(new Vector2Int(-1, 0));
                    AddNextRoute(new Vector2Int(0, 1));
                    AddNextRoute(new Vector2Int(0, -1));

                    //去掉这个点
                    queue.Remove(p);
                }
            }
            //搜寻路径
            List<Vector2Int> path = new List<Vector2Int>();
            path.Add(stop);
            while (true)
            {
                p = path.First();

                if (p == start)
                {
                    break;
                }

                List<RouteSort> minSet = new List<RouteSort>();
                minSet.Add(new RouteSort(p + Vector2Int.Left, SearchDic));
                minSet.Add(new RouteSort(p + Vector2Int.Right, SearchDic));
                minSet.Add(new RouteSort(p + Vector2Int.Up, SearchDic));
                minSet.Add(new RouteSort(p + Vector2Int.Down, SearchDic));
                minSet.Sort();
                path.Insert(0, minSet[0].vector);
            }

            result.resultList = path;
            result.SearchAera = SearchDic.Count;
            return result;
        }

        public static List<Vector2Int> ExpandAera(int distanse, Func<Vector2Int, bool> canMoveFunc)
        {
            RouteDic SearchDic = new RouteDic();
            //索引队列
            List<Vector2Int> queue = new List<Vector2Int>();
            List<Vector2Int> avaliablePos = new List<Vector2Int>();

            Vector2Int start = new Vector2Int(0, 0);

            SearchDic.Add(start, 0);
            queue.Add(start);

            Vector2Int p = start;
            bool flag = true;

            bool AddNextRoute(Vector2Int ov)
            {
                if (canMoveFunc(p + ov) && !SearchDic.ContainsKey(p + ov))
                {
                    if (SearchDic[p] + 1 > distanse)
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
                    AddNextRoute(new Vector2Int(1, 0));
                    AddNextRoute(new Vector2Int(-1, 0));
                    AddNextRoute(new Vector2Int(0, 1));
                    AddNextRoute(new Vector2Int(0, -1));

                    //去掉这个点
                    queue.Remove(p);
                }
            }
            avaliablePos.Add(start);
            return avaliablePos;
        }

    }

}

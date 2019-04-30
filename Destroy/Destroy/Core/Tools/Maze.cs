using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destroy
{
    /*
    /// <summary>
    /// 迷宫生成
    /// </summary>
    public class MazeGeneration
    {
        /// <summary>
        /// 生成迷宫对象.带碰撞体
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <param name="wallRender">墙壁使用的贴图</param>
        /// <returns></returns>
        public static GameObject GetMaze(int height, int width, RenderPoint wallRender)
        {
            GameObject gameObject = new GameObject("Maze", "Maze");
            Mesh mesh = gameObject.AddComponent<Mesh>();
            mesh.Init(GetMazeMesh(height, width));
            Debug.Log("迷宫生成完毕,生成墙壁" + mesh.PosList.Count);

            gameObject.AddComponent<Collider>();
            Renderer renderer = gameObject.AddComponent<Renderer>();
            renderer.Rendering(wallRender);

            return gameObject;
        }

        /// <summary>
        /// 生成迷宫Mesh信息 包含的信息为墙的位置
        /// </summary>
        public static List<Vector2> GetMazeMesh(int height, int width)
        { 
            Random random = new Random(456112332);

            if (height % 2 != 1 || width % 2 != 1)
            {
                Debug.Error("生成算法要求迷宫的长宽为奇数");
                return null;
            }

            GameObject gameObject = new GameObject();

            //存储墙的位置.开始时包含所有范围内的点
            List<Vector2> WallPos = new List<Vector2>();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    WallPos.Add(new Vector2(x, y));
                }
            }

            //关键点,一定是空白的. 未访问过的点.开始时所有关键点都在里面
            List<Vector2> AvailablePos = new List<Vector2>();
            for (int y = 1; y < height; y += 2)
            {
                for (int x = 1; x < width; x += 2)
                {
                    AvailablePos.Add(new Vector2(x, y));
                }
            }
            //先挖掉默认空格
            WallPos.RemoveAll(AvailablePos.Contains<Vector2>);
            
            //已经访问过的点
            List<Vector2> ArrivedPos = new List<Vector2>();

            //路径点
            List<Vector2> RoutePos = new List<Vector2>();

            Vector2 startPos = AvailablePos[random.Next(0, AvailablePos.Count - 1)];
            
            //随机找一个点作为起点.递归连线,直到可用点都用光了
            LinkTo(startPos);
            
            //把路径上的墙挖掉
            WallPos.RemoveAll(RoutePos.Contains<Vector2>);

            //挖掉左下角和右上角
            //WallPos.Remove(new Vector2(0, 1));
            WallPos.Remove(new Vector2(width - 1, height - 2));

            return WallPos;

            void LinkTo(Vector2 currert)
            {
                //可供连接的目标点
                List<Vector2> canLinkPos = new List<Vector2>();

                if (AvailablePos.Contains(currert + Vector2.Up * 2))
                {
                    canLinkPos.Add(currert + Vector2.Up * 2);
                }
                if (AvailablePos.Contains(currert + Vector2.Down * 2))
                {
                    canLinkPos.Add(currert + Vector2.Down * 2);
                }
                if (AvailablePos.Contains(currert + Vector2.Left * 2))
                {
                    canLinkPos.Add(currert + Vector2.Left * 2);
                }
                if (AvailablePos.Contains(currert + Vector2.Right * 2))
                {
                    canLinkPos.Add(currert + Vector2.Right * 2);
                }

                //如果这是一个全新的点,那么将其从可用列表移动至已经检索过列表
                if(!ArrivedPos.Contains(currert))
                {
                    //将当前点加入已检索点列表
                    AvailablePos.Remove(currert);
                    ArrivedPos.Add(currert);
                }


                //如果已经走到死路上了,那么就将这个点转入闭锁点
                if (canLinkPos.Count == 0)
                {
                    ArrivedPos.Remove(currert);
                    //ClosePos.Add(currert);
                    
                    if (ArrivedPos.Count == 0)
                        return;
                    //从已经检索过但是还没有封闭的点中随机选取一个,继续探
                    else
                    {
                        int r = random.Next(0, ArrivedPos.Count - 1);
                        LinkTo(ArrivedPos[r]);
                    }
                }
                else
                {
                    int r = random.Next(0, canLinkPos.Count - 1);
                    Vector2 nextPos = canLinkPos[r];
                    //打通中间的墙
                    RoutePos.Add((currert + nextPos) / 2);
                    //开始检索下一个点
                    LinkTo(nextPos);
                }
               
            }
        }
    }
    */
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Destroy;

namespace Destroy.Standard
{
    /// <summary>
    /// Layer划分,越上面的显示的越靠前,数字可以随意改动,但是最好使用枚举来进行数字排列避免出错
    /// </summary>
    public enum Layer
    {
        /// <summary>
        /// 
        /// </summary>
        Cursor = 8,
        /// <summary>
        /// 
        /// </summary>
        Environment = 9,
        /// <summary>
        /// 
        /// </summary>
        Agent = 10,
        /// <summary>
        /// 
        /// </summary>
        MoveRoute = 11,
        /// <summary>
        /// 
        /// </summary>
        MoveAera = 12,
    }

    /// <summary>
    /// 可移动范围指示器
    /// </summary>
    public class MoveIndicator : Script
    {
        /// <summary>
        /// 
        /// </summary>
        public override void Awake()
        {
            Renderer.Depth = (int)Layer.MoveAera;
            SetActive(false);
        }

        /// <summary>
        /// 沿中心点展开一个范围指示器
        /// </summary>
        /// <param name="center">中心点</param>
        /// <param name="expandWidth">展开范围</param>
        public void ExpandAera(Vector2 center, int expandWidth)
        {
            //获取该展开的范围
            List<Vector2> list = Navigation.ExpandAera(center, expandWidth, Navigation.CanMoveInPhysics);
            Refresh();
            //使用蓝色填充该范围
            Draw(list, Color.Blue);
        }

        /// <summary>
        /// 设置是否激活/显示
        /// </summary>
        public void SetActive(bool active)
        {
            GameObject.SetActive(active);
        }

        /// <summary>
        /// 展开的范围是否包括某一个点
        /// </summary>
        public bool Contains(Vector2 v)
        {
            return Renderer.RendererPoints.ContainsKey(v);
        }
    }

    /// <summary>
    /// 路径指示器
    /// </summary>
    public class RouteIndicator : Script
    {
        /// <summary>
        /// 
        /// </summary>
        public override void Awake()
        {
            Renderer.Depth = (int)Layer.MoveRoute;
            SetActive(false);
        }

        public int PosCount { get { return RendererPoints.Count; } }

        /// <summary>
        /// 沿路径展开一个路径指示器
        /// </summary>
        /// <param name="beginPos">起点</param>
        /// <param name="endPos">终点</param>
        public void SearchRoute(Vector2 beginPos, Vector2 endPos)
        {
            List<Vector2> list = Navigation.Search(beginPos, endPos).ResultList;
            Refresh();
            Draw(list, Color.Green);
        }

        /// <summary>
        /// 设置是否显示
        /// </summary>
        public void SetActive(bool active)
        {
            GameObject.SetActive(active);
        }



    }

    /// <summary>
    /// 鼠标光标. 当鼠标位于屏幕边缘时会自动滚屏
    /// </summary>
    public class Cursor : Script
    {
        /// <summary>
        /// 鼠标滚屏速度
        /// </summary>
        public float MoveSpeed = 10;
        private float MoveInterval;
        private float MoveTime = 0f;

        private static Cursor instanse;
        /// <summary>
        /// 单例对象
        /// </summary>
        public static Cursor Instance
        {
            get
            {
                if (instanse == null)
                {
                    instanse = CreateCursor();
                }
                return instanse;
            }
        }

        private static Cursor CreateCursor()
        {
            var cursorCom = Actor.CreateWith<Cursor>("Cursor", "Cursor");
            cursorCom.Renderer.Depth = (int)Layer.Cursor;
            cursorCom.Draw(Vector2.Zero, Color.DarkRed);
            return cursorCom;
        }

        /// <summary>
        /// 更新摄像机
        /// </summary>
        public override void Update()
        {
            this.Position = Input.MousePosition;

            //当光标位于屏幕边缘时,移动摄像机
            Vector2 moveDir = Vector2.Zero;
            if (Position.X == Camera.Main.Position.X)
            {
                moveDir.X = -1;
            }
            else if (Position.X == Camera.Main.Position.X + Config.ScreenWidth - 1)
            {
                moveDir.X = 1;
            }
            if (Position.Y == Camera.Main.Position.Y)
            {
                moveDir.Y = -1;
            }
            else if (Position.Y == Camera.Main.Position.Y + Config.ScreenHeight - 1)
            {
                moveDir.Y = 1;
            }
            if (moveDir != Vector2.Zero)
            {
                MoveInterval = 1 / MoveSpeed;
                MoveTime += Time.DeltaTime;
                if (MoveTime > MoveInterval)
                {
                    MoveTime = 0f;
                    Camera.Main.Position += moveDir;
                }
            }

        }
    }
}

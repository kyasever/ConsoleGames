using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destroy.Standard
{
    /// <summary>
    /// 战棋策略游戏的角色控制器
    /// </summary>
    public class SRPGAgent : Script
    {
        public MoveIndicator moveAera;
        public RouteIndicator routeAera;

        /// <summary>
        /// 
        /// </summary>
        public override void Awake()
        {
            Renderer.Depth = (int)Layer.Agent;
            DrawString("岩", new Destroy.Color(222, 178, 222),Config.DefaultBackColor);
        }

        /// <summary>
        /// 角色状态
        /// </summary>
        public enum State
        {
            /// <summary>
            /// 待机
            /// </summary>
            None,
            /// <summary>
            /// 移动
            /// </summary>
            Move,
            /// <summary>
            /// 搜索
            /// </summary>
            Search,
        }
        /// <summary>
        /// 角色状态
        /// </summary>
        [ShowInInspector]
        public State state = State.None;

        private Vector2 lastPos = Vector2.Zero;

        /// <summary>
        /// 
        /// </summary>
        public override void Update()
        {
            switch (state)
            {
                case State.None:
                    if (Input.MousePosition == this.Position)
                    {
                        if (Input.GetMouseButtonUp(MouseButton.Left))
                        {
                            Debug.Log(GameObject.Name + "ExpandAera:10");
                            moveAera.SetActive(true);
                            moveAera.ExpandAera(Position, 10);
                            state = State.Move;
                        }
                        else if (Input.GetMouseButtonUp(MouseButton.Right))
                        {
                            Debug.Log(GameObject.Name + "SearchMode");
                            state = State.Search;
                        }
                    }
                    break;
                case State.Move:
                    //Debug.Log(Cursor.Instanse.Position.ToString() + moveAera.Contains(Cursor.Instanse.Position).ToString());
                    Vector2 cursorPos = Cursor.Instance.Position;
                    if (moveAera.Contains(Cursor.Instance.Position))
                    {
                        if (cursorPos != lastPos)
                        {
                            routeAera.SetActive(true);
                            routeAera.SearchRoute(Position, Cursor.Instance.Position);
                        }
                        if (Input.GetMouseButtonUp(MouseButton.Left))
                        {
                            Debug.Log(GameObject.Name + "MoveTo:" + cursorPos.ToString());
                            Position = cursorPos;
                            routeAera.SetActive(false);
                            moveAera.SetActive(false);
                            state = State.None;
                        }
                    }
                    else
                    {
                        routeAera.SetActive(false);
                    }
                    lastPos = cursorPos;
                    break;
                case State.Search:
                    cursorPos = Cursor.Instance.Position;
                    if (cursorPos != lastPos)
                    {
                        routeAera.SetActive(true);
                        routeAera.SearchRoute(Position, Cursor.Instance.Position);
                    }
                    if (Input.GetMouseButtonUp(MouseButton.Left))
                    {
                        if (routeAera.PosCount < 2)
                        {
                            return;
                        }
                        Debug.Log(GameObject.Name + "MoveTo:" + cursorPos.ToString());
                        Position = cursorPos;
                        routeAera.SetActive(false);
                        moveAera.SetActive(false);
                        state = State.None;
                    }
                    lastPos = cursorPos;
                    break;
            }

        }

    }
}

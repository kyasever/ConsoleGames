namespace Destroy.Standard
{
    /// <summary>
    /// 战棋策略游戏的角色控制器
    /// </summary>
    public class SRPGAgent : Script
    {
        public MoveIndicator aera;
        public RouteIndicator routeAera;

        /// <summary>
        /// 
        /// </summary>
        public override void Awake()
        {
            Renderer.Depth = (int)Layer.Agent;
            DrawString("岩", new Destroy.Color(222, 178, 222), Config.DefaultBackColor);
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
            /// <summary>
            /// 瞄准准备攻击
            /// </summary>
            Aim,
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
            Vector2 cursorPos = Cursor.Instance.Position;
            bool needRefresh = (cursorPos != lastPos);
            switch (state)
            {
                case State.None:
                    if (Input.MousePosition == this.Position)
                    {
                        if (Input.GetMouseButtonUp(MouseButton.Left))
                        {
                            Debug.Log(GameObject.Name + "ExpandAera:10");
                            aera.SetActive(true);
                            aera.ExpandAera(Position, 10);
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
                    if (aera.Contains(Cursor.Instance.Position))
                    {
                        if (needRefresh)
                        {
                            routeAera.SetActive(true);
                            routeAera.SearchRoute(Position, Cursor.Instance.Position);
                        }
                        if (Input.GetMouseButtonUp(MouseButton.Left))
                        {
                            Debug.Log(GameObject.Name + "MoveTo:" + cursorPos.ToString());
                            Position = cursorPos;
                            routeAera.SetActive(false);

                            //展开攻击范围
                            aera.ExpandAera(Position, 2, Color.Red);
                            aera.SetActive(true);

                            //移动阶段结束,进入攻击阶段
                            state = State.Aim;
                        }
                    }
                    else
                    {
                        routeAera.SetActive(false);
                    }
                    break;
                case State.Search:
                    if (needRefresh)
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
                        aera.SetActive(false);
                        state = State.None;
                    }
                    break;
                case State.Aim:
                    if (aera.Contains(Cursor.Instance.Position))
                    {
                        if (needRefresh)
                        {
                            Cursor.Instance.ChangeColor(true);
                        }
                        if (Input.GetMouseButtonUp(MouseButton.Left))
                        {
                            Debug.Log(GameObject.Name + "attack:" + cursorPos.ToString());

                            aera.SetActive(false);
                            //攻击结束 回归初始状态
                            state = State.None;
                        }
                    }
                    else
                    {
                        if(needRefresh)
                            Cursor.Instance.ChangeColor(false);
                        routeAera.SetActive(false);
                    }
                    break;
            }
            lastPos = cursorPos;

        }

    }
}

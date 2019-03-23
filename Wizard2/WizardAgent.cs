using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Destroy;
using Destroy.Example;

namespace Wizard2
{
    public abstract class SingletonScript<T> : Script where T : new()
    {
        private static T instance;
        /// <summary>
        /// 单例
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance == null)
                    instance = new T();
                return instance;
            }
        }
    }

    /// <summary>
    /// Layer划分,越上面的显示的越靠前,数字可以随意改动,但是最好使用枚举来进行数字排列避免出错
    /// </summary>
    public enum Layer
    {
        Cursor = 8,
        Environment = 9,
        Agent = 10,
        MoveRoute = 11,
        MoveAera = 12,
    }

    public class GameMode
    {
        private static MoveIndicator moveIndicator;
        public static MoveIndicator MoveIndicator
        {
            get
            {
                if(moveIndicator == null)
                {
                    moveIndicator = GameObject.CreateWith<MoveIndicator>("MoveIndicator", "World");
                }
                return moveIndicator;
            }
        }

        private static RouteIndicator routeIndicator;
        public static RouteIndicator RouteIndicator
        {
            get
            {
                if (routeIndicator == null)
                {
                    routeIndicator = GameObject.CreateWith<RouteIndicator>("RouteIndicator", "World");
                }
                return routeIndicator;
            }
        }

        public static GameMode Instance; 
    }


    public static class AgentFactory
    {
        public static WizardAgent CreatePlayerAgent(Vector2 Location)
        {
            GameObject gameObject = new GameObject("PlayerAgent", "Player");
            gameObject.Position = Location;

            gameObject.AddComponent<Mesh>();
            Renderer renderer = gameObject.AddComponent<Renderer>();
            renderer.Depth = (int)Layer.Agent;
            renderer.Rendering("岩", new Color(222, 178, 222), Config.DefaultBackColor);

            WizardAgent wizardAgent = gameObject.AddComponent<WizardAgent>();
            return wizardAgent;
        }
    }

    /// <summary>
    /// 可移动范围指示器
    /// </summary>
    public class MoveIndicator : Script 
    {
        private Mesh mesh;
        private Renderer renderer;

        public override void Awake()
        {
            mesh = AddComponent<Mesh>();
            renderer = AddComponent<Renderer>();
            SetActive(false);
        }

        public void ExpandAera(Vector2 center,int expandWidth)
        {
            List<Vector2> list = NavMesh.ExpandAera(center, expandWidth, NavMesh.CanMoveInPhysics);
            mesh.Init(list);

            RenderPoint rp = new RenderPoint("  ", Color.Blue, Color.Blue, (int)Layer.MoveAera);
            renderer.Rendering(rp);
        }

        public void SetActive(bool active)
        {
            GameObject.SetActive(active);
        }

        public bool Contains(Vector2 v)
        {
            return mesh.PosList.Contains(v);
        }
    }

    /// <summary>
    /// 路径指示器
    /// </summary>
    public class RouteIndicator : Script
    {
        private Mesh mesh;
        private Renderer renderer;

        public override void Awake()
        {
            mesh = AddComponent<Mesh>();
            renderer = AddComponent<Renderer>();
            renderer.Depth = (int)Layer.MoveRoute;
        }

        public void SearchRoute(Vector2 beginPos, Vector2 endPos)
        {
            List<Vector2> list = NavMesh.Search(beginPos, endPos).ResultList;
            mesh.Init(list);

            RenderPoint rp = new RenderPoint("  ", Config.DefaultForeColor, Color.Green, (int)Layer.MoveRoute);
            renderer.Rendering(rp);
        }

        public void SetActive(bool active)
        {
            GameObject.SetActive(active);
        }
    }

    public class WizardAgent : Script
    {
        public MoveIndicator moveAera;
        public RouteIndicator routeAera;
        public GameMode gameMode;

        public override void Awake()
        {
            moveAera = GameMode.MoveIndicator;
            routeAera = GameMode.RouteIndicator;
            gameMode = GameMode.Instance;
        }

        public bool IsClicked
        {
            get
            {
                if (Input.MousePosition == this.Position)
                {
                    if (Input.GetMouseButtonUp(MouseButton.Left))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public enum State
        {
            None,
            Move,
            Attack,
        }
        [ShowInInspector]
        public State state = State.None;

        private Vector2 lastPos = Vector2.Zero;
        public override void Update()
        {
            switch (state)
            {
                case State.None:
                    if (IsClicked)
                    {
                        Debug.Log(GameObject.Name + "ExpandAera:5");
                        moveAera.SetActive(true);
                        moveAera.ExpandAera(Position, 8);
                        state = State.Move;
                    }
                    break;
                case State.Move:
                    //Debug.Log(Cursor.Instanse.Position.ToString() + moveAera.Contains(Cursor.Instanse.Position).ToString());
                    Vector2 cursorPos = Cursor.Instance.Position;
                    if (moveAera.Contains(Cursor.Instance.Position))
                    {
                        if(cursorPos != lastPos)
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
            }

        }

    }
}

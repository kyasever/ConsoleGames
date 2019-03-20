using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Destroy;
namespace Wizard2
{

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

    public static class AgentFactory
    {
        public static WizardAgent CreatePlayerAgent(Vector2 Location)
        {
            //这两个东西不应该属于某一个,而是公共的
            //可移动区域
            GameObject moveAera = new GameObject("MoveAera", "Player");
            MoveAera moveAeraCom = moveAera.AddComponent<MoveAera>();

            //移动轨迹区域
            GameObject moveRoute = new GameObject("MoveRoute", "Player");
            MoveRoute moveRouteCom = moveRoute.AddComponent<MoveRoute>();


            GameObject gameObject = new GameObject("PlayerAgent", "Player");
            gameObject.Position = Location;

            gameObject.AddComponent<Mesh>();
            Renderer renderer = gameObject.AddComponent<Renderer>();
            renderer.Depth = (int)Layer.Agent;
            renderer.Rendering("岩", new Colour(222, 178, 222), Config.DefaultBackColor);

            WizardAgent wizardAgent = gameObject.AddComponent<WizardAgent>();
            moveAera.SetActive(false);
            wizardAgent.moveAera = moveAeraCom;
            moveRoute.SetActive(false);
            wizardAgent.moveRoute = moveRouteCom;
            return wizardAgent;

        }
    }

    /// <summary>
    /// 移动范围组件
    /// </summary>
    public class MoveAera : Script
    {
        private Mesh mesh;
        private Renderer renderer;

        public override void Awake()
        {

            mesh = AddComponent<Mesh>();
            renderer = AddComponent<Renderer>();
            renderer.Depth = (int)Layer.MoveAera;
        }

        public void ExpandAera(Vector2 center,int expandWidth)
        {
            List<Vector2> list = NavMesh.ExpandAera(center, expandWidth, NavMesh.CanMoveInPhysics);
            mesh.Init(list);

            RenderPoint rp = new RenderPoint("  ", Colour.Blue, Colour.Blue, (int)Layer.MoveAera);
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

    public class MoveRoute : Script
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

            RenderPoint rp = new RenderPoint("  ", Config.DefaultForeColor, Colour.Green, (int)Layer.MoveRoute);
            renderer.Rendering(rp);
        }

        public void SetActive(bool active)
        {
            GameObject.SetActive(active);
        }
    }

    public class WizardAgent : Script
    {
        public MoveAera moveAera;
        public MoveRoute moveRoute;

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
                    Vector2 cursorPos = Cursor.Instanse.Position;
                    if (moveAera.Contains(Cursor.Instanse.Position))
                    {
                        if(cursorPos != lastPos)
                        {
                            moveRoute.SetActive(true);
                            moveRoute.SearchRoute(Position, Cursor.Instanse.Position);
                        }
                        if (Input.GetMouseButtonUp(MouseButton.Left))
                        {
                            Debug.Log(GameObject.Name + "MoveTo:" + cursorPos.ToString());
                            Position = cursorPos;
                            moveRoute.SetActive(false);
                            moveAera.SetActive(false);
                            state = State.None;
                        }
                    }
                    else
                    {
                        moveRoute.SetActive(false);
                    }
                    lastPos = cursorPos;
                    break;
            }

        }

    }
}

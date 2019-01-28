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
        Agent = 10,
        MoveRoute = 11,
        MoveAera = 12,
    }

    public static class AgentFactory
    {
        public static WizardAgent CreatePlayerAgent(Vector2Int Location)
        {
            GameObject gameObject = new GameObject("PlayerAgent", "Player");
            gameObject.Position = Location;

            gameObject.AddComponent<Mesh>();
            Renderer renderer = gameObject.AddComponent<Renderer>();
            renderer.Depth = (int)Layer.Agent;
            renderer.Rendering("岩");

            //可移动区域
            GameObject moveAera = new GameObject("MoveAera", "Player");
            moveAera.Parent = gameObject;
            MoveAera moveAeraCom = moveAera.AddComponent<MoveAera>();

            //移动轨迹区域
            GameObject moveRoute = new GameObject("MoveRoute", "Player");
            moveRoute.Parent = gameObject;
            MoveRoute moveRouteCom = moveRoute.AddComponent<MoveRoute>();



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
            LocalPosition = new Vector2Int(0, 0);
            mesh = AddComponent<Mesh>();
            renderer = AddComponent<Renderer>();
            renderer.Depth = (int)Layer.MoveAera;
        }

        public void SetAera(List<Vector2Int> list)
        {
            mesh.Init(list);
            Dictionary<Vector2Int, RenderPoint> rendererDic = new Dictionary<Vector2Int, RenderPoint>();
            RenderPoint rp = new RenderPoint("  ", Colour.Blue, Colour.Blue, (int)Layer.MoveAera);
            foreach(var v in list)
            {
                rendererDic.Add(v, rp);
            }
            renderer.Rendering(rendererDic);
        }

        public void SetActive(bool active)
        {
            GameObject.SetActive(active);
        }

        public void ExpandAera(int length)
        {
            SetAera( NavMesh.ExpandAera(length, NavMesh.CanMoveInAll));
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

        public void SetAera(List<Vector2Int> list)
        {
            mesh.Init(list);
            renderer.Rendering(" ", Colour.Blue, Colour.Blue);
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

       

        public override void Update()
        {
            if(Input.MousePosition == this.Position)
            {
                if(Input.GetMouseButtonDown( MouseButton.Left))
                {
                    moveAera.SetActive(true);
                    moveAera.ExpandAera(2);
                }
            }
        }

    }
}

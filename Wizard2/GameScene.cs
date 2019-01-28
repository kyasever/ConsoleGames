using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Destroy;
using Destroy.Template;
namespace Wizard2
{
    public class GameScene : Scene
    {
        public GameScene(string name):base(name)
        {

        }

        public override void OnDestroy()
        {
            
        }

        public override void OnStart()
        {
            AgentFactory.CreatePlayerAgent(new Vector2Int(10,10));
            SystemUIFactroy.GetTimerUI(new Vector2Int(20, 5));
        }
    }

}

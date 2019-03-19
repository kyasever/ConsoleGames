using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Destroy;
using Destroy.Example;
using Destroy.Winform;
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

            Resource.Init();
            Debug.Log("查找目录Resouce,资源数量:" + Resource.Name_Path.Keys.Count);
            foreach(var s in Resource.Name_Path.Keys)
            {
                Debug.Log(s);
            }

            string str = Resource.Load<string>("Map");
            Debug.Log(str);
        }
    }

}

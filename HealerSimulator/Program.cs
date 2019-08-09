using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Destroy;
using Destroy.Standard;
using Destroy.Winform;

namespace HealerSimulator
{
    static class Program
    {
        private static void Main()
        {
            Config.TickPerSecond = 120;
            Config.ScreenWidth = 70;
            Config.ScreenHeight = 40;
            Config.CharWidth = CharWidthEnum.Double;
            //用编辑器模式开始游戏
            WinformEngine.OpenWithEditor(StartGame);
        }

        private static void StartGame()
        {
            //开始游戏的主逻辑
            GameMode.Instance.Init();
            //开启游戏进入场景
            StartScene scene = new StartScene();
            SceneManager.Load(scene, LoadSceneMode.Single);
            //游戏的主场景本质上不控制任何逻辑,只是与数据进行绑定- 更改 - 显示等.
            //游戏的数据都由gameMode说的算
        }
    }

    public class StartScene : Scene
    {
        //开启游戏主场景
        MainScene scene;
        public override void OnStart()
        {
            Destroy.Button btn = UIFactroy.CreateButton(new Vector2(20, 20), "开始游戏", 
                () => { scene = new MainScene(); SceneManager.Load(scene, LoadSceneMode.Single);   });





            var text = Utils.CreateTextAera(20, 5, true);
            text.Position = new Vector2(10, 10);
            text.Rendering("欢迎来到本游戏,本游戏目前只有一个试玩关卡.");
        }
      
    }

}

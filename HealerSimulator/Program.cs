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
            //用编辑器模式开始游戏
            WinformEngine.OpenWithEditor(StartGame);
        }

        private static void StartGame()
        {
            //开始游戏的主逻辑
            GameMode.Instance.Init();
            //开启游戏主场景
            StartScene scene = new StartScene();
            //Destroy.Standard.StartScene scene = new Destroy.Standard.StartScene();
            //加载游戏主场景
            SceneManager.Load(scene, LoadSceneMode.Single);
        }
    }

    public class StartScene : Scene
    {
        //开启游戏主场景
        MainScene scene;

        public override void OnStart()
        {
            Destroy.Button btn = UIFactroy.CreateButton(new Vector2(20, 20), "开始游戏", 
                () => { scene = new MainScene(); SceneManager.Load(scene, LoadSceneMode.Single);  });
        }
    }


}

using Destroy;
using Destroy.Winform;
using Destroy.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wizard2
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Config.TickPerSecond = 30;
            Config.ScreenWidth = 40;
            Config.ScreenHeight = 30;
            Config.RendererSize = new Vector2(16, 16);
            //将项目-属性-输出类型调整为windows应用程序
            //使用Winform模式开始游戏并打开Editor.
            //TODO:Winform纯净模式开始游戏,不包含Editor.新建一个Form来干这个
            //Notice:目前取消了对透明面板+原生控制台组合的支持,Winform只用Winform的IO
            WinformEngine.OpenWithEditor(StartGame);
        }

        private static void StartGame()
        {
            //SRPGScene scene = new SRPGScene();
            //var scene = new TPSScene();
            
            var scene = new newTestScene();

            SceneManager.Load(scene, LoadSceneMode.Single);

            Camera.Main.Position = new Vector2(0, 0);

            Debug.Log("Hello Destroy");
        }
    }
    //没有createwith new 就可以直接创建对应的物体.
    public class newTestScene : Scene
    {
        public override void OnStart()
        {
            MyScript mainObj = Actor.CreateWith<MyScript>("Player", "testTag");
        }
    }

    public class MyScript : Script
    {
        public override void Awake()
        {
            Position = new Vector2(15, 15);

            DrawString("asdfasdfasd", Color.Blue, Color.Green, Vector2.Zero);


            Destroy.Standard.Cursor.Instance.Position = new Vector2(10, 10);

            MoveIndicator moveIndicator = Actor.CreateWith<MoveIndicator>("MoveIndicator", "Indicator");
            RouteIndicator routeIndicator = Actor.CreateWith<RouteIndicator>("RouteIndicator", "Indicator");

            SRPGAgent sRPGAgent = Actor.CreateWith<SRPGAgent>("Player", "PP");
            sRPGAgent.Position = new Vector2(10, 10);
            sRPGAgent.moveAera = moveIndicator;
            sRPGAgent.routeAera = routeIndicator;

        }

        public override void Update()
        {
        }

    }

}

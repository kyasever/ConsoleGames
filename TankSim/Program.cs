using System;

namespace TankSim
{
    using Destroy;
    using System.Text;

    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>

        //[STAThread]

        static void Main()
        {
            Config.TickPerSecond = 60;
            Config.ScreenWidth = 20;
            Config.ScreenHeight = 40;
            Config.RendererSize = new Vector2(16, 16);

            WinformEngine.OpenWithEditor(StartGame);
        }

        private static void StartGame()
        {
            var scene = new MainScene();

            SceneManager.Load(scene, LoadSceneMode.Single);

            Camera.Main.Position = new Vector2(0, 0);

            Debug.Log("Hello Destroy");
        }
    }


    //没有createwith new 就可以直接创建对应的物体.
    public class MainScene : Scene
    {
        public override void OnStart()
        {
            UIObject.CreateWith<BossHP>("BOSSHP", "UI");
        }
    }

    public class BossHP : Script
    {
        private ProgressBar slider;

        public override void Awake()
        {
            DrawString("BOSS:");
            DrawString("200/200", new Vector2(3, 0));
            Position = new Vector2(1, 39);
            slider = ProgressBar.Create(10,100,0);
            slider.Parent = gameObject;
            slider.LocalPosition = new Vector2(0, -1);
        }

        public override void Update()
        {
            slider.Value += 0.1f;
            if (slider.Value == 100)
                slider.Value = 0;
        }
    }


}

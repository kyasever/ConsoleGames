using System;

namespace TankSim
{
    using System.Text;
    using Destroy;

    static class Program
    {
        private static void Main()
        {
            //引擎执行帧率
            Config.TickPerSecond = 120;
            //游戏画面的大小
            Config.ScreenWidth = 70;
            Config.ScreenHeight = 40;
            //每一个元单位格子是由一个宽度组成(英文)还是两个宽度组成(汉字)
            Config.CharWidth = CharWidthEnum.Double;
            //用编辑器模式开始游戏
            WinformEngine.OpenWithEditor(StartGame);
        }

        private static void StartGame()
        {
            //开启游戏主场景
            StartScene scene = new StartScene();
            //加载游戏主场景
            SceneManager.Load(scene, LoadSceneMode.Single);
        }
    }

    public class MainScene : Scene
    {

    }

    public class StartScene : Scene
    {
        //开启游戏主场景
        MainScene scene;
        Player player;
        public override void OnStart()
        {
            player = GameObject.CreateWith<Player>();
        }

        public override void OnDestroy()
        {
            SceneManager.DontDestroyOnLoad(player.GameObject);
            scene = new MainScene();
            SceneManager.Load(scene, LoadSceneMode.Single);

            MiutiObject miuti = MiutiObject.Create(false);
            SingleObject single = GameObject.CreateWith<Single>();
        }
    }

    public class Player : Script
    {
        public override void Awake()
        {

        }
        public override void Start()
        {

        }
        public override void Update()
        {

        }
    }

    public class MiutiObject : Script
    {
        public static MiutiObject Create(bool hasCom)
        {
            GameObject gameObject = new GameObject();
            gameObject.AddComponent<Renderer>();
            gameObject.AddComponent<Collider>();
            if(hasCom)
            {
                gameObject.AddComponent<MyScript>();
            }
        }
    }

    public class SingleObject : Script
    {
        public void Awake()
        {
            Renderer rendererCom = AddComponent<Renderer>();
            rendererCom.Rendering("一个简单的对象");
        }
    }
}

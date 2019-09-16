Destroy引擎
===
欢迎来到Destroy引擎的介绍文档,本引擎最开始源于与[Charlie](https://github.com/GreatDestroyerCharlie)在合作的项目,并感谢他对于本项目提供的技术支持.

如果想贡献代码👍欢迎提交[Pull request](https://github.com/kyasever/ConsoleGames/pulls)

同时本工程含有两个控制台游戏可供游玩.可以直接点击链接下载编译后版本,运行需要.netframework4.6.1
* ![图](https://github.com/kyasever/ConsoleGames/blob/master/Resouce/hs.png)
* [点击下载试玩][HealerSimulater(治疗模拟器)](https://github.com/kyasever/ConsoleGames/raw/master/HealerSimulator.rar) 扮演一个团队中的治疗者~~WOW打本模拟器~~ , 拯救团队于危难之中.~~保护我的敌人,痛击我的队友~~
* ![图](https://github.com/kyasever/ConsoleGames/blob/master/Resouce/wa.png)
* [点击下载试玩][WizardAdventrue(巫师冒险)](https://github.com/kyasever/ConsoleGames/raw/master/WizardAdvanture.rar) 控制一队小队探索地下城,战胜敌人,战棋策略游戏.
* [分流下载][Destroy.dll](https://github.com/kyasever/ConsoleGames/raw/master/Destroy.dll)引擎编译后版本.

### 

一.引擎的启动和兼容模式
------
* 在初始化中通过修改Config静态类中的变量来设定引擎的初始化参数,未设置
则为默认推荐参数.
* 同时兼容三种不同的引擎处理模式.分别为GDI+渲染并附带引擎编辑工具的模式,GDI+渲染的纯净游戏模式和基于WindosAPI原生控制台的调用模式.
* 建立一个类继承Scene,来使用引擎的场景管理功能. 
```cs
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
```

二.引擎的场景管理
------
* 每一个创建的游戏物体均属于某一个场景来管理,当一个场景为激活状态时,新建的游戏物体会自动归属于这个场景,当这个场景销毁.或者加载一个新的场景时,场景管理其会自动新建和回收属于这些场景的游戏对象.
```cs
    public class StartScene : Scene
    {
        public override void OnStart()
        {
            Player player = GameObject.CreateWith<Player>();
        }
        public override void OnDestroy()
        {
            SceneManager.DontDestroyOnLoad(player.GameObject);
            scene = new MainScene();
            SceneManager.Load(scene, LoadSceneMode.Single);
        }
    }
```
三.引擎的组件化和游戏对象
-------
* 游戏对象由GameObject类和Component类组成,进行组件化管理.
* 图1
* 通常一个游戏对象的类需要建立GameObject,然后往上面挂载一些组件来完成游戏对象的组装.考虑到代码的数量较大,因此引擎提供了两种推荐的游戏物体初始化方式.
```cs
    ///第一种方式适合带有自定义参数,构成比较复杂的游戏物体的创建.在静态函数中完成游戏物体的构建.
    MiutiObject miuti = MiutiObject.Create(false);
    //第二种方式适合构成比较简单,不需要自定义参数的组件,参数为创建GamaObject的参数.相当于创建一个GO,挂载想要的脚本,然后返回脚本的对象.在脚本的Awake()函数中完成游戏物体的组装
    SingleObject single = GameObject.CreateWith<SingleObject>();

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
```

四.API文档
------
 - [wiki](https://github.com/kyasever/ConsoleGames/wiki)页面现已开放,[点击跳转](https://github.com/kyasever/ConsoleGames/wiki)查看详细文档.
namespace Destroy.Example
{
    using Destroy;
    using System.Collections.Generic;

    public class Program
    {
        private static void Main()
        {
            Config.TickPerSecond = 100;
            Config.ScreenWidth = 40;
            Config.ScreenHeight = 30;

            //将项目-属性-输出类型调整为windows应用程序
            //使用Winform模式开始游戏并打开Editor.
            //TODO:Winform纯净模式开始游戏,不包含Editor.新建一个Form来干这个
            //Notice:目前取消了对透明面板+原生控制台组合的支持,Winform只用Winform的IO
            WinformEngine.OpenWithEditor(StartGame);

            //将项目-属性-输出类型调整为控制台应用程序
            //使用基于Windows的底层IO开始游戏,不包含Editor功能
            //RuntimeEngine.Run(StartGame);
        }

        private static void StartGame()
        {
            ExampleScenePushBox scene = new ExampleScenePushBox();
            SceneManager.Load(scene, LoadSceneMode.Single);
            Debug.Log("Hello Destroy");
        }
    }

    #region 引擎教学.稍后开一个新工程搬过去
    /// <summary>
    /// 通常下建议创建一个这样的静态类来管理一些需要创建的游戏物体
    /// </summary>
    public static class ExampleFactroy
    {
        //启动一个游戏,Main方法
        public static void MainGame()
        {

        }

        //构造一个Destroy引擎的游戏对象
        public static GameObject ASimpleGameObject()
        {
            //------------------
            // GameObject
            //------------------
            //创建一个对象,名字为Name,标签为Tag
            GameObject gameObject = new GameObject("Name", "Tag");
            //将一个物体整体的变为活跃,默认是活跃的
            gameObject.SetActive(true);
            //查看它的子物体和数量信息
            int childCount = gameObject.ChildCount;
            //指定父物体
            GameObject child = new GameObject("child", "Tag");
            child.Parent = gameObject;
            //更改世界坐标
            gameObject.Position = new Vector2Int(10, 10);
            //更改本地坐标(相对于父物体的坐标)
            child.LocalPosition = new Vector2Int(0, 1);

            //------------------
            // Components
            //------------------
            //Mesh组件,表示这个物体的形状
            Mesh mesh = gameObject.AddComponent<Mesh>();
            //手动制定mesh的范围,默认只包括本身一个点.这样就包括两个点了
            mesh.Init(new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(0, 1) });

            //只有挂载了这个组件的物体才能接收到碰撞回调.
            Collider collider = gameObject.AddComponent<Collider>();

            // 只有挂载了这个才能接收到鼠标点击UI事件.
            RayCastTarget rayCastTarget = gameObject.AddComponent<RayCastTarget>();

            //Renderer组件负责物体的显示
            Renderer renderer = gameObject.AddComponent<Renderer>();
            //渲染模式游戏物体位置和摄像机相关,UI和摄像机坐标无关. 深度约大的渲染优先级越低
            renderer.Init(RendererMode.GameObject, 10);
            //调用Rendering方法来进行渲染,重新调用就可以重新渲染了.
            renderer.Rendering("哈哈哈哈啊哈");
            //可以像这样只改变一项
            renderer.SetBackColor(Colour.Blue);
            ColorStringBiulder biulder = new ColorStringBiulder();
            biulder.ForeColor = Colour.Blue;
            biulder.AppendString("蓝色");
            biulder.ForeColor = Colour.Green;
            biulder.AppendString("绿色");
            //也可以像这样渲染,其他渲染方式详见API
            renderer.Rendering(biulder.ToRenderer());


            //------------------
            //Script and Others
            //------------------
            //除了组件之外,也可以挂载其他自定义的脚本.也可以选择使用由DestroyEngine提供的便捷全面的内置脚本

            //角色控制器是一个引擎内置脚本 默认使用WSAD键控制
            CharacterController controller = gameObject.AddComponent<CharacterController>();
            controller.Speed = 10;
            //是否可以穿过其他碰撞体
            controller.CanMoveInCollider = false;

            //挂载一个自定义脚本
            //接下来的内容请跳转MyScript类内部查看
            gameObject.AddComponent<MyScript>();

            //返回游戏物体,引擎的大部分物体都是类似于这样创建的
            return gameObject;
        }
    }

    /// <summary>
    /// 自定义脚本
    /// </summary>
    public class MyScript : Script
    {
        //如果该游戏物体挂有collider,那么它和其他Collider碰撞的时候就可以在这里接收碰撞信息了
        public override void OnCollision(Collision collision)
        {
            //碰撞发生点
            Vector2Int pos = collision.HitPos;
            //其他碰撞体
            Collider other = collision.OtherCollier;
        }

        //创建的时候被调用
        public override void Start()
        {
            //移动组件的Position 相当于移动GameObject的Position
            Position = new Vector2Int(10, 11);
            //获取一个组件,组件和游戏物体均可调用此方法
            Mesh mesh = GetComponent<Mesh>();
            //可以开关单个组件,关乎组件是否处于活跃状态
            mesh.Enable = false;

            //目前摄像机只有一个,可以通过这个获取引用
            Camera camera = Camera.Main;
            //改变摄像机位置
            camera.Position += new Vector2Int(0, 1);
        }

        //每帧调用一次
        public override void Update()
        {
            //获取距离上一帧的时间间隔
            float time = Time.DeltaTime;
            //获取按键信息
            Input.GetKey(System.ConsoleKey.A);
            //输出信息
            Debug.Log("Help!!!");
        }
    }
    #endregion
}
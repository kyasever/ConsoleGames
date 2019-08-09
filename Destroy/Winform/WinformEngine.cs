namespace Destroy
{
    using Destroy.Winform;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Application = System.Windows.Forms.Application;

    /// <summary>
    /// Winform程序入口
    /// </summary>
    public class WinformEngine
    {
        public static Thread FormThread;
        public static Thread EditorThread;

        /// <summary>
        /// 使用Winform渲染,但是不打开Editor组件.目前实现的有点偷懒
        /// </summary>
        [STAThread]
        public static void OpenWithoutEditor(Action action)
        {
            FormEditor.EditorMode = true;
            OpenWithEditor(action);
        }

        /// <summary>
        /// Winform的初始化.
        /// </summary>
        [STAThread]
        public static void OpenWithEditor(Action action, string FormName = "Destroy")
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var mainForm = new FormEditor();

            mainForm.Text = FormName;

            #region 关于注册的说明
            //任何引擎入口要搞定的事情就是注册好这几个事件
            //理论上来讲Core现在基本没什么平台依赖的东西了

            //Windows的入口没写,反正估计一阵子之内也用不上了
            //举例,如果是Windows可以这样进行绑定
            //ConsoleIO.WindowsInit();
            //Native.WindowsMouseInit();
            //StandardIO.GetMousePositionInPixelEvent += Native.GetMousePositionPixelWindows;
            //StandardIO.RendererEvent += Native.Print;

            //如果是Winform则可以这么初始化
            //ConsoleIO.Init();
            //StandardIO.RendererEvent += ConsoleIO.Renderer;
            #endregion

            //注册输入事件 输入事件来自于WindowsAPI
            StandardIO.GetKeyEvent += Windows.Windows.GetInput;
            StandardIO.GetMouseButtonEvent += Windows.Windows.GetInputMouse;
            //将窗体的对应方法绑到核心事件
            StandardIO.DebugLogEvent += EditorSystem.DebugLog;
            StandardIO.GetMousePositionInPixelEvent += EditorSystem.GetMousePositionPixel;
            StandardIO.RendererEvent += EditorSystem.Renderer;



            RuntimeEngine.Init();

            //将窗体的更新作为系统绑定到核心里
            RuntimeEngine.AddSystem<EditorSystem>();

            RuntimeEngine.SetSystemUpdate();
            //留给开发者的初始化接口,由中间层封装控制
            action.Invoke();


            //开启Editor线程
            FormThread = new Thread(() => { Application.Run(mainForm); });
            FormThread.Start();
            //这个解决方法很不优雅,但是必须等这个窗口创建完成了才能开始编辑器生命周期
            Thread.Sleep(10);
            //这个线程负责进行无缝渲染,不和引擎同步
            EditorThread = new Thread(EditorRuntime.Run);
            EditorThread.Start();

            RuntimeEngine.Run();
        }
    }
}
namespace Destroy
{
    using Destroy.Winform;
    using System;
    using System.Threading;
    using Application = System.Windows.Forms.Application;

    /// <summary>
    /// Winform程序入口
    /// </summary>
    public class WinformEngine
    {
        private static Thread EditorThread;


        /// <summary>
        /// Winform的初始化.
        /// </summary>
        [STAThread]
        public static void OpenWithEditor(Action action)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var mainForm = new FormEditor();

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

            //开启Editor线程
            EditorThread = new Thread(() => { Application.Run(mainForm); });
            EditorThread.Start();


            RuntimeEngine.Init();

            //将窗体的更新作为系统绑定到核心里
            RuntimeEngine.AddSystem<EditorSystem>();

            RuntimeEngine.SetSystemUpdate();
            //留给开发者的初始化接口,由中间层封装控制
            action.Invoke();
            RuntimeEngine.Run();
        }
    }
}
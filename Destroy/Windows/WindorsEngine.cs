using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destroy.Windows
{
    /// <summary>
    /// 留出接口,请帮忙实现一下
    /// </summary>
    public class WindowsAPI
    {
        /// <summary>
        /// 获取键盘按键
        /// </summary>
        public static bool GetInput(ConsoleKey consoleKey)
        {
            return (Windows.GetAsyncKeyState((int)consoleKey) & 0x8000) != 0;
        }

        /// <summary>
        /// 获取鼠标按键
        /// </summary>
        public static bool GetInputMouse(MouseButton mouseButton)
        {
            return GetInput((ConsoleKey)mouseButton);
        }

        /// <summary>
        /// 输出调试信息
        /// </summary>
        public static void DebugLog(string s)
        {
            System.Diagnostics.Debug.WriteLine(s);
        }

        /// <summary>
        /// 获得鼠标所在像素点的位置
        /// </summary>
        public static Vector2 GetMousePositionPixel()
        {
            return Vector2.Zero;
        }

        /// <summary>
        /// 渲染输出
        /// </summary>
        public static void Renderer(List<RenderPoint> list)
        {

        }
    }

    /// <summary>
    /// windows初始化
    /// </summary>
    public class WindowsEngine
    {
        /// <summary>
        /// WindorsAPI的初始化.
        /// </summary>
        public static void OpenWithWindows(Action action)
        {
            //注册输入事件 输入事件来自于WindowsAPI
            StandardIO.GetKeyEvent += WindowsAPI.GetInput;
            StandardIO.GetMouseButtonEvent += WindowsAPI.GetInputMouse;
            //将窗体的对应方法绑到核心事件
            StandardIO.DebugLogEvent += WindowsAPI.DebugLog;
            StandardIO.GetMousePositionInPixelEvent += WindowsAPI.GetMousePositionPixel;
            StandardIO.RendererEvent += WindowsAPI.Renderer;


            RuntimeEngine.Init();

            //在这里添加新的系统

            RuntimeEngine.SetSystemUpdate();

            //执行引擎的初始化代码
            action.Invoke();

            RuntimeEngine.Run();
        }


    }
}

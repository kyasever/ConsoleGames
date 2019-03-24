using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Destroy.Windows
{
    /// <summary>
    /// 暂时的API
    /// </summary>
    public static class Windows
    {
        /// <summary>
        /// 获取当前鼠标键盘的状态
        /// </summary>
        [DllImport("User32.dll", EntryPoint = "GetAsyncKeyState")]
        public static extern short GetAsyncKeyState(int key);

        //时钟计时方法:保存Start-End / frequency 就是经过的时间(单位:秒)

        /// <summary>
        /// 获取当前系统运行的时钟周期 
        /// </summary>
        [DllImport("kernel32.dll", EntryPoint = "QueryPerformanceCounter")]
        public static extern short QueryPerformanceCounter(ref long x);

        /// <summary>
        /// 获取当前CPU的主频
        /// </summary>
        [DllImport("kernel32.dll", EntryPoint = "QueryPerformanceFrequency")]
        public static extern short QueryPerformanceFrequency(ref long x);

        /// <summary>
        /// 获取输入按键
        /// </summary>
        public static bool GetInput(ConsoleKey consoleKey)
        {
            return (GetAsyncKeyState((int)consoleKey) & 0x8000) != 0;
        }

        /// <summary>
        /// 获取鼠标按键
        /// </summary>
        public static bool GetInputMouse(MouseButton mouseButton)
        {
            return GetInput((ConsoleKey)mouseButton);
        }
    }
}
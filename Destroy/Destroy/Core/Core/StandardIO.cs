namespace Destroy
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 平台相关的系统只向这里注册,其他方法均为引擎内部实现
    /// 引擎入口务必要保证处理好里面的事件
    /// </summary>
    public static class StandardIO
    {
        /*
         * ConsoleKey 和 Keys 有一定的差距但是不大,姑且认为可以强转
         * https://docs.microsoft.com/zh-cn/dotnet/api/system.windows.forms.keys?view=netframework-4.7.2
         */

        /// <summary>
        /// 标准调试信息输出
        /// </summary>
        public static Action<string> DebugLogEvent;

        /// <summary>
        /// 渲染出口
        /// </summary>
        public static Action<List<RenderPoint>> RendererEvent;

        /// <summary>
        /// 输入入口,该按键是否被按下
        /// 目前通过WindowsAPI实现
        /// </summary>
        public static Func<MouseButton, bool> GetMouseButtonEvent;

        /// <summary>
        /// 输入入口,键盘按键是否被按下
        /// 通过WindowsAPI实现
        /// </summary>
        public static Func<ConsoleKey, bool> GetKeyEvent;

        /// <summary>
        /// 鼠标位置
        /// Winform实现,鼠标指针在游戏面板控件的位置
        /// Windows实现,不怎么好用
        /// </summary>
        public static Func<Vector2> GetMousePositionInPixelEvent;
    }
}
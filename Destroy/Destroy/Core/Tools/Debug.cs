using System;

namespace Destroy
{
    /// <summary>
    /// 调试类(用于调试程序) <see langword="static"/>
    /// </summary>
    public static class Debug
    {
        /// <summary>
        /// 向调试器输出窗口打印
        /// </summary>
        private static void Output(string msg)
        {
            System.Diagnostics.Debug.WriteLine(msg.ToString());
        }
        static Debug()
        {
#if DEBUG
            StandardIO.DebugLogEvent += Output;
#endif
        }

        /// <summary>
        /// 输出普通调试信息
        /// </summary>
        public static void Log(object msg)
        {
            StandardIO.DebugLogEvent?.Invoke(msg.ToString());
        }

        /// <summary>
        /// 输出警告调试信息
        /// </summary>
        public static void Warning(object msg)
        {
            StandardIO.DebugLogEvent?.Invoke(msg.ToString());
        }

        /// <summary>
        /// 输出错误调试信息
        /// </summary>
        public static void Error(object msg)
        {
            StandardIO.DebugLogEvent?.Invoke(msg.ToString());
        }
    }
}
namespace Destroy
{
    using System.Collections.Generic;

    /// <summary>
    /// 时间类 <see langword="static"/>
    /// </summary>
    public static class Time
    {
        /// <summary>
        /// 游戏总运行时间(秒)
        /// </summary>
        public static float TotalTime { get; internal set; }

        /// <summary>
        /// 这一帧距离上一帧的时间(秒)
        /// </summary>
        public static float DeltaTime { get; internal set; }

        /// <summary>
        /// 当前帧率
        /// </summary>
        public static float CurFrameRate => 1 / DeltaTime;

        #region Windows Only

        /// <summary>
        /// 所有系统该帧运行的时间(秒)
        /// </summary>
        public static float AllSystemsSpendTime { get; internal set; }

        /// <summary>
        /// 单个系统该帧运行的时间(秒)
        /// </summary>
        public static Dictionary<string, float> SystemsSpendTimeDict { get; private set; } = new Dictionary<string, float>();

        /// <summary>
        /// 最大帧率
        /// </summary>
        public static float MaxFrameRate => 1 / AllSystemsSpendTime;

        #endregion
    }
}
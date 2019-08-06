using System;

namespace Destroy
{
    /// <summary>
    /// 鼠标按键
    /// </summary>
    public enum MouseButton
    {
        /// <summary>
        /// 左键
        /// </summary>
        Left = 1,
        /// <summary>
        /// 中键
        /// </summary>
        Right = 2,
        /// <summary>
        /// 右键
        /// </summary>
        Middle = 4,
    }


    /// <summary>
    /// 坐标系类型
    /// </summary>
    public enum CoordinateType
    {
        /// <summary>
        /// right x, down Y
        /// </summary>
        Window = 0,
        /// <summary>
        /// right X, up Y
        /// </summary>
        Normal = 1,
    }

    /// <summary>
    /// 旋转角度
    /// </summary>
    public enum RotationAngle
    {
        /// <summary>
        /// 顺时针旋转90度
        /// </summary>
        RotRight90,
        /// <summary>
        /// 旋转180度
        /// </summary>
        Rot180,
        /// <summary>
        /// 逆时针旋转90度
        /// </summary>
        RotLeft90,
    }

    /// <summary>
    /// 除0异常
    /// </summary>
    [Serializable]
    public class NaNException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public override string Message => "NaN Exception:Can not divide 0.";
    }
}
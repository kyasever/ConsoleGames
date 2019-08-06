namespace Destroy
{
    /// <summary>
    /// 相机组件
    /// 暂时还没有别的功能,相当于正交视角摄像机
    /// </summary>
    public class Camera : Component
    {
        /// <summary>
        /// 单例的,只有一个相机
        /// </summary>
        public static Camera Main { get; private set; }

        /// <summary>
        /// 暂时除了位置之外相机还没有别的选择
        /// </summary>
        public Camera() => Main = this;
    }
}
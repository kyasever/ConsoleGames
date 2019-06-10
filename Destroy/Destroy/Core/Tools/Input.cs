namespace Destroy
{
    using System;

    /// <summary>
    /// 获取标准输入 对外接口 <see langword="static"/>
    /// </summary>
    public static class Input
    {
        internal static event Func<MouseButton, bool> GetMouseButtonDownEvent;

        internal static event Func<MouseButton, bool> GetMouseButtonUpEvent;

        internal static event Func<ConsoleKey, bool> GetKeyDownEvent;

        internal static event Func<ConsoleKey, bool> GetKeyUpEvent;


        #region 直接的Get从外部获取值
        /// <summary>
        /// 获取鼠标在游戏中的像素坐标
        /// </summary>
        public static Vector2 MousePositionInPixel
        {
            get
            {
                if (StandardIO.GetMousePositionInPixelEvent != null)
                    return StandardIO.GetMousePositionInPixelEvent.Invoke();
                else
                    return Vector2.DefaultInput;
            }
        }

        /// <summary>
        /// 持续获取按键输入
        /// </summary>
        public static bool GetKey(ConsoleKey consoleKey)
        {
            if (StandardIO.GetKeyEvent != null)
                return StandardIO.GetKeyEvent(consoleKey);
            else
                return false;
        }

        /// <summary>
        /// 获取鼠标按键
        /// </summary>
        public static bool GetMouseButton(MouseButton mouseButton)
        {
            if (StandardIO.GetMouseButtonEvent != null)
                return StandardIO.GetMouseButtonEvent.Invoke(mouseButton);
            else
                return false;
        }

        #endregion

        #region 间接的Get从InputSystem中获取值,InputSystem与平台无关

        /// <summary>
        /// 获取按下鼠标按键
        /// </summary>
        public static bool GetMouseButtonDown(MouseButton mouseButton)
        {
            if (GetMouseButtonDownEvent != null)
                return GetMouseButtonDownEvent(mouseButton);
            else
                return false;
        }

        /// <summary>
        /// 获取抬起鼠标按键
        /// </summary>
        public static bool GetMouseButtonUp(MouseButton mouseButton)
        {
            if (GetMouseButtonUpEvent != null)
                return GetMouseButtonUpEvent(mouseButton);
            else
                return false;
        }



        /// <summary>
        /// 获取按下的按键
        /// </summary>
        public static bool GetKeyDown(ConsoleKey consoleKey)
        {
            if (GetKeyDownEvent != null)
                return GetKeyDownEvent(consoleKey);
            else
                return false;
        }

        /// <summary>
        /// 获取弹起的按键
        /// </summary>
        public static bool GetKeyUp(ConsoleKey consoleKey)
        {
            if (GetKeyUpEvent != null)
                return GetKeyUpEvent(consoleKey);
            else
                return false;
        }
        #endregion

        /// <summary>
        /// 获取鼠标当前所处的位置(场景中世界坐标的位置)
        /// </summary>
        public static Vector2 MousePosition
        {
            get
            {
                Vector2 posPixel = MousePositionInPixel;
                Vector2 pos = new Vector2(posPixel.X / Config.RendererSize.X,
                    posPixel.Y / Config.RendererSize.Y);
                //改变坐标系
                pos.Y = Config.ScreenHeight - pos.Y - 1;
                //加上摄像机的坐标
                pos += Camera.Main.Position;
                return pos;
            }
        }

        /// <summary>
        /// 获取指定方向上的按键输入
        /// </summary>
        public static int GetDirectInput(ConsoleKey negative, ConsoleKey positive)
        {
            int result = 0;

            if (GetKey(negative))
                result -= 1;
            if (GetKey(positive))
                result += 1;

            return result;
        }
    }
}
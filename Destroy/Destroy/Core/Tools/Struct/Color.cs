namespace Destroy
{
    using System;

    /// <summary>
    /// 颜色类,可以实现对ConsoleColor和Drawing.Color的兼容支持
    /// </summary>
    public struct Color
    {
        /// <summary>
        /// alpha通道,支持颜色混合和颜色相加操作,同等depth会进行颜色混合
        /// alpha = 255,不透明 alpha = 0,透明
        /// </summary>
        private uint alpha;
        private uint r;
        private uint g;
        private uint b;

        /// <summary>
        /// alpha
        /// </summary>
        public uint Alpha { get => alpha; set => alpha = value; }

        /// <summary>
        /// red
        /// </summary>
        public uint R { get => r; set => r = value; }
        /// <summary>
        /// green
        /// </summary>
        public uint G { get => g; set => g = value; }
        /// <summary>
        /// blue
        /// </summary>
        public uint B { get => b; set => b = value; }

        internal ConsoleColor? consoleColor;

        /// <summary>
        /// 使用RGB构造
        /// </summary>
        public Color(uint r, uint g, uint b,uint alpha)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.alpha = alpha;
            consoleColor = null;
        }

        /// <summary>
        /// 使用ConsoleColor构造
        /// </summary>
        public Color(ConsoleColor consoleColor)
        {
            this.consoleColor = consoleColor;
            ParseConsoleColor(consoleColor, out r, out g, out b);
            alpha = 255;
        }
        #region 静态颜色
        /// <summary>
        /// 默认透明色
        /// </summary>
        public static Color TransParent = new Color(ConsoleColor.White).SetTransParent();
        /// <summary>
        /// 
        /// </summary>
        public static Color Black = new Color(ConsoleColor.Black);
        /// <summary>
        /// 
        /// </summary>
        public static Color Blue = new Color(ConsoleColor.Blue);
        /// <summary>
        /// 
        /// </summary>
        public static Color Green = new Color(ConsoleColor.Green);
        /// <summary>
        /// 
        /// </summary>
        public static Color Cyan = new Color(ConsoleColor.Cyan);
        /// <summary>
        /// 
        /// </summary>
        public static Color Red = new Color(ConsoleColor.Red);
        /// <summary>
        /// 
        /// </summary>
        public static Color Gray = new Color(ConsoleColor.Gray);
        /// <summary>
        /// 
        /// </summary>
        public static Color Magenta = new Color(ConsoleColor.Magenta);
        /// <summary>
        /// 
        /// </summary>
        public static Color Yellow = new Color(ConsoleColor.Yellow);
        /// <summary>
        /// 
        /// </summary>
        public static Color White = new Color(ConsoleColor.White);
        /// <summary>
        /// 
        /// </summary>
        public static Color DarkBlue = new Color(ConsoleColor.DarkBlue);
        /// <summary>
        /// 
        /// </summary>
        public static Color DarkGreen = new Color(ConsoleColor.DarkGreen);
        /// <summary>
        /// 
        /// </summary>
        public static Color DarkCyan = new Color(ConsoleColor.DarkCyan);
        /// <summary>
        /// 
        /// </summary>
        public static Color DarkRed = new Color(ConsoleColor.DarkRed);
        /// <summary>
        /// 
        /// </summary>
        public static Color DarkMagenta = new Color(ConsoleColor.DarkMagenta);
        /// <summary>
        /// 
        /// </summary>
        public static Color DarkYellow = new Color(ConsoleColor.DarkYellow);
        /// <summary>
        /// 
        /// </summary>
        public static Color DarkGray = new Color(ConsoleColor.DarkGray);
        #endregion

        /// <summary>
        /// 比较
        /// </summary>
        public static bool operator ==(Color left, Color right)
        {
            return left.R == right.R && left.G == right.G && left.B == right.B;
        }

        /// <summary>
        /// !=
        /// </summary>
        public static bool operator !=(Color left, Color right)
        {
            return !(left == right);
        }

        /// <summary>
        /// ==
        /// </summary>
        public override bool Equals(object obj) => this == (Color)obj;

        /// <summary>
        /// Hash
        /// </summary>
        public override int GetHashCode() => base.GetHashCode();


        /// <summary>
        /// System.ConsoleColor转换为System.Drawing.Color
        /// </summary>
        public static void ParseConsoleColor(ConsoleColor consoleColor, out uint r, out uint g, out uint b)
        {
            switch (consoleColor)
            {
                case ConsoleColor.Black:
                    {
                        r = 0;
                        g = 0;
                        b = 0;
                    }
                    break;
                case ConsoleColor.DarkBlue:
                    {
                        r = 0;
                        g = 0;
                        b = 128;
                    }
                    break;
                case ConsoleColor.DarkGreen:
                    {
                        r = 0;
                        g = 128;
                        b = 0;
                    }
                    break;
                case ConsoleColor.DarkCyan:
                    {
                        r = 0;
                        g = 128;
                        b = 128;
                    }
                    break;
                case ConsoleColor.DarkRed:
                    {
                        r = 128;
                        g = 0;
                        b = 0;
                    }
                    break;
                case ConsoleColor.DarkMagenta:
                    {
                        r = 128;
                        g = 0;
                        b = 128;
                    }
                    break;
                case ConsoleColor.DarkYellow:
                    {
                        r = 128;
                        g = 128;
                        b = 0;
                    }
                    break;
                case ConsoleColor.Gray:
                    {
                        r = 192;
                        g = 192;
                        b = 192;
                    }
                    break;
                case ConsoleColor.DarkGray:
                    {
                        r = 128;
                        g = 128;
                        b = 128;
                    }
                    break;
                case ConsoleColor.Blue:
                    {
                        r = 0;
                        g = 0;
                        b = 255;
                    }
                    break;
                case ConsoleColor.Green:
                    {
                        r = 0;
                        g = 255;
                        b = 0;
                    }
                    break;
                case ConsoleColor.Cyan:
                    {
                        r = 0;
                        g = 255;
                        b = 255;
                    }
                    break;
                case ConsoleColor.Red:
                    {
                        r = 255;
                        g = 0;
                        b = 0;
                    }
                    break;
                case ConsoleColor.Magenta:
                    {
                        r = 255;
                        g = 0;
                        b = 255;
                    }
                    break;
                case ConsoleColor.Yellow:
                    {
                        r = 255;
                        g = 255;
                        b = 0;
                    }
                    break;
                case ConsoleColor.White:
                    {
                        r = 255;
                        g = 255;
                        b = 255;
                    }
                    break;
                default:
                    throw new Exception("Error");
            }
        }

        /// <summary>
        /// 将colour解析为ConsoleColor
        /// </summary>
        /// <return>返回值不会为空</return>
        public ConsoleColor ToConsoleColor()
        {
            if (consoleColor == null)
                return ColorToConsoleColor((byte)R, (byte)G, (byte)B);
            return (ConsoleColor)consoleColor;
        }

        /// <summary>
        /// 根据System.Drawing.Color的RGB值返回最接近的System.ConsoleColor
        /// </summary>
        public static ConsoleColor ColorToConsoleColor(byte r, byte g, byte b)
        {
            ConsoleColor result = 0;
            double delta = double.MaxValue;

            foreach (ConsoleColor each in Enum.GetValues(typeof(ConsoleColor)))
            {
                string name = Enum.GetName(typeof(ConsoleColor), each);

                uint R, G, B;
                ParseConsoleColor(each, out R, out G, out B);
                double t = Math.Pow(R - r, 2.0) + Math.Pow(G - g, 2.0) + Math.Pow(B - b, 2.0);
                if (t == 0.0)
                    return each;
                if (t < delta)
                {
                    delta = t;
                    result = each;
                }
            }
            return result;
        }
    }
}
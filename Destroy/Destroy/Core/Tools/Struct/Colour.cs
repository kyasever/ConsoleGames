namespace Destroy
{
    using System;

    /// <summary>
    /// 颜色类,可以实现对ConsoleColor和Drawing.Color的兼容支持
    /// </summary>
    public struct Colour
    {
        private uint r;
        private uint g;
        private uint b;
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
        public Colour(uint r, uint g, uint b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            consoleColor = null;
        }

        /// <summary>
        /// 使用ConsoleColor构造
        /// </summary>
        public Colour(ConsoleColor consoleColor)
        {
            this.consoleColor = consoleColor;
            ParseConsoleColor(consoleColor,out r,out g,out b);
        }
        #region 静态颜色
        /// <summary>
        /// 
        /// </summary>
        public static Colour Black = new Colour(ConsoleColor.Black);
        /// <summary>
        /// 
        /// </summary>
        public static Colour Blue = new Colour(ConsoleColor.Blue);
        /// <summary>
        /// 
        /// </summary>
        public static Colour Green = new Colour(ConsoleColor.Green);
        /// <summary>
        /// 
        /// </summary>
        public static Colour Cyan = new Colour(ConsoleColor.Cyan);
        /// <summary>
        /// 
        /// </summary>
        public static Colour Red = new Colour(ConsoleColor.Red);
        /// <summary>
        /// 
        /// </summary>
        public static Colour Gray = new Colour(ConsoleColor.Gray);
        /// <summary>
        /// 
        /// </summary>
        public static Colour Magenta = new Colour(ConsoleColor.Magenta);
        /// <summary>
        /// 
        /// </summary>
        public static Colour Yellow = new Colour(ConsoleColor.Yellow);
        /// <summary>
        /// 
        /// </summary>
        public static Colour White = new Colour(ConsoleColor.White);
        /// <summary>
        /// 
        /// </summary>
        public static Colour DarkBlue = new Colour(ConsoleColor.DarkBlue);
        /// <summary>
        /// 
        /// </summary>
        public static Colour DarkGreen = new Colour(ConsoleColor.DarkGreen);
        /// <summary>
        /// 
        /// </summary>
        public static Colour DarkCyan = new Colour(ConsoleColor.DarkCyan);
        /// <summary>
        /// 
        /// </summary>
        public static Colour DarkRed = new Colour(ConsoleColor.DarkRed);
        /// <summary>
        /// 
        /// </summary>
        public static Colour DarkMagenta = new Colour(ConsoleColor.DarkMagenta);
        /// <summary>
        /// 
        /// </summary>
        public static Colour DarkYellow = new Colour(ConsoleColor.DarkYellow);
        /// <summary>
        /// 
        /// </summary>
        public static Colour DarkGray = new Colour(ConsoleColor.DarkGray);
        #endregion

        /// <summary>
        /// 比较
        /// </summary>
        public static bool operator ==(Colour left, Colour right)
        {
            return left.R == right.R && left.G == right.G && left.B == right.B;
        }

        /// <summary>
        /// !=
        /// </summary>
        public static bool operator !=(Colour left, Colour right)
        {
            return !(left == right);
        }

        /// <summary>
        /// ==
        /// </summary>
        public override bool Equals(object obj) => this == (Colour)obj;

        /// <summary>
        /// Hash
        /// </summary>
        public override int GetHashCode() => base.GetHashCode();


        /// <summary>
        /// System.ConsoleColor转换为System.Drawing.Color
        /// </summary>
        public static void ParseConsoleColor(ConsoleColor consoleColor,out uint r,out uint g,out uint b)
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
                ParseConsoleColor(each,out R,out G,out B);
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
namespace Destroy
{
    using System;
    using System.Drawing;

    /// <summary>
    /// 颜色类,可以实现对ConsoleColor和Drawing.Color的兼容支持
    /// </summary>
    public struct Colour
    {
        /// <summary>
        /// red
        /// </summary>
        public uint R { get; set; }
        /// <summary>
        /// green
        /// </summary>
        public uint G { get; set; }
        /// <summary>
        /// blue
        /// </summary>
        public uint B { get; set; }

        private ConsoleColor? consoleColor;

        /// <summary>
        /// 使用RGB构造
        /// </summary>
        public Colour(uint r, uint g, uint b)
        {
            R = r;
            G = g;
            B = b;
            consoleColor = null;
        }

        /// <summary>
        /// 使用Drawing.Color构造
        /// </summary>
        public Colour(Color color)
        {
            R = color.R;
            G = color.G;
            B = color.B;
            consoleColor = null;
        }

        /// <summary>
        /// 使用ConsoleColor构造
        /// </summary>
        public Colour(ConsoleColor consoleColor)
        {
            this.consoleColor = consoleColor;

            Color color = ConsoleColorToColor(consoleColor);

            R = color.R;
            G = color.G;
            B = color.B;
        }

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
        /// 转换为ConsoleColor供C#原生渲染器调用
        /// </summary>
        /// <return>返回值不会为空</return>
        public ConsoleColor ToConsoleColor()
        {
            if (consoleColor == null)
                return ColorToConsoleColor(Color.FromArgb((int)R, (int)G, (int)B));
            return (ConsoleColor)consoleColor;
        }

        /// <summary>
        /// 转化为System.Drawing.Color供其他渲染引擎使用
        /// </summary>
        public Color ToColor() => Color.FromArgb((int)R, (int)G, (int)B);

        /// <summary>
        /// 根据System.Drawing.Color的RGB值返回最接近的System.ConsoleColor
        /// </summary>
        public static ConsoleColor ColorToConsoleColor(Color color)
        {
            ConsoleColor closestConsoleColor = 0;
            double delta = double.MaxValue;

            foreach (ConsoleColor consoleColor in Enum.GetValues(typeof(ConsoleColor)))
            {
                string consoleColorName = Enum.GetName(typeof(ConsoleColor), consoleColor);
                consoleColorName = string.Equals(consoleColorName, nameof(ConsoleColor.DarkYellow), StringComparison.Ordinal) ? nameof(Color.Orange) : consoleColorName;
                Color rgbColor = Color.FromName(consoleColorName);
                double sum = Math.Pow(rgbColor.R - color.R, 2.0) + Math.Pow(rgbColor.G - color.G, 2.0) + Math.Pow(rgbColor.B - color.B, 2.0);

                double epsilon = 0.001;
                if (sum < epsilon)
                {
                    return consoleColor;
                }

                if (sum < delta)
                {
                    delta = sum;
                    closestConsoleColor = consoleColor;
                }
            }
            return closestConsoleColor;
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

                Color color = Color.FromName(name == "DarkYellow" ? "Orange" : name);
                double t = Math.Pow(color.R - r, 2.0) + Math.Pow(color.G - g, 2.0) + Math.Pow(color.B - b, 2.0);
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

        /// <summary>
        /// System.ConsoleColor转换为System.Drawing.Color
        /// </summary>
        public static Color ConsoleColorToColor(ConsoleColor consoleColor)
        {
            int r, g, b;

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
            return Color.FromArgb(r, g, b);
        }
    }
}
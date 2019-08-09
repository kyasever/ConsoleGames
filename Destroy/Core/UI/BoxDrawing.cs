namespace Destroy
{
    using System.Text;

    /// <summary>
    /// 保存一些特殊Unicode字符
    /// </summary>
    public class UnicodeDrawing
    {
        /// <summary>
        /// '█'
        /// </summary>
        public static char Block8_8 = '█';
        /// <summary>
        /// '▉'
        /// </summary>
        public static char Block7_8 = '▉';
        /// <summary>
        /// '▊'
        /// </summary>
        public static char Block6_8 = '▊';
        /// <summary>
        /// '▋'
        /// </summary>
        public static char Block5_8 = '▋';
        public static char Block4_8 = '▌';
        public static char Block3_8 = '▍';
        public static char Block2_8 = '▎';
        public static char Block1_8 = '▏';
        public static char Block0_8 = ' ';
    }


    /// <summary>
    /// 用于制表符加法运算的一个辅助类
    /// </summary>
    public class BoxDrawingCharacter
    {
        private bool down, up, left, right;

        internal BoxDrawingCharacter(bool up, bool down, bool left, bool right)
        {
            this.down = down;
            this.up = up;
            this.left = left;
            this.right = right;
        }
        /// <summary>
        /// 从一个字符c解析一个对象,从而可以进行运算
        /// </summary>
        public static BoxDrawingCharacter Prase(char c)
        {
            switch (c)
            {
                case '┌':
                    return BoxDownRight;
                case '┐':
                    return BoxDownLeft;
                case '└':
                    return BoxUpRight;
                case '┘':
                    return BoxUpLeft;
                case '─':
                    return BoxHorizontal;
                case '│':
                    return BoxVertical;
                case '├':
                    return BoxVerticalRight;
                case '┤':
                    return BoxVerticalLeft;
                case '┬':
                    return BoxHorizontalDown;
                case '┴':
                    return BoxHorizontalUp;
                case '┼':
                    return BoxVerticalHorizontal;
                default:
                    return new BoxDrawingCharacter(false, false, false, false);
            }

        }

        /// <summary>
        /// 从一个对象转化为一个字符,进行输出
        /// </summary>
        public char ToChar()
        {
            if (!up && down && !left && right)
            {
                return '┌';
            }
            else if (!up && down && left && !right)
            {
                return '┐';
            }
            else if (up && !down && !left && right)
            {
                return '└';
            }
            else if (up && !down && left && !right)
            {
                return '┘';
            }
            else if (!up && !down && left && right)
            {
                return '─';
            }
            else if (up && down && !left && !right)
            {
                return '│';
            }

            else if (up && down && !left && right)
            {
                return '├';
            }
            else if (up && down && left && !right)
            {
                return '┤';
            }
            else if (!up && down && left && right)
            {
                return '┬';
            }
            else if (up && !down && left && right)
            {
                return '┴';
            }
            else if (up && down && left && right)
            {
                return '┼';
            }

            else
            {
                return ' ';
            }
        }

        /// <summary>
        /// 说到底都是为了进行加法叠加运算...
        /// </summary>
        public static BoxDrawingCharacter operator +(BoxDrawingCharacter a, BoxDrawingCharacter b)
        {
            return new BoxDrawingCharacter(a.up || b.up, a.down || b.down, a.left || b.left, a.right || b.right);
        }
        #region 创建制表符对象
        /// <summary>
        /// 
        /// </summary>
        public static BoxDrawingCharacter BoxDownRight = new BoxDrawingCharacter(false, true, false, true);
        /// <summary>
        /// 
        /// </summary>
        public static BoxDrawingCharacter BoxDownLeft = new BoxDrawingCharacter(false, true, true, false);
        /// <summary>
        /// 
        /// </summary>
        public static BoxDrawingCharacter BoxUpRight = new BoxDrawingCharacter(true, false, false, true);
        /// <summary>
        /// 
        /// </summary>
        public static BoxDrawingCharacter BoxUpLeft = new BoxDrawingCharacter(true, false, true, false);
        /// <summary>
        /// 
        /// </summary>
        public static BoxDrawingCharacter BoxHorizontal = new BoxDrawingCharacter(false, false, true, true);
        /// <summary>
        /// 
        /// </summary>
        public static BoxDrawingCharacter BoxVertical = new BoxDrawingCharacter(true, true, false, false);
        /// <summary>
        /// 
        /// </summary>
        public static BoxDrawingCharacter BoxVerticalRight = new BoxDrawingCharacter(true, true, false, true);
        /// <summary>
        /// 
        /// </summary>
        public static BoxDrawingCharacter BoxVerticalLeft = new BoxDrawingCharacter(true, true, true, false);
        /// <summary>
        /// 
        /// </summary>
        public static BoxDrawingCharacter BoxHorizontalDown = new BoxDrawingCharacter(false, true, true, true);
        /// <summary>
        /// 
        /// </summary>
        public static BoxDrawingCharacter BoxHorizontalUp = new BoxDrawingCharacter(true, false, true, true);
        /// <summary>
        /// 
        /// </summary>
        public static BoxDrawingCharacter BoxVerticalHorizontal = new BoxDrawingCharacter(true, true, true, true);
        #endregion
    }

    /// <summary>
    /// 制表符绘制的辅助类
    /// </summary>
    public static class BoxDrawingSupply
    {
        //┘└──┘└
        // https://www.oreilly.com/openbook/docbook/book/iso-box.html

        /// <summary>
        /// 
        /// </summary>
        public static char boxDownRight = '┌';
        /// <summary>
        /// 
        /// </summary>
        public static char boxDownLeft = '┐';
        /// <summary>
        /// 
        /// </summary>
        public static char boxUpRight = '└';
        /// <summary>
        /// 
        /// </summary>
        public static char boxUpLeft = '┘';
        /// <summary>
        /// 
        /// </summary>
        public static char boxHorizontal = '─';
        /// <summary>
        /// 
        /// </summary>
        public static char boxVertical = '│';
        /// <summary>
        /// 
        /// </summary>
        public static char boxVerticalRight = '├';
        /// <summary>
        /// 
        /// </summary>
        public static char boxVerticalLeft = '┤';
        /// <summary>
        /// 
        /// </summary>
        public static char boxHorizontalDown = '┬';
        /// <summary>
        /// 
        /// </summary>
        public static char boxHorizontalUp = '┴';
        /// <summary>
        /// 
        /// </summary>
        public static char boxVerticalHorizontal = '┼';

        /// <summary>
        /// 获取一个方框第一行的字符串
        /// </summary>
        public static string GetFirstLine(int width)
        {
            if (Config.CharWidth == CharWidthEnum.Single)
            {
                StringBuilder sb = new StringBuilder();
                //左上角
                sb.Append(boxDownRight);
                //上部
                for (int i = 1; i < width; i++)
                {
                    sb.Append(boxHorizontal);
                }
                //右上角
                sb.Append(boxDownLeft);

                return sb.ToString();
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                //左上角
                sb.Append(' ');
                sb.Append(boxDownRight);
                //上部
                for (int i = 0; i < width - 2; i++)
                {
                    sb.Append(boxHorizontal);
                    sb.Append(boxHorizontal);
                }
                //右上角
                sb.Append(boxDownLeft);
                sb.Append(' ');

                return sb.ToString();
            }
        }

        /// <summary>
        /// 获取一个方框最后一行的字符串
        /// </summary>
        public static string GetLastLine(int width)
        {
            if (Config.CharWidth == CharWidthEnum.Single)
            {
                StringBuilder sb = new StringBuilder();
                //左上角
                sb.Append(boxUpRight);
                //上部
                for (int i = 1; i < width; i++)
                {
                    sb.Append(boxHorizontal);
                }
                //右上角
                sb.Append(boxUpLeft);

                return sb.ToString();
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                //左下角
                sb.Append(' ');
                sb.Append(boxUpRight);
                //上部
                for (int i = 0; i < width - 2; i++)
                {
                    sb.Append(boxHorizontal);
                    sb.Append(boxHorizontal);
                }
                //右下角
                sb.Append(boxUpLeft);
                sb.Append(' ');
                return sb.ToString();
            }
        }

        /// <summary>
        /// 获取一个方框中间行的字符串
        /// </summary>
        public static string GetMiddleLine(int width, string str)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(boxVertical);
            string newstr = CharUtils.SubStr(str, width);
            sb.Append(newstr);
            sb.Append(boxVertical);
            return sb.ToString();
        }

        /// <summary>
        /// 获取一个方框中间行的字符串
        /// </summary>
        public static string GetMiddleLine(int width)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(boxVertical);
            for (int i = 1; i < width; i++)
            {
                sb.Append(' ');
            }
            sb.Append(boxVertical);
            //Debug.Warning(sb.ToString());
            return sb.ToString();

        }
    }
}

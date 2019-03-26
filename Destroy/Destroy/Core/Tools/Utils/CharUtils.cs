namespace Destroy
{
    using Destroy.Standard;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// 字符串工具类 <see langword="static"/>
    /// </summary>
    public static class CharUtils
    {
        /// <summary>
        /// 将两个字符作为制表符混合相加,输出结果
        /// </summary>
        public static char BoxDrawingAdd(char c1, char c2)
        {
            bool b1 = IsTabChar(c1);
            bool b2 = IsTabChar(c2);
            if (b1)
            {
                if (b2)
                {
                    BoxDrawingCharacter bdc1 = BoxDrawingCharacter.Prase(c1);
                    BoxDrawingCharacter bdc2 = BoxDrawingCharacter.Prase(c2);
                    return (bdc1 + bdc2).ToChar();
                    //制表符运算
                }
                else
                {
                    return c1;
                }
            }
            else if (b2)
            {
                return c2;
            }
            else
            {
                //默认输出右侧
                return c2;
            }
        }

        /// <summary>
        /// 判断一个char是不是制表符
        /// </summary>
        public static bool IsTabChar(char c)
        {
            if (c >= 0x2500 && c <= 0x257F)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 返回一个字符的宽度
        /// </summary>
        public static int GetCharWidth(char c)
        {
            if (IsTabChar(c))
                return 1;
            else if (c >= 0x4e00 && c <= 0x9fbb)   //只要不低于127都算Chinese
                return 2;
            else
                return 1;
        }

        /// <summary>
        /// 返回一个字符串的宽度
        /// </summary>
        public static int GetStringWidth(string str)
        {
            int sum = 0;
            foreach (char c in str)
            {
                sum += GetCharWidth(c);
            }
            return sum;
        }

        /// <summary>
        /// 将string分割成一个个格子, 单宽字符占据半个格子, 双宽字符会占据一个格子, 如果单宽后面直接跟双宽, 会在单宽字符后面先补上一个空格
        /// </summary>
        public static List<string> DivideString(string str)
        {
            List<string> girds = new List<string>();
            int tempLength = 0;
            string temp = "";
            foreach (char c in str)
            {
                if (GetCharWidth(c) == 1)
                {
                    if (tempLength == 0) //暂存
                    {
                        tempLength = 1;
                        temp = c.ToString();
                    }
                    else
                    {
                        tempLength = 0;
                        temp += c.ToString();
                        girds.Add(temp);
                        temp = "";
                    }
                }
                else if (GetCharWidth(c) == 2)
                {
                    if (tempLength == 1) //如果当前有缓存,那么先存储一个空格
                    {
                        tempLength = 0;
                        temp += " ";
                        girds.Add(temp);
                        temp = "";
                    }
                    //再存储
                    girds.Add(c.ToString());
                }
            }
            if (tempLength == 1)
            {
                tempLength = 0;
                temp += " ";
                girds.Add(temp);
                temp = "";
            }
            return girds;
        }

        /// <summary>
        /// 按照标准长度截断字符串,不足用空格补上,超出截断
        /// </summary>
        public static string SubStr(string str, int width)
        {
            StringBuilder builder = new StringBuilder();
            int sum = 0;
            foreach (char c in str)
            {
                int charWidth = GetCharWidth(c);
                if (sum + charWidth <= width)
                {
                    sum += charWidth;
                    builder.Append(c);
                }
                //当超出时跳出, 剩下的补空格
                else
                    break;
            }
            //长度不足用空格来补
            for (int i = 0; i < width - sum; i++)
            {
                builder.Append(' ');
            }
            return builder.ToString();
        }
    }
}
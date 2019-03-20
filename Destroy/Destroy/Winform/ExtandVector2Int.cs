using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destroy.Winform
{ 
    /// <summary>
    /// Vector2Int扩展,这个属于Destroy.Winform
    /// </summary>
    public static class ExtandVector2Int
    {
        //扩展方法必须为静态的
        //扩展方法的第一个参数必须由this来修饰（第一个参数是被扩展的对象）

        /// <summary>
        /// Point转换为Vector2Int
        /// </summary>
        public static Vector2 ToVector2Int(this Point _Point)
        {
            return new Vector2(_Point.X, _Point.Y);
        }

        /// <summary>
        /// Size 转换为Vector2Int
        /// </summary>
        public static Vector2 Size(this Size _Size)
        {
            return new Vector2(_Size.Width, _Size.Height);
        }

        /// <summary>
        /// Vector2Int 转换为Point
        /// </summary>
        public static Point ToPoint(this Vector2 _Vector2Int)
        {
            return new Point(_Vector2Int.X, _Vector2Int.Y);
        }

        /// <summary>
        /// 给Vector2扩展一个Distanse方法,求两个点的距离
        /// </summary>
        public static int Distanse(this Vector2 _Vector2Int, Vector2 otherV)
        {
            return Math.Abs(otherV.X - _Vector2Int.X) + Math.Abs(otherV.Y - _Vector2Int.Y);
        }
    }
}

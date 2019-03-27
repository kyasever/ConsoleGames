namespace Destroy
{
    using System;

    /// <summary>
    /// 整数型二维向量
    /// 原Vector2Int更改为Vector2为了简化
    /// </summary>
    public struct Vector2 : IComparable
    {
        /// <summary>
        /// 
        /// </summary>
        public int X;

        /// <summary>
        /// 
        /// </summary>
        public int Y;

        /// <summary>
        /// 
        /// </summary>
        public Vector2 Negative => this * -1;

        /// <summary>
        /// 
        /// </summary>
        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// 
        /// </summary>
        public override int GetHashCode() => base.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        public override bool Equals(object obj) => this == (Vector2)obj;

        /// <summary>
        /// 
        /// </summary>
        public override string ToString() => $"[X:{X},Y:{Y}]";

        /// <summary>
        /// 当检测到输入失灵的时候返回的默认值,不返回0,0为了加以区分
        /// </summary>
        public static Vector2 DefaultInput => new Vector2(-8, -8);

        /// <summary>
        /// 比较原则, 左上角的小于右下角的, 按照从上到下, 从左到右排序
        /// </summary>
        public int CompareTo(object obj)
        {
            Vector2 right = (Vector2)obj;
            if (Y < right.Y)
            {
                return 1;
            }
            else if (Y == right.Y)
            {
                if (X > right.X)
                {
                    return 1;
                }
                else if (X == right.X)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 向量之间的距离
        /// </summary>
        public static int Distance(Vector2 a, Vector2 b)
        {
            int x = Math.Abs(a.X - b.X);
            int y = Math.Abs(a.Y - b.Y);
            return x + y;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Vector2 Zero => new Vector2();

        /// <summary>
        /// 
        /// </summary>
        public static Vector2 Up => new Vector2(0, 1);

        /// <summary>
        /// 
        /// </summary>
        public static Vector2 Down => new Vector2(0, -1);

        /// <summary>
        /// 
        /// </summary>
        public static Vector2 Left => new Vector2(-1, 0);

        /// <summary>
        /// 
        /// </summary>
        public static Vector2 Right => new Vector2(1, 0);

        /// <summary>
        /// 
        /// </summary>
        public static bool operator ==(Vector2 left, Vector2 right) => left.X == right.X && left.Y == right.Y;

        /// <summary>
        /// 
        /// </summary>
        public static bool operator !=(Vector2 left, Vector2 right) => left.X != right.X || left.Y != right.Y;

        /// <summary>
        /// 
        /// </summary>
        public static Vector2 operator +(Vector2 left, Vector2 right)
        {
            left.X += right.X;
            left.Y += right.Y;
            return left;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Vector2 operator -(Vector2 left, Vector2 right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            return left;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Vector2 operator *(Vector2 left, int right)
        {
            left.X *= right;
            left.Y *= right;
            return left;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Vector2 operator /(Vector2 left, int right)
        {
            if (right == 0)
                throw new NaNException();
            left.X /= right;
            left.Y /= right;
            return left;
        }



        /// <summary>
        /// 
        /// </summary>
        public static explicit operator Vector2Float(Vector2 vector)
        {
            Vector2Float vector2 = new Vector2Float();
            vector2.X = vector.X;
            vector2.Y = vector.Y;
            return vector2;
        }

        /// <summary>
        /// 给Vector2扩展一个Distanse方法,求两个点的距离
        /// </summary>
        public int Distance(Vector2 otherV)
        {
            return Math.Abs(otherV.X - X) + Math.Abs(otherV.Y - Y);
        }
    }
}
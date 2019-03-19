namespace Destroy
{
    using System;

    /// <summary>
    /// 整数型二维向量
    /// </summary>
    public struct Vector2Int : IComparable
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
        public Vector2Int Negative => this * -1;

        /// <summary>
        /// 
        /// </summary>
        public Vector2Int(int x, int y)
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
        public override bool Equals(object obj) => this == (Vector2Int)obj;

        /// <summary>
        /// 
        /// </summary>
        public override string ToString() => $"[X:{X},Y:{Y}]";

        /// <summary>
        /// 当检测到输入失灵的时候返回的默认值,不返回0,0为了加以区分
        /// </summary>
        public static Vector2Int DefaultInput => new Vector2Int(-8, -8);

        /// <summary>
        /// 比较原则, 左上角的小于右下角的, 按照从上到下, 从左到右排序
        /// </summary>
        public int CompareTo(object obj)
        {
            Vector2Int right = (Vector2Int)obj;
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
        public static int Distance(Vector2Int a, Vector2Int b)
        {
            int x = Math.Abs(a.X - b.X);
            int y = Math.Abs(a.Y - b.Y);
            return x + y;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Vector2Int Zero => new Vector2Int();

        /// <summary>
        /// 
        /// </summary>
        public static Vector2Int Up => new Vector2Int(0, 1);

        /// <summary>
        /// 
        /// </summary>
        public static Vector2Int Down => new Vector2Int(0, -1);

        /// <summary>
        /// 
        /// </summary>
        public static Vector2Int Left => new Vector2Int(-1, 0);

        /// <summary>
        /// 
        /// </summary>
        public static Vector2Int Right => new Vector2Int(1, 0);

        /// <summary>
        /// 
        /// </summary>
        public static bool operator ==(Vector2Int left, Vector2Int right) => left.X == right.X && left.Y == right.Y;

        /// <summary>
        /// 
        /// </summary>
        public static bool operator !=(Vector2Int left, Vector2Int right) => left.X != right.X || left.Y != right.Y;

        /// <summary>
        /// 
        /// </summary>
        public static Vector2Int operator +(Vector2Int left, Vector2Int right)
        {
            left.X += right.X;
            left.Y += right.Y;
            return left;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Vector2Int operator -(Vector2Int left, Vector2Int right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            return left;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Vector2Int operator *(Vector2Int left, int right)
        {
            left.X *= right;
            left.Y *= right;
            return left;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Vector2Int operator /(Vector2Int left, int right)
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
        public static explicit operator Vector2Int(Vector2 vector)
        {
            Vector2Int vector2Int = new Vector2Int();
            vector2Int.X = (int)vector.X;
            vector2Int.Y = (int)vector.Y;
            return vector2Int;
        }
    }
}
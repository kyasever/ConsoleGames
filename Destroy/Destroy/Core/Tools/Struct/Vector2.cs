namespace Destroy
{
    using System;

    /// <summary>
    /// 二维向量
    /// </summary>
    public struct Vector2
    {
        /// <summary>
        /// X值
        /// </summary>
        public float X;

        /// <summary>
        /// Y值
        /// </summary>
        public float Y;

        /// <summary>
        /// 向量长度
        /// </summary>
        public float Magnitude
        {
            get
            {
                float magSquare = X * X + Y * Y;
                return (float)Math.Sqrt(magSquare);
            }
        }

        /// <summary>
        /// 单位向量
        /// </summary>
        public Vector2 Normalized => this / Magnitude;

        /// <summary>
        /// 反向
        /// </summary>
        public Vector2 Negative => this * -1f;

        /// <summary>
        /// 构造
        /// </summary>
        public Vector2(float x, float y)
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
        /// 向量之间的距离float
        /// </summary>
        public static float Distance(Vector2 a, Vector2 b)
        {
            Vector2 vector = a - b;
            return vector.Magnitude;
        }

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
        public static Vector2 operator *(Vector2 left, float right)
        {
            left.X *= right;
            left.Y *= right;
            return left;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Vector2 operator /(Vector2 left, float right)
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
        public static explicit operator Vector2(Vector2Int vector)
        {
            Vector2 vector2 = new Vector2();
            vector2.X = vector.X;
            vector2.Y = vector.Y;
            return vector2;
        }
    }
}
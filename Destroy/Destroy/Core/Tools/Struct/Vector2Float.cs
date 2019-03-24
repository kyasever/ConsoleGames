namespace Destroy
{
    using System;

    /// <summary>
    /// 二维向量
    /// 原Vector2->Vector2Float
    /// </summary>
    public struct Vector2Float
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
        public Vector2Float Normalized => this / Magnitude;

        /// <summary>
        /// 反向
        /// </summary>
        public Vector2Float Negative => this * -1f;

        /// <summary>
        /// 构造
        /// </summary>
        public Vector2Float(float x, float y)
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
        public override bool Equals(object obj) => this == (Vector2Float)obj;

        /// <summary>
        /// 
        /// </summary>
        public override string ToString() => $"[X:{X},Y:{Y}]";

        /// <summary>
        /// 
        /// </summary>
        public static Vector2Float Zero => new Vector2Float();

        /// <summary>
        /// 
        /// </summary>
        public static Vector2Float Up => new Vector2Float(0, 1);

        /// <summary>
        /// 
        /// </summary>
        public static Vector2Float Down => new Vector2Float(0, -1);

        /// <summary>
        /// 
        /// </summary>
        public static Vector2Float Left => new Vector2Float(-1, 0);

        /// <summary>
        /// 
        /// </summary>
        public static Vector2Float Right => new Vector2Float(1, 0);

        /// <summary>
        /// 向量之间的距离float
        /// </summary>
        public static float Distance(Vector2Float a, Vector2Float b)
        {
            Vector2Float vector = a - b;
            return vector.Magnitude;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool operator ==(Vector2Float left, Vector2Float right) => left.X == right.X && left.Y == right.Y;

        /// <summary>
        /// 
        /// </summary>
        public static bool operator !=(Vector2Float left, Vector2Float right) => left.X != right.X || left.Y != right.Y;

        /// <summary>
        /// 
        /// </summary>
        public static Vector2Float operator +(Vector2Float left, Vector2Float right)
        {
            left.X += right.X;
            left.Y += right.Y;
            return left;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Vector2Float operator -(Vector2Float left, Vector2Float right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            return left;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Vector2Float operator *(Vector2Float left, float right)
        {
            left.X *= right;
            left.Y *= right;
            return left;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Vector2Float operator /(Vector2Float left, float right)
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
    }
}
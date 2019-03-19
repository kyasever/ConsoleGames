namespace Destroy
{
    using System;

    /// <summary>
    /// 矩阵
    /// </summary>
    public class Matrix
    {
        private int[,] items;

        /// <summary>
        /// 
        /// </summary>
        public int Row => items.GetLength(0);

        /// <summary>
        /// 
        /// </summary>
        public int Column => items.GetLength(1);

        /// <summary>
        /// 
        /// </summary>
        public Matrix(int row, int column)
        {
            items = new int[row, column];
        }

        /// <summary>
        /// 
        /// </summary>
        public int this[int x, int y]
        {
            get
            {
                if (x < 0 || y < 0 || x > Row - 1 || y > Column - 1)
                    throw new ArgumentOutOfRangeException();
                return items[x, y];
            }
            set
            {
                if (x < 0 || y < 0 || x > Row - 1 || y > Column - 1)
                    throw new ArgumentOutOfRangeException();
                items[x, y] = value;
            }
        }

        /// <summary>
        /// 向量乘以该矩阵顺时针旋转90度
        /// </summary>
        public static Matrix RotateR90()
        {
            Matrix rotMatrix = new Matrix(2, 2);
            rotMatrix[0, 0] = 0;
            rotMatrix[0, 1] = 1;
            rotMatrix[1, 0] = -1;
            rotMatrix[1, 1] = 0;

            return rotMatrix;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Matrix operator +(Matrix left, Matrix right)
        {
            if (left.Row != right.Row || left.Column != right.Column)
                throw new Exception("无法相加!");

            int row = left.Row;
            int column = left.Column;
            Matrix matrix = new Matrix(row, column);

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                    matrix[i, j] = left[i, j] + right[i, j];
            }
            return matrix;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Matrix operator -(Matrix left, Matrix right)
        {
            if (left.Row != right.Row || left.Column != right.Column)
                throw new Exception("无法相减!");

            int row = left.Row;
            int column = left.Column;
            Matrix matrix = new Matrix(row, column);

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                    matrix[i, j] = left[i, j] - right[i, j];
            }
            return matrix;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Matrix operator *(Matrix left, int right)
        {
            int row = left.Row;
            int column = left.Column;
            Matrix matrix = new Matrix(row, column);

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                    matrix[i, j] = left[i, j] * right;
            }
            return matrix;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Matrix operator *(Matrix left, Matrix right)
        {
            if (left.Column != right.Row)
                throw new Exception("无法相乘!");

            int lr = left.Row;
            int lc = left.Column;
            int rc = right.Column;

            Matrix matrix = new Matrix(lr, rc);
            for (int i = 0; i < lr; i++)
            {
                for (int j = 0; j < rc; j++)
                {
                    for (int k = 0; k < lc; k++)
                        matrix[i, j] += left[i, k] * right[k, j];
                }
            }
            return matrix;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Vector2Int operator *(Vector2Int left, Matrix right)
        {
            if (right.Row != 2)
                throw new Exception("无法相乘");

            Matrix matrix = new Matrix(1, 2);
            matrix[0, 0] = left.X;
            matrix[0, 1] = left.Y;
            Matrix result = matrix * right;

            return new Vector2Int(result[0, 0], result[0, 1]);
        }
    }
}
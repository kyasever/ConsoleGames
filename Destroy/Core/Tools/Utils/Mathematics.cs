namespace Destroy
{
    /// <summary>
    /// 简易数学库 <see langword="static"/>
    /// </summary>
    public static class Mathematics
    {
        /// <summary>
        /// 算两个浮点数是否相近
        /// </summary>
        public static bool Approximately(float x, float y, float epsilon = 0.001f)
        {
            return x + epsilon >= y && x - epsilon <= y;
        }

        /// <summary>
        /// 限制Int取值范围
        /// </summary>
        public static int ClampInt(int i, int min, int max)
        {
            i = (i > max) ? max : i;
            i = (i < min) ? min : i;

            return i;
        }

        /// <summary>
        /// 在数组中取值
        /// </summary>
        public static T GetInArray<T>(T[,] array, int x, int y, CoordinateType coordinate)
        {
            if (coordinate == CoordinateType.Normal)
            {
                int _x = array.GetLength(0) - 1 - y;
                int _y = x;
                return array[_x, _y];
            }
            else if (coordinate == CoordinateType.Window)
            {
                return array[y, x];
            }
            return default(T);
        }

        /// <summary>
        /// 设置数组的值
        /// </summary>
        public static void SetInArray<T>(T[,] array, T item, int x, int y, CoordinateType coordinate)
        {
            if (coordinate == CoordinateType.Normal)
            {
                int _x = array.GetLength(0) - 1 - y;
                int _y = x;
                array[_x, _y] = item;
            }
            else if (coordinate == CoordinateType.Window)
            {
                array[y, x] = item;
            }
        }

        /// <summary>
        /// 旋转数组
        /// </summary>
        public static T[,] RotateArray<T>(T[,] array, RotationAngle angle)
        {
            T[,] rotArray = null;

            int width = array.GetLength(1);
            int height = array.GetLength(0);

            switch (angle)
            {
                case RotationAngle.RotRight90:
                    {
                        rotArray = new T[width, height];
                        for (int i = 0; i < width; i++)
                            for (int j = 0; j < height; j++)
                                rotArray[i, j] = array[height - 1 - j, i];
                    }
                    break;
                case RotationAngle.Rot180:
                    {
                        rotArray = new T[height, width];
                        for (int i = 0; i < height; i++)
                            for (int j = 0; j < width; j++)
                                rotArray[i, j] = array[height - 1 - i, width - 1 - j];
                    }
                    break;
                case RotationAngle.RotLeft90:
                    {
                        rotArray = new T[width, height];
                        for (int i = 0; i < width; i++)
                            for (int j = 0; j < height; j++)
                                rotArray[i, j] = array[j, width - 1 - i];
                    }
                    break;
            }

            return rotArray;
        }
    }
}
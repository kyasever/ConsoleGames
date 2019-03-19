namespace Destroy
{
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// 矩形类,用于返回标准矩形的点集.
    /// TODO:需要重写
    /// </summary>
    public class Rectangle
    {
        /// <summary>
        /// 宽度高度
        /// </summary>
        public int Width, Height;

        /// <summary>
        /// 点列表
        /// </summary>
        public List<Vector2Int> PosList;

        /// <summary>
        /// 字符
        /// </summary>
        public string Str;

        /// <summary>
        /// 初始化,给定起始坐标,长,宽. 进行点集的初始化操作
        /// </summary>
        public Rectangle(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            PosList = new List<Vector2Int>();
            AddMesh();
            AddTexture();
        }

        /// <summary>
        /// 添加Mesh组件
        /// </summary>
        private void AddMesh()
        {
            //添加上下边框的Mesh
            for (int i = 0; i < Width; i++)
            {
                PosList.Add(new Vector2Int(i, 0));
                PosList.Add(new Vector2Int(i, Height - 1));
            }
            //添加左右边框的Mesh
            for (int i = 0; i < Height; i++)
            {
                PosList.Add(new Vector2Int(0, i));
                PosList.Add(new Vector2Int(Width - 1, i));
            }
            Sort();
        }

        /// <summary>
        /// 添加贴图
        /// </summary>
        private void AddTexture()
        {
            //添加边框的贴图
            StringBuilder sb = new StringBuilder();
            sb.Append(BoxDrawingSupply.GetFirstLine(Width));
            for (int i = 0; i < Height - 2; i++)
            {
                sb.Append(' ');
                sb.Append(BoxDrawingSupply.boxVertical);
                sb.Append(BoxDrawingSupply.boxVertical);
                sb.Append(' ');
            }
            sb.Append(BoxDrawingSupply.GetLastLine(Width));
            Str = sb.ToString();
        }

        /// <summary>
        /// 去重复排序点集合
        /// </summary>
        public void Sort()
        {
            //使用HashSet去重复. 之后排序
            HashSet<Vector2Int> set = new HashSet<Vector2Int>();
            foreach (var v in PosList)
            {
                set.Add(v);
            }
            List<Vector2Int> newList = new List<Vector2Int>();
            foreach (var v in set)
            {
                newList.Add(v);
            }
            newList.Sort();
            PosList = newList;
        }
    }
}
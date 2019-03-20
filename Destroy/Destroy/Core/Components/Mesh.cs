namespace Destroy
{
    /*
     * TODO: 处理一下旋转之后的重绘问题
     */

    using System.Collections.Generic;

    /// <summary>
    /// Mesh组件 默认生成单点Mesh
    /// </summary>
    public class Mesh : Component
    {
        /// <summary>
        /// Mesh组件 默认生成单点Mesh
        /// </summary>
        public Mesh()
        {
            PosList = new List<Vector2>
            {
                new Vector2(0, 0)
            };
        }

        /// <summary>
        /// Mesh对象的列表
        /// </summary>
        public List<Vector2> PosList { get; private set; }

        /// <summary>
        /// TODO 这个方法需要重写或者删除
        /// 顺时针旋转90度,如果有碰撞的话会认为旋转失败
        /// </summary>
        public bool Rotate()
        {
            List<Vector2> newlist = new List<Vector2>();
            //旋转90度矩阵
            Matrix matrix = Matrix.RotateR90();

            for (int i = 0; i < PosList.Count; i++)
            {
                newlist.Add(PosList[i] * matrix);
            }
            //检测是否被碰撞卡住了
            Collider collider = GetComponent<Collider>();
            if (collider != null)
            {
                //如果旋转后会卡进墙里 那么返回旋转失败
                foreach (Vector2 v in newlist)
                {
                    //if (RuntimeEngine.GetSystem<PhysicsSystem>().HasCollider(Position + v, collider))
                    //{
                        return false;
                    //}
                }
                //改变碰撞体
                //collider.Change(newlist);
            }
            else
            {
                return false;
            }

            //旋转成功
            PosList = new List<Vector2>();
            foreach (Vector2 v in newlist)
            {
                PosList.Add(v);
            }
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                //renderer.RePaint();
            }
            else
            {
                return false;
            }
            return true;
        }


        private void Sort()
        {
            //使用HashSet去重复. 之后排序
            HashSet<Vector2> set = new HashSet<Vector2>();
            foreach (Vector2 v in PosList)
            {
                set.Add(v);
            }
            List<Vector2> newList = new List<Vector2>();
            foreach (Vector2 v in set)
            {
                newList.Add(v);
            }
            newList.Sort();
            PosList = newList;
        }

        /// <summary>
        /// 进行多点初始化
        /// </summary>
        public void Init(List<Vector2> list)
        {
            //删除原点检测.Mesh可以是任何形状的.
            //bool hasZeroFlag = false;
            ////检测是否包含原点,如果包含那么初始化成功
            //foreach (Vector2 v in list)
            //{
            //    if (v == Vector2.Zero)
            //    {
            //        hasZeroFlag = true;
            //        break;
            //    }
            //}
            //if(!hasZeroFlag)
            //{
            //    Debug.Warning(GameObject.Name + "没有原点,出于保险考虑加了个原点进去");
            //    list.Add(Vector2.Zero);
            //}
            PosList = list;
            //更改Mesh的时候进行重新排序
            Sort();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Destroy;

namespace HealerSimulator
{



    public static class Utils
    {
        /// <summary>
        /// 创建一个实心矩形的mesh区域,以左下角为原点
        /// </summary>
        public static List<Vector2> CreateRecMesh(int width, int height)
        {
            List<Vector2> result = new List<Vector2>();
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    result.Add(new Vector2(w, h));
                }
            }
            return result;
        }

        /// <summary>
        /// 创建一个实心矩形的mesh区域,以左下角为原点
        /// </summary>
        public static List<Vector2> CreateLineMesh(int width)
        {
            List<Vector2> result = new List<Vector2>();
            for (int w = 0; w < width; w++)
            {
                result.Add(new Vector2(w, 0));
            }
            return result;
        }

        /// <summary>
        /// 创建一个label,由于renderer组件的强大.所以直接返回renderer对象就行了
        /// </summary>
        public static Renderer CreateLabel(int width,string name = "StateLabel", string tag = "UI", GameObject parent = null, Vector2 localPosition = default)
        {
            //左上的的label 长度20 表示正在释放的技能的名字和施法时间等
            var obj = new GameObject(name,tag,parent,localPosition,CreateLineMesh(width));
            var labelCom = obj.AddComponent<Renderer>();
            labelCom.Init(RendererMode.UI, (int)Layer.Label);
            return labelCom;
        }
    }
}

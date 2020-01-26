using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destroy
{ //之后版本要整合进入引擎核心的内容
    public static class UIUtils
    { 
        /// <summary>
        /// 创建一个文本区域,可以设置是否包含文本框
        /// </summary>
        public static Renderer CreateTextAera(int width, int height, bool hasRect) {
            GameObject gameObject = new GameObject(posList: CreateRecMesh(width, height));
            var renderCom = gameObject.AddComponent<Renderer>();
            if (hasRect) {
                var box = UIFactroy.CreateBoxDrawingRect(new Vector2(-1, -1), height, width);
                box.Parent = gameObject;
            }
            return renderCom;
        }

        /// <summary>
        /// 创建一个实心矩形的mesh区域,以左下角为原点
        /// </summary>
        public static List<Vector2> CreateRecMesh(int width, int height) {
            List<Vector2> result = new List<Vector2>();
            for (int h = height - 1; h >= 0; h--) {
                for (int w = 0; w < width; w++) {
                    result.Add(new Vector2(w, h));
                }
            }
            return result;
        }

        /// <summary>
        /// 创建一个实心矩形的mesh区域,以左下角为原点
        /// </summary>
        public static List<Vector2> CreateLineMesh(int width) {
            List<Vector2> result = new List<Vector2>();
            for (int w = 0; w < width; w++) {
                result.Add(new Vector2(w, 0));
            }
            return result;
        }

        /// <summary>
        /// 创建一个label,由于renderer组件的强大.所以直接返回renderer对象就行了
        /// 返回一个空白的label
        /// </summary>
        public static Renderer CreateLabel(int width, string name = "StateLabel", string tag = "UI", GameObject parent = null, Vector2 localPosition = default(Vector2)) {
            //左上的的label 长度20 表示正在释放的技能的名字和施法时间等
            var obj = new GameObject(name, tag, parent, localPosition, CreateLineMesh(width));
            var labelCom = obj.AddComponent<Renderer>();
            labelCom.Init(RendererMode.UI, -1);
            return labelCom;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pic"></param>
        /// <returns></returns>
        public static Renderer CreatePointObj(string pic) {
            var renderer = CreateLabel(1);
            renderer.Rendering(pic);
            return renderer;
        }
    }
}

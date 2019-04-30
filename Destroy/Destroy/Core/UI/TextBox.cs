using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destroy
{
    /// <summary>
    /// 文本框组件
    /// </summary>
    public class TextBox : Component
    {
        /// <summary>
        /// 最后返回的应该是TextBox组件,可以通过label更改每一条的信息,
        /// </summary>
        public List<Renderer> Labels = new List<Renderer>();
        /// <summary>
        /// 边框的对象
        /// </summary>
        public GameObject boxDrawing;
    }
}

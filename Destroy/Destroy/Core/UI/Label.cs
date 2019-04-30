using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destroy
{
    /// <summary>
    /// 标签组件 基本处于没完成的状态
    /// </summary>
    public class Label : Component
    {
        public static Label Create()
        {
            UIObject obj = new UIObject();
            Label label = obj.AddComponent<Label>();
            return label;
        }

        private string text;
        public string Text
        {
            get => text; 
            set
            {
                text = value;
                DrawString(value);
            }
        }
    }
}

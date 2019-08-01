namespace Destroy
{
    /// <summary>
    /// 标签组件 基本处于没完成的状态
    /// </summary>
    public class Label : Component
    {
        public static Label Create()
        {
            UIObject obj = new UIObject("Label","UI");
            Label label = obj.AddComponent<Label>();
            return label;
        }

        public static Label Create(string text)
        {
            var label = Create();
            label.Text = text;
            return label;
        }

        private string text;
        public string Text
        {
            get => text;
            set
            {
                if(value != text)
                {
                    text = value;
                    DrawString(value);
                }
            }
        }
    }
}

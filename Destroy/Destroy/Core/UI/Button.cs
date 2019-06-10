using System;

namespace Destroy
{
    /// <summary>
    /// 按钮组件 有待各种边界检查和封装
    /// </summary>
    public class Button : Component
    {
        public static Button Create(Vector2 pos, string text, Action onClick = null)
        {
            UIObject obj = new UIObject();
            Button button = obj.AddComponent<Button>();
            button.Position = pos;
            button.Text = text;
            if (onClick != null)
                button.OnClickEvent += onClick;
            return button;
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

        public override void Initialize()
        {
            OnMoveInEvent += MoveIn;
            OnMoveOutEvent += MoveOut;
        }

        private void MoveIn()
        {
            Renderer.SetBackColor(Color.Yellow);
        }
        private void MoveOut()
        {
            Renderer.SetBackColor(Config.DefaultBackColor);
        }
    }
}

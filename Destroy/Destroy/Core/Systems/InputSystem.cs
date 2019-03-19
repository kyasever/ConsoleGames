namespace Destroy
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 处理来自底层的输入
    /// </summary>
    public class InputSystem : DestroySystem
    {
        private List<ConsoleKey> pressedKeys = new List<ConsoleKey>();
        private List<MouseButton> clickedButtons = new List<MouseButton>();

        /// <summary>
        /// 将getkey事件处理为getkeyDown事件并绑定到Input类
        /// </summary>
        public override void Start()
        {
            Input.GetMouseButtonDownEvent += GetMouseButtonDown;
            Input.GetMouseButtonUpEvent += GetMouseButtonUp;
            Input.GetKeyDownEvent += GetKeyDown;
            Input.GetKeyUpEvent += GetKeyUp;
        }

        /// <summary>
        /// Update
        /// </summary>
        public override void Update()
        {
            List<ConsoleKey> consoleKeys = new List<ConsoleKey>();
            foreach (ConsoleKey consoleKey in pressedKeys)
            {
                if (Input.GetKey(consoleKey))
                    consoleKeys.Add(consoleKey);
            }
            pressedKeys = consoleKeys;

            List<MouseButton> mouseButtons = new List<MouseButton>();
            foreach (MouseButton mouseButton in clickedButtons)
            {
                if (Input.GetMouseButton(mouseButton))
                    mouseButtons.Add(mouseButton);
            }
            clickedButtons = mouseButtons;
        }

        /// <summary>
        /// 
        /// </summary>
        protected  bool GetMouseButtonDown(MouseButton mouseButton)
        {
            if (Input.GetMouseButton(mouseButton))
            {
                if (!clickedButtons.Contains(mouseButton))
                {
                    clickedButtons.Add(mouseButton);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected  bool GetMouseButtonUp(MouseButton mouseButton)
        {
            if (Input.GetMouseButton(mouseButton))
            {
                if (!clickedButtons.Contains(mouseButton))
                {
                    clickedButtons.Add(mouseButton);
                }
                return false;
            }
            else
            {
                if (clickedButtons.Contains(mouseButton))
                {
                    clickedButtons.Remove(mouseButton);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected  bool GetKeyDown(ConsoleKey consoleKey)
        {
            if (Input.GetKey(consoleKey))
            {
                if (!pressedKeys.Contains(consoleKey))
                {
                    pressedKeys.Add(consoleKey);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected  bool GetKeyUp(ConsoleKey consoleKey)
        {
            if (Input.GetKey(consoleKey))
            {
                if (!pressedKeys.Contains(consoleKey))
                {
                    pressedKeys.Add(consoleKey);
                }
                return false;
            }
            else
            {
                if (pressedKeys.Contains(consoleKey))
                {
                    pressedKeys.Remove(consoleKey);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


    }
}
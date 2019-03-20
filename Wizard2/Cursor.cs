using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Destroy;

namespace Wizard2
{
    public class Cursor : Script
    {
        public float MoveSpeed = 10;
        private float MoveInterval;
        private float MoveTime = 0f;

        public static Cursor Instanse;
        public static Cursor CreateCursor()
        {
            GameObject gameObject = new GameObject("Cursor", "Cursor");
            var renderer =  gameObject.AddComponent<Renderer>();
            renderer.Depth = (int)Layer.Cursor;
            renderer.Rendering("  ", Config.DefaultForeColor, Colour.DarkRed);
            var cursorCom = gameObject.AddComponent<Cursor>();
            Instanse = cursorCom;
            return cursorCom;
        }


        public override void Update()
        {
            this.Position = Input.MousePosition;

            //当光标位于屏幕边缘时,移动摄像机
            Vector2 moveDir = Vector2.Zero;
            if(Position.X == Camera.Main.Position.X)
            {
                moveDir.X = -1;
            }
            else if(Position.X == Camera.Main.Position.X + Config.ScreenWidth -1)
            {
                moveDir.X = 1;
            }
            if (Position.Y == Camera.Main.Position.Y)
            {
                moveDir.Y = -1;
            }
            else if (Position.Y == Camera.Main.Position.Y + Config.ScreenHeight - 1)
            {
                moveDir.Y = 1;
            }
            if(moveDir != Vector2.Zero)
            {
                MoveInterval = 1 / MoveSpeed;
                MoveTime += Time.DeltaTime;
                if (MoveTime > MoveInterval)
                {
                    MoveTime = 0f;
                    Camera.Main.Position += moveDir;
                }
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destroy
{
    /// <summary>
    /// tools
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// 屏幕坐标转世界坐标
        /// </summary>
        public static Vector2 Screen2World(Vector2 posScreen) {
            Vector2 pos = new Vector2(posScreen.X / Config.RendererSize.X,
                posScreen.Y / Config.RendererSize.Y);
            //改变坐标系
            pos.Y = Config.ScreenHeight - pos.Y - 1;
            //加上摄像机的坐标
            pos += Camera.Main.Position;
            return pos;
        }

        /// <summary>
        /// 屏幕坐标转世界坐标
        /// </summary>
        public static Vector2 World2Screen(Vector2 posWorld) {
            Vector2 pos = posWorld - Camera.Main.Position;
            pos.Y = Config.ScreenHeight - pos.Y + 1;
            pos = new Vector2(pos.X * Config.RendererSize.X,pos.Y * Config.RendererSize.Y);
            return pos;
        }

    }
}

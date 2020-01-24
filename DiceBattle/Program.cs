using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Destroy;
using Destroy.Standard;
using Destroy.Winform;

namespace DiceBattle
{
    static class Program
    {
        private static void Main() {
            Config.TickPerSecond = 60;
            Config.ScreenWidth = 50;
            Config.ScreenHeight = 30;
            //用编辑器模式开始游戏
            WinformEngine.OpenWithEditor(StartGame, "HealerSimulator");
        }

        private static void StartGame() {

            UIFactroy.CreateLabel(new Vector2(2, 2), "123asd");
            //开启游戏进入场景
            //StartScene scene = new StartScene();
            //SceneManager.Load(scene, LoadSceneMode.Single);
            //游戏的主场景本质上不控制任何逻辑,只是与数据进行绑定- 更改 - 显示等.
            //游戏的数据都由gameMode说的算
        }
    }
}

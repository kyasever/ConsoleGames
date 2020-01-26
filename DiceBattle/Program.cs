using System;
using System.Collections.Generic;
using System.Linq;
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

            GameObject.CreateWith<Grid>();

            //开启游戏进入场景
            //StartScene scene = new StartScene();
            //SceneManager.Load(scene, LoadSceneMode.Single);
            //游戏的主场景本质上不控制任何逻辑,只是与数据进行绑定- 更改 - 显示等.
            //游戏的数据都由gameMode说的算
        }
    }

    public class Grid : Script
    {

        public string pic = "坦";
        
        public override void Awake() {
            var o1 = UIUtils.CreatePointObj(pic);
            o1.SetParent(GameObject);
            var o2 = ProgressBar.Create(5);
            o2.SetParent(GameObject);
            o2.LocalPosition = Vector2.Right;

            Position = new Vector2(10, 10);
        }
    }
}

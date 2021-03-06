﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Destroy;
using Destroy.Standard;
using Destroy.Winform;

namespace HealerSimulator
{
    static class Program
    {
        private static void Main()
        {
            Config.TickPerSecond = 120;
            Config.ScreenWidth = 70;
            Config.ScreenHeight = 40;
            Config.CharWidth = CharWidthEnum.Double;
            //用编辑器模式开始游戏
            WinformEngine.OpenWithEditor(StartGame,"HealerSimulator");
        }

        private static void StartGame()
        {
            //开启游戏进入场景
            StartScene scene = new StartScene();
            SceneManager.Load(scene, LoadSceneMode.Single);
            //游戏的主场景本质上不控制任何逻辑,只是与数据进行绑定- 更改 - 显示等.
            //游戏的数据都由gameMode说的算
        }
    }

    public class StartScene : Scene
    {
        //开启游戏主场景
        MainScene scene;
        public override void OnStart()
        {
            UIFactroy.CreateButton(new Vector2(20, 23), "开始教学关卡",
                () => { GameMode.Instance.Init(); LoadScene(); });
            UIFactroy.CreateButton(new Vector2(20, 22), "开始关卡简单(0)",
                () => { GameMode.Instance.InitGame(0); LoadScene(); });
            UIFactroy.CreateButton(new Vector2(20, 21), "开始关卡中等(5)",
                () => { GameMode.Instance.InitGame(5); LoadScene(); });
            UIFactroy.CreateButton(new Vector2(20, 20), "开始关卡理论极限(10)",
                () => { GameMode.Instance.InitGame(10); LoadScene(); });
            UIFactroy.CreateButton(new Vector2(20, 19), "开始关卡测试用(100)",
                () => { GameMode.Instance.InitGame(100); LoadScene(); });



            var text = Utils.CreateTextAera(20, 3, true);
            text.Position = new Vector2(10, 14);
            text.Rendering("欢迎来到本游戏,本游戏目前只有一个试玩关卡. 建议游玩简单或者中等难度,后面两个仅供测试用(大概是过不了关的)");

            text = Utils.CreateTextAera(20, 5, true);
            text.Position = new Vector2(10, 8);
            text.Rendering("操作说明,鼠标指向为治疗目标,123为单体治疗技能,45为群体治疗技能。(推荐多用134技能,应急使用25技能，并注意自己的蓝条不要oom)");
        }

        private void LoadScene()
        {
            scene = new MainScene();
            SceneManager.Load(scene, LoadSceneMode.Single);
        }
    }

}

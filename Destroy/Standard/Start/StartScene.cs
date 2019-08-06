using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destroy.Standard
{
    /// <summary>
    /// 一个新的主场景,开始一个新的游戏
    /// </summary>
    public class StartScene : Scene
    {
        /// <summary>
        /// 用new 新建这个场景
        /// </summary>
        public StartScene() : base("StartScene")
        {

        }

        public override void OnStart()
        {
            var manager = GameObject.CreateWith<StartSceneManager>();
            if(HelpDialog.Instance == null)
                GameObject.CreateWith<HelpDialog>("helpmenu","help");
        }

    }

    internal class StartSceneManager : Script
    {
        public override void Awake()
        {
            int x = Config.ScreenWidth / 2 - 10; int y = Config.ScreenHeight * 7 / 10;
            var l = UIFactroy.CreateLabel(new Vector2(x - 4, y + 2), "使用鼠标或键盘,选择要进入的游戏场景");
            Button a = UIFactroy.CreateButton(new Vector2(x, y + 1), "[1.盒子射击demo]", StartGameTPS);
            Button b = UIFactroy.CreateButton(new Vector2(x, y), "[2.战棋寻路Demo]", StartGameSRPG);
        }

        public override void Update()
        {
            if(Input.GetKeyDown( ConsoleKey.D1))
            {
                
                StartGameTPS();
            }
            else if(Input.GetKeyDown( ConsoleKey.D2))
            {
                
                StartGameSRPG();
            }
        }

        internal void StartGameTPS()
        {

            Debug.Log("切换场景1");
            HelpDialog.Instance.Type = HelpDialog.TextType.TPS;
            var tpsScene = new TPSScene();
            SceneManager.Load(tpsScene, LoadSceneMode.Single);
        }

        internal void StartGameSRPG()
        {
            Debug.Log("切换场景2");
            HelpDialog.Instance.Type = HelpDialog.TextType.SRPG;
            var scene = new SRPGScene();
            SceneManager.Load(scene, LoadSceneMode.Single);
        }
    }

    internal class HelpDialog : Script
    {
        [HideInInspector]
        public static HelpDialog Instance;

        TextBox helpTextBox;
        Button returnBtn;

        public enum TextType
        {
            TPS,SRPG
        }

        private TextType type;

        public TextType Type 
        {
            set
            {
                type = value;
                if (value == TextType.TPS)
                {
                    helpTextBox.SetText("WSAD移动", 1);
                    helpTextBox.SetText("QE或者鼠标切枪", 2);
                    helpTextBox.SetText("空格射击,击中销毁物体", 3);
                    helpTextBox.SetText("手枪射速慢,单发", 4);
                    helpTextBox.SetText("步枪射速快,连发", 5);
                }
                else if(value == TextType.SRPG)
                {
                    helpTextBox.SetText("\"岩\"是己方角色", 1);
                    helpTextBox.SetText("左键点击角色", 2);
                    helpTextBox.SetText("展开移动范围并移动", 3);
                    helpTextBox.SetText("右键点击角色", 4);
                    helpTextBox.SetText("不限制距离寻路移动", 5);
                }
            }
            get
            {
                return type;
            }
        }


        public override void Awake()
        {
            //这个类是单例且不会随场景销毁的
            Instance = this;

            helpTextBox = UIFactroy.CreateTextBox(new Vector2(Config.ScreenWidth - 12,0),5,10);
            helpTextBox.GameObject.Parent = GameObject;
            var drawing =  UIFactroy.CreateBoxDrawingRect(new Vector2(Config.ScreenWidth - 12, 6), 1, 10);
            drawing.Parent = GameObject;
            returnBtn = UIFactroy.CreateButton(new Vector2(Config.ScreenWidth-11,7),"回到主界面",Return,10);
            returnBtn.GameObject.Parent = GameObject;

            InitText();

            SceneManager.DontDestroyOnLoad(GameObject);
        }

        private void Return()
        {
            InitText();
            Scene scene = new StartScene();
            SceneManager.Load(scene);
        }

        private void InitText()
        {
            helpTextBox.SetText("这是一个帮助UI", 1);
            helpTextBox.SetText("不会因为场景改变消失", 2);
            helpTextBox.SetText("点击上方按钮可以回到", 3);
            helpTextBox.SetText("这个主场景", 4);
            helpTextBox.SetText("", 5);
        }
    }
}

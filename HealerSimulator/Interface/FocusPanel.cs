using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Destroy;

namespace HealerSimulator
{
    public class FocusPanel : Script
    {
        FocusHUD focus, player;

        public override void Awake()
        {
            focus = CreateFocusHUD();
            player = CreateFocusHUD();
            player.LocalPosition = new Vector2(0, 5);
        }

        public override void Update()
        {
            focus.sourceCharacter = GameMode.Instance.FocusCharacter;
            player.sourceCharacter = GameMode.Instance.Player;
        }

        public FocusHUD CreateFocusHUD()
        {
            FocusHUD c = GameObject.CreateWith<FocusHUD>("Focus", "Focus", GameObject, new Vector2(0, 0), posList: Utils.CreateRecMesh(10, 2));
            return c;
        }
    }

    /// <summary>
    /// 焦点框体 显示人物的详细属性
    /// </summary>
    public class FocusHUD : Script
    {
        public Character sourceCharacter;

        public GameObject BoxDrawing;
        //第一条label 第一排.角色信息
        private Renderer label1;
        //第二排 备注信息
        private Renderer label2;
        //第三四排 血条
        private Renderer labelHP, labelMP;
        //第三四排 进度条
        private ProgressBarChar hpSlider, mpSlider;

        public override void Awake()
        {
            //添加边框对象
            BoxDrawing = UIFactroy.CreateBoxDrawingRect(Position - new Vector2(1, 1), 4, 24);
            BoxDrawing.Parent = GameObject;

            label1 = Utils.CreateLabel(24, parent: GameObject, localPosition: new Vector2(0, 3));
            label2 = Utils.CreateLabel(24, parent: GameObject, localPosition: new Vector2(0, 2));
            labelHP = Utils.CreateLabel(8, parent: GameObject, localPosition: new Vector2(0, 1));
            labelMP = Utils.CreateLabel(8, parent: GameObject, localPosition: new Vector2(0, 0));

            hpSlider = ProgressBarChar.Create(16);
            hpSlider.GameObject.Parent = GameObject;
            hpSlider.LocalPosition = new Vector2(8, 1);

            mpSlider = ProgressBarChar.Create(16);
            mpSlider.GameObject.Parent = GameObject;
            mpSlider.LocalPosition = new Vector2(8, 0);
            mpSlider.fullColor = Color.Blue;
            mpSlider.enableColor = false;
        }

        public override void Update()
        {
            if (sourceCharacter != null)
            {
                Refresh(sourceCharacter);
            }
        }

        public void Refresh()
        {
            //
        }
        public void Refresh(Character c)
        {
            label1.Rendering(c.ClassName + "   " + c.CharacterName);
            label2.Rendering(c.Description);
            string hp = "[ " + CharUtils.ParseNum(c.HP, 4) + "/" + CharUtils.ParseNum(c.MaxHP, 4) + "]";
            labelHP.Rendering("[HP]" + hp);

            string mp = "[ " + CharUtils.ParseNum(c.MP, 4) + "/" + CharUtils.ParseNum(c.MaxMP, 4) + "]";
            labelMP.Rendering("[MP]" + mp);
            

            hpSlider.Value = c.HP;
            hpSlider.MaxValue = c.MaxHP;

            mpSlider.Value = c.MP;
            mpSlider.MaxValue = c.MaxMP;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Destroy;

namespace HealerSimulator
{
    public class PlayerPanel : Script
    {

        public List<SkillHUD> skillList = new List<SkillHUD>();

        public override void Awake()
        {
            var list = GameMode.Instance.Player.SkillList;
            for (int i = 0; i < list.Count; i++)
            {
                var hud = CreateSkillHUD();
                hud.sourceSkill = list[i];
                skillList.Add(hud); ;
            }
            RefrestGrid();

            GameObject.CreateWith<CastStripHUD>("CastStrip", "H", GameObject, new Vector2(0, 5)).sourceCharacter = GameMode.Instance.Player;
        }

        /// <summary>
        /// 给Grid里的东西排序,排位置
        /// </summary>
        public void RefrestGrid()
        {
            List<Vector2> location = new List<Vector2>();
            for (int i = 0; i < 30; i = i + 7)
            {
                location.Add(new Vector2(i, 1));
            }

            for (int i = 0; i < Math.Min(skillList.Count, 10); i++)
            {
                skillList[i].LocalPosition = location[i];
            }
        }

        public SkillHUD CreateSkillHUD()
        {
            SkillHUD s = GameObject.CreateWith<SkillHUD>("Skill" + skillList.Count.ToString(), "Focus", GameObject, new Vector2(0, 0));
            return s;
        }
    }

    /// <summary>
    /// 施法条模块
    /// </summary>
    public class CastStripHUD : Script
    {
        public Character sourceCharacter;

        public GameObject BoxDrawing;
        private Renderer labelUp;
        private ProgressBarChar progressCommonCD;
        private ProgressBar progressCasting;

        public override void Awake()
        {
            //添加边框对象
            BoxDrawing = UIFactroy.CreateBoxDrawingRect(Position - new Vector2(1, 1), 2, 30);
            BoxDrawing.Parent = GameObject;

            //左上的的label 长度20 表示正在释放的技能的名字和施法时间等
            var obj = new GameObject("StateLabel", "UI", GameObject, new Vector2(0, 1), Utils.CreateLineMesh(20));
            labelUp = obj.AddComponent<Renderer>();
            labelUp.Init(RendererMode.UI, (int)Layer.Label);

            //上面的条表示公cd
            progressCommonCD = ProgressBarChar.Create(10,fillChar:'<');
            progressCommonCD.GameObject.Parent = GameObject;
            progressCommonCD.LocalPosition = new Vector2(20, 1);
            progressCommonCD.emptyColor = Color.Green;
            progressCommonCD.fullColor = Color.Red;

            //下面的用实心进度条 表示施法条
            progressCasting = ProgressBar.Create(30);
            progressCasting.GameObject.Parent = GameObject;
            progressCasting.LocalPosition = new Vector2(0, 0);
        }

        public override void Update()
        {
            if(sourceCharacter != null)
            {

                Refresh(sourceCharacter);
            }
        }

        public void Refresh(Character c)
        {
            if(c.CurSkill == null)
            {
                labelUp.Rendering("当前没有释放技能");

                progressCommonCD.Value = c.CommonTime;
                progressCommonCD.MaxValue = c.CommonInterval;

                progressCasting.Value = 0;
                return;
            }
            float duringTime = c.CurSkill.CastingInterval - c.CurSkill.CastingRelease;
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append(c.CurSkill.skillName);
            sb.Append("][");
            sb.Append(duringTime.ToString("F2"));
            sb.Append("/");
            sb.Append(c.CurSkill.CastingInterval.ToString("F2"));
            sb.Append("]");
            labelUp.Rendering(sb.ToString());

            progressCommonCD.Value = c.CommonTime;
            progressCommonCD.MaxValue = c.CommonInterval;

            progressCasting.Value = duringTime;
            progressCasting.MaxValue = c.CurSkill.CastingInterval;
        }
    }


    /// <summary>
    /// 单个技能显示模块
    /// </summary>
    public class SkillHUD : Script
    {
        /// <summary>
        /// 数据绑定
        /// </summary>
        public Skill sourceSkill;

        #region 制表符边框和换色操作
        /// <summary>
        /// 制表符边框,可以改变颜色以表示选中状态
        /// </summary>
        public GameObject BoxDrawing;

        private Color unSelectColor = Color.White;
        private Color SelectColor = Color.Cyan;
        public void Select(bool b)
        {
            if (b)
            {
                BoxDrawing.GetComponent<Renderer>().SetForeColor(SelectColor);
                BoxDrawing.GetComponent<Renderer>().SetDepth((int)Layer.SelectBox);
            }
            else
            {
                BoxDrawing.GetComponent<Renderer>().SetForeColor(unSelectColor);
                BoxDrawing.GetComponent<Renderer>().SetDepth((int)Layer.Box);
            }
        }

        #endregion

        private Renderer labelUp;
        private Renderer labelDown;

        public override void Awake()
        {
            //添加边框对象
            BoxDrawing = UIFactroy.CreateBoxDrawingRect(Position - new Vector2(1, 1), 2, 6);
            BoxDrawing.Parent = GameObject;

            //上面的label 长度6 表示技能名字
            var obj = new GameObject("StateLabel", "UI", GameObject, new Vector2(0, 1), Utils.CreateLineMesh(6));
            labelUp = obj.AddComponent<Renderer>();
            labelUp.Init(RendererMode.UI, (int)Layer.Label);

            //下面的label长度6 表示技能cd
            obj = new GameObject("CDLabel", "UI", GameObject, new Vector2(0, 0), Utils.CreateLineMesh(6));
            labelDown = obj.AddComponent<Renderer>();
            labelDown.Init(RendererMode.UI, (int)Layer.Label);
        }

        public override void Update()
        {
            if (sourceSkill != null)
            {
                Refresh(sourceSkill);
            }
        }

        public void Refresh(Skill s)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(s.skillName);
            sb.Append("[");
            sb.Append(s.Key.ToString("G"));
            sb.Append("]");
            if (s.CanCast)
            {
                labelUp.Rendering(sb.ToString(), Color.Green);
            }
            else
            {
                labelUp.Rendering(sb.ToString(), Color.Red);
            }

            //说明是瞬发技能
            if (s.CDDefault < 0)
            {
                labelDown.Rendering("可用", Color.Green);
            }

            else
            {
                if (s.CDRelease < 0)
                {
                    labelDown.Rendering("可用", Color.Green);
                }
                else
                {
                    sb = new StringBuilder();
                    sb.Append(s.CDRelease.ToString("F1"));
                    sb.Append("/");
                    sb.Append(s.CD.ToString("F1"));
                    labelDown.Rendering(sb.ToString());
                }
            }
        }
    }

}

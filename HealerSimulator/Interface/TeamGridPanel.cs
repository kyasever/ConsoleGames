using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Destroy;

namespace HealerSimulator
{
    /// <summary>
    /// 团队框架的UI框架,作为一个整体,暂定包含10个位置,负责处理它们之间的交互
    /// </summary>
    public class TeamGridPanel : Script
    {
        public List<CharacterHUD> hpBoxList = new List<CharacterHUD>();
        public int SelectIndex;

        private GameMode game = GameMode.Instance;

        public override void Awake()
        {
            foreach(var c in game.TeamCharacters)
            {
                var hud = CreateCharacterHUD();
                hud.sourceCharacter = c;
                hpBoxList.Add(hud);
            }
            RefrestGrid();
        }

        /// <summary>
        /// 给Grid里的东西排序,排位置
        /// </summary>
        public void RefrestGrid()
        {
            List<Vector2> location = new List<Vector2>();
            for (int i = 13; i > 0; i = i - 3)
            {
                location.Add(new Vector2(1, i));
            }
            for (int i = 13; i > 0; i = i - 3)
            {
                location.Add(new Vector2(27, i));
            }

            for(int i = 0;i<Math.Min(hpBoxList.Count,10);i++)
            {
                hpBoxList[i].LocalPosition = location[i];
            }

        }

        public CharacterHUD CreateCharacterHUD()
        {
            CharacterHUD c = GameObject.CreateWith<CharacterHUD>("Character" + hpBoxList.Count.ToString(), "Focus", GameObject, new Vector2(0,0), posList: Utils.CreateRecMesh(25, 2));
            return c;
        }
    }

    /// <summary>
    /// TODO 增加显示 重复就重复吧只是暂时重复一点,而且和别的UI区分就很大了
    /// </summary>
    public class BossHUD : Script
    {
        public Character sourceCharacter;

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

        /// <summary>
        /// 整体挂载有UI检测组件,但这个物体没有显示,显示位于子物体
        /// </summary>
        private RayCastTarget rayCastTargetCom;

        /// <summary>
        /// 进度条组件
        /// </summary>
        private ProgressBar progressBar;

        /// <summary>
        /// label组件,mesh由自己定制,不使用Mesh
        /// </summary>
        private Renderer labelCom;

        public override void Awake()
        {
            //虽然添加了raycast对象,但是还没添加pos....
            //而且panel -> obj的管理模式非常值得借鉴
            rayCastTargetCom = AddComponent<RayCastTarget>();
            rayCastTargetCom.OnMoveInEvent += () => { Select(true); };
            rayCastTargetCom.OnMoveOutEvent += () => { Select(false); };

            //添加边框对象
            BoxDrawing = UIFactroy.CreateBoxDrawingRect(Position - new Vector2(1, 1), 2, 40);
            BoxDrawing.Parent = GameObject;
            Select(false);

            //添加Label对象
            List<Vector2> posList = new List<Vector2>();
            for (int i = 0; i < 40; i++)
            {
                posList.Add(new Vector2(i, 0));
            }
            GameObject labelObj = new GameObject("StateLabel", "UI", GameObject, new Vector2(0, 1), posList);
            labelCom = labelObj.AddComponent<Renderer>();
            labelCom.Init(RendererMode.UI, (int)Layer.Label);

            //添加血条对象
            progressBar = ProgressBar.Create(length: 40);
            progressBar.GameObject.Parent = GameObject;
            progressBar.LocalPosition = new Vector2(0, 0);

            
        }

        public override void Update()
        {
            sourceCharacter = GameMode.Instance.Boss;
            if (sourceCharacter != null)
            {
                Refresh(sourceCharacter);
            }
        }

        public void Refresh(Character c)
        {
            StringBuilder sb = new StringBuilder().Append("BOSS:").Append(c.CharacterName).Append("   ");
            sb.Append("[").Append(c.HP).Append("/").Append(c.MaxHP).Append("]");
            labelCom.Rendering(sb.ToString());
            progressBar.Value = c.HP;
            progressBar.MaxValue = c.MaxHP;
        }
     }

    /// <summary>
    /// 初始化的时候给它绑定一个Characer,之后每次Update它都会从C中提取数据进行渲染
    /// TODO 增加双缓冲
    /// </summary>
    public class CharacterHUD : Script
    {
        public override void Awake()
        {
            rayCastTargetCom = AddComponent<RayCastTarget>();
            rayCastTargetCom.OnMoveInEvent += () => { Select(true); };
            rayCastTargetCom.OnMoveOutEvent += () => { Select(false); };

            //添加边框对象
            BoxDrawing = UIFactroy.CreateBoxDrawingRect(Position - new Vector2(1, 1), 2, 25);
            BoxDrawing.Parent = GameObject;
            Select(false);

            //添加Label对象
            List<Vector2> posList = new List<Vector2>();
            for (int i = 0; i < 25; i++)
            {
                posList.Add(new Vector2(i, 0));
            }
            for (int i = 18; i < 25; i++)
            {
                posList.Add(new Vector2(i, -1));
            }
            GameObject labelObj = new GameObject("StateLabel", "UI", GameObject, new Vector2(0, 1), posList);
            labelCom = labelObj.AddComponent<Renderer>();
            labelCom.Init(RendererMode.UI, (int)Layer.Label);
            //添加血条对象
            progressBar = ProgressBarChar.Create(length: 18);
            progressBar.GameObject.Parent = GameObject;
            progressBar.LocalPosition = new Vector2(0, 0);
        }

        public Color DefaultColor = Color.White;

        public Character sourceCharacter;

        public override void Update()
        {
            if(sourceCharacter != null)
            {
                Refresh(sourceCharacter);
            }
        }
        /// <summary>
        /// 刷新,根据所有数据刷新显示,这个排版方式虽然还可以,但是过于不优雅了.
        /// 日后需要从引擎层面加以改善,目前是notepad排版,然后用这个拼
        /// </summary>
        public void Refresh(Character character)
        {
            ColorStringBuilder sb = new ColorStringBuilder();
            sb.AppendString("-[", DefaultColor);
            sb.AppendString(CharUtils.SubStr(character.ClassName, 2), Color.Blue);
            sb.AppendString("][", DefaultColor);
            sb.AppendString(CharUtils.SubStr(character.CharacterName, 12), Color.DarkCyan);
            sb.AppendString("]-[HP]", DefaultColor);
            string hp = "[ " + CharUtils.ParseNum(character.HP, 4) + "/" + CharUtils.ParseNum(character.MaxHP, 4) + "]";
            sb.AppendString(hp);

            if (character.Buffs.Count > 0)
            {
                sb.AppendString("-[", DefaultColor);
                sb.AppendString(character.Buffs[0].Icon);
            }
            #region 略 太丑了
            if (character.Buffs.Count > 1)
            {
                sb.AppendString("][", DefaultColor);
                sb.AppendString(character.Buffs[1].Icon);
            }
            if (character.Buffs.Count > 2)
            {
                sb.AppendString("][", DefaultColor);
                sb.AppendString(character.Buffs[2].Icon);
            }
            if (character.Buffs.Count > 3)
            {
                sb.AppendString("] -[", DefaultColor);
                sb.AppendString(character.Buffs[3].Icon);
            }
            if (character.Buffs.Count > 4)
            {
                sb.AppendString("][", DefaultColor);
                sb.AppendString(character.Buffs[4].Icon);
            }
            #endregion
            if (character.Buffs.Count > 5)
            {
                sb.AppendString("][", DefaultColor);
                sb.AppendString(character.Buffs[5].Icon);
                sb.AppendString("] ", DefaultColor);
            }

            labelCom.Rendering(sb.ToRenderer());

            progressBar.MaxValue = character.MaxHP;
            progressBar.Value = character.HP;
        }

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

            if(b)
            {
                if(GameMode.Instance.FocusCharacter != sourceCharacter && sourceCharacter.IsAlive)
                {
                    GameMode.Instance.FocusCharacter = sourceCharacter;
                }
            }
        }

        #endregion

        /// <summary>
        /// 整体挂载有UI检测组件,但这个物体没有显示,显示位于子物体
        /// </summary>
        private RayCastTarget rayCastTargetCom;

        /// <summary>
        /// 进度条组件
        /// </summary>
        private ProgressBarChar progressBar;

        /// <summary>
        /// label组件,mesh由自己定制,不使用Mesh
        /// </summary>
        private Renderer labelCom;




    }

}

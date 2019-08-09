using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Destroy;

namespace HealerSimulator
{
    /// <summary>
    /// 对于Robot和Boss来说,不需要那么多复杂的判定.只需要执行结果就可以了.
    /// 比如技能没有施法,爆击,闪避等. 只需要判定减伤就可以了
    /// ATK 也可以是负数.就认为是治疗了. 只吃接受方的加成,不吃输出方的加成.
    /// 一个普通的Skill先经过初步处理为一个SimpleSkill 然后再进行下一步被攻击方的判定
    /// 等controller大概没有bug了,要抽分controller的逻辑来供给其他单位使用
    /// </summary>
    public class SimpleSkill
    {
        public int Atk = 100;
    }



    //引擎无关,游戏核心部分,暂时不考虑场景管理
    public class GameMode
    {
        public static Random RandomInstance = new Random(5);

        /// <summary>
        /// 之后要更换方式
        /// 依旧使用Behit方式,然后给Player中的增加
        /// </summary>
        public void CastSkill(Skill s,Character t)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} 对 {1} 释放了 {2} ", s.Caster.CharacterName, t.CharacterName, s.skillName);
            //消耗蓝
            s.Caster.MP -= s.MPCost;
            //给目标加血
            if(s.Type == Skill.SkillType.NomalHeal)
            {
                t.HP += s.Atk;

                sb.AppendFormat(" | 造成了:{0}治疗效果", s.Atk.ToString());
                //添加一条Skada记录
                Skada.Instance.AddRecord(new SkadaRecord() { Accept = t, Source = s.Caster, UseSkill = s, Value = s.Atk });

            }
            else if(s.Type == Skill.SkillType.MiutiHeal)
            {
                foreach(var c in TeamCharacters)
                {
                    c.HP += s.Atk;

                    sb.AppendFormat(" | 造成了:{0}治疗效果", s.Atk.ToString());
                    //添加一条Skada记录
                    Skada.Instance.AddRecord(new SkadaRecord() { Accept = c, Source = s.Caster, UseSkill = s, Value = s.Atk });

                }
            }

            if (s.CDDefault > 0)
            {
                s.CDRelease = s.CD;
            }

            Debug.Log(sb.ToString());
        }

        private static readonly GameMode instance = new GameMode();
        public static GameMode Instance
        {
            get
            {
                return instance;
            }
        }

        private GameMode()
        {

        }
        public Character Boss;

        public Character Player;

        public Character FocusCharacter;

        public List<Character> TeamCharacters;

        public List<Character> DeadCharacters = new List<Character>();

        public Action UpdateEvent;


        public void Clear()
        {
            TeamCharacters = new List<Character>();
            DeadCharacters = new List<Character>();
            Boss = null;
            Player = null;
            FocusCharacter = null;
            UpdateEvent = null;
        }

        /// <summary>
        /// 创建教学关
        /// </summary>
        public void Init()
        {
            Boss = Character.CreateNPC("B", "这一个长长长的BOSS", 25000);
            Boss.HP = 22000;

            TeamCharacters = new List<Character>();
            TeamCharacters.Add(Character.CreateNPC("远", "粗心的法师", 1650));
            TeamCharacters.Add(Character.CreateNPC("坦", "平庸的坦克", 2800));
            TeamCharacters.Add(Character.CreateNPC("近", "鲁莽的斗士", 2200));
            TeamCharacters.Add(Character.CreateNPC("近", "水晶核心", 6000));
            Character c= Character.CreateHealer();

            var controller = new PlayerController(c);



            TeamCharacters.Add(c);

            Player = c;
            FocusCharacter = Player;
        }

        public void InitGame(int difficultyLevel)
        {
            //创建小队
            TeamCharacters = new List<Character>();
            TeamCharacters.Add(Character.CreateNPC("远", "粗心的法师", 1650));
            TeamCharacters.Add(Character.CreateNPC("坦", "平庸的坦克", 2800));
            TeamCharacters[1].Duty = TeamDuty.Tank;
            TeamCharacters.Add(Character.CreateNPC("近", "鲁莽的斗士", 2200));
            TeamCharacters.Add(Character.CreateNPC("近", "可靠的武士", 1800));

            foreach(var v in TeamCharacters)
            {
                v.HP = v.MaxHP;
                v.Evasion = 0.7f;
                v.controller = new NPCController(v);
            }
            
            //创建玩家
            Character c = Character.CreateHealer();
            c.Evasion = 1f - difficultyLevel * 0.1f;
            c.controller = new PlayerController(c);
            TeamCharacters.Add(c);

            //创建BOSS
            int hp = (int)(25000 * (1 + difficultyLevel / 10f));
            Boss = Character.CreateNPC("B", "这一个长长长的BOSS", hp);
            Boss.HP = Boss.MaxHP;
            Boss.controller = new BossController(Boss, difficultyLevel);

            //创建游戏控制器
            new GameContrtoller();

            //初始化game设定
            Player = c;
            FocusCharacter = Player;
        }

    }
}

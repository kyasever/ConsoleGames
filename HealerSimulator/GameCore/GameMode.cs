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
        /// 使用一个极度复杂的方法来解决技能结算问题,内部之后可能再考虑用设计模式来进行整理,但是入口一定是只有这一个
        /// s: 准备结算的技能 t:技能的攻击目标
        /// 方法通过判定s和t的各种数据来得出结论和操作攻击结果
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
                foreach(var c in TeamCharaters)
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

        private static GameMode instance;
        public static GameMode Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new GameMode();
                }
                return instance;
            }
        }

        private GameMode()
        {

        }
        public Character Boss;

        public Character Player;

        public Character FocusCharacter;

        public List<Character> TeamCharaters; 

        /// <summary>
        /// 创建教学关
        /// </summary>
        public void Init()
        {
            Boss = Character.CreateNPC("B", "这一个长长长的BOSS", 25000);
            Boss.HP = 22000;

            TeamCharaters = new List<Character>();
            TeamCharaters.Add(Character.CreateNPC("法", "粗心的法师", 1650));
            TeamCharaters.Add(Character.CreateNPC("坦", "平庸的坦克", 2800));
            TeamCharaters.Add(Character.CreateNPC("斗", "鲁莽的斗士", 2200));
            TeamCharaters.Add(Character.CreateNPC("中", "水晶核心", 6000));
            Character c= Character.CreateHealerPaladin();


            TeamCharaters.Add(c);
            Player = c;
            FocusCharacter = Player;
        }

    }
}

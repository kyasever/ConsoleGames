using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Destroy;



namespace HealerSimulator
{

    /// <summary>
    /// 技能类. 考虑使用继承进行扩展. 技能的释放是一个权限很大的函数,它可以获得游戏中的所有的数据并决定该干什么.
    /// </summary>
    public class Skill
    {
        public int Atk = 100;
         
        public enum SkillType
        {
            NomalHeal,
            MiutiHeal,
        }

        public SkillType Type;

        public static Skill CreateSkillP1(Character caster, ConsoleKey key)
        {
            //secDecrypt 


            Skill s = new Skill()
            {
                Key = key,
                Caster = caster,
                Type = SkillType.NomalHeal,
                Atk = 300,
                CastingDefaultInterval = 3f,
                MPCost = 30,
                skillName = "治疗术",
                skillDiscription = "慢速而有效的治疗",
            };
            return s;
        }

        public static Skill CreateSkillP2(Character caster, ConsoleKey key)
        {
            Skill s = new Skill()
            {
                Key = key,
                Caster = caster,
                Type = SkillType.NomalHeal,
                Atk = 450,
                CastingDefaultInterval = -1f,
                MPCost = 200,
                skillName = "快速治疗",
                skillDiscription = "瞬发,高蓝耗,高治疗",
            };
            return s;
        }

        public static Skill CreateSkillP3(Character caster, ConsoleKey key)
        {
            Skill s = new Skill()
            {
                Key = key,
                Caster = caster,
                Type = SkillType.NomalHeal,
                Atk = 600,
                CastingDefaultInterval = 3f,
                MPCost = 150,
                skillName = "强效治疗",
                skillDiscription = "强而有效的单体治疗",
            };
            return s;
        }

        public static Skill CreateSkillP4(Character caster, ConsoleKey key)
        {
            Skill s = new Skill()
            {
                Key = key,
                Caster = caster,
                Type = SkillType.MiutiHeal,
                Atk = 200,
                CastingDefaultInterval = 3.5f,
                MPCost = 150,
                skillName = "治疗祷言",
                skillDiscription = "有效的群体治疗",
            };
            return s;
        }

        public static Skill CreateSkillP5(Character caster, ConsoleKey key)
        {
            Skill s = new Skill()
            {
                Key = key,
                Caster = caster,
                Type = SkillType.MiutiHeal,
                CDDefault = 90f,
                Atk = 1000,
                CastingDefaultInterval = -1f,
                MPCost = 100,
                skillName = "救赎祷言",
                skillDiscription = "应急群体治疗",
            };
            return s;
        }



        public ConsoleKey Key;

        public Character Caster;



        public bool CanCast
        {
            get
            {
                if (CDRelease > 0)
                {
                    return false;
                }
                if (Caster.MP < MPCost)
                {
                    return false;
                }
                return true;
            }
        }

        public int MPCost = 10;

        /// <summary>
        /// 默认cd
        /// </summary>
        public float CDDefault = -1f;

        /// <summary>
        /// 实际CD
        /// </summary>
        public float CD { get => CDDefault / Caster.Speed; }

        /// <summary>
        /// 剩余CD 小于0表示冷却好了
        /// </summary>
        public float CDRelease = -1;

        /// <summary>
        /// 默认施法时间,负数表示为瞬发技能
        /// </summary>
        public float CastingDefaultInterval = 1.5f;

        /// <summary>
        /// 判断是否是瞬发技能
        /// </summary>
        public bool IsInstant { get { return (CastingDefaultInterval < 0); } }

        /// <summary>
        /// 实际施法时间
        /// </summary>
        public float CastingInterval { get => CastingDefaultInterval / Caster.Speed; }

        /// <summary>
        /// 剩余施法时间
        /// </summary>
        public float CastingRelease = -1f;

        public string skillName = "技能名";
        public string skillDiscription = "技能效果";
    }
}

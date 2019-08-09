using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Destroy;

namespace HealerSimulator
{
    public enum TeamDuty
    {
        Tank,
        Healer,
        MeleeDPS,
        RangeDPS,
    }

    /// <summary>
    /// 角色类 主要负责处理游戏逻辑和数据,和显示完全脱钩,和Destroy完全脱钩,没有生命周期,只有数据,和处理数据的函数
    /// 不管是什么角色,都有这里面所有的属性,只是处理方式不同.
    /// hpmax = 1000 + sta * 20 mpmax = 500 + int * 30
    /// </summary>
    public class Character
    {
        public static Character CreateNPC(string className, string name, int maxHp)
        {
            Character c = new Character()
            {
                Stama = (maxHp - 1000) / 20,
                ClassName = className,
                CharacterName = name,
            };
            return c;
        }

        /// <summary>
        /// 使用静态方法创建一个标准玩家控制的角色,这个角色的职业是Paladin
        /// </summary>
        /// <returns></returns>
        public static Character CreateHealer()
        {
            Character c = new Character()
            {
                //人物的基础属性
                Stama = 46,
                Speed = 1.2f,
                Crit = 0.2f,
                Inte = 55,
                Master = 0f,
                Defense = 0f,
                MaxAP = 0,
                ClassName = "奶",
                CharacterName = "完美的操纵者",
                Description = "玩家控制单位",
            };
            c.HP = c.MaxHP;
            c.MP = c.MaxMP;
            c.AP = c.MaxAP;
            c.SkillList.Add(Skill.CreateSkillP1(c, ConsoleKey.D1));
            c.SkillList.Add(Skill.CreateSkillP2(c, ConsoleKey.D2));
            c.SkillList.Add(Skill.CreateSkillP3(c, ConsoleKey.D3));
            c.SkillList.Add(Skill.CreateSkillP4(c, ConsoleKey.D4));
            c.SkillList.Add(Skill.CreateSkillP5(c, ConsoleKey.D5));
            return c;
        }

        /// <summary>
        /// 暂时加一个这个,让Character接受某个Controller的托管
        /// </summary>
        public Controller controller;

        public TeamDuty Duty = TeamDuty.MeleeDPS;

        /// <summary>
        /// 闪避率,这个值通常取决于这个角色的操作水平,只有可以被闪避的伤害才可以触发闪避效果
        /// 取值范围0-1 1为完全闪避. 通常玩家控制角色闪避率为1,NPC闪避率较低
        /// </summary>
        public float Evasion = 0f;


        /// <summary>
        /// 返回是否可以命中该单位的判定,取决于该单位的闪避,和命中
        /// </summary>
        /// <param name="hit">命中修正 0为无修正 -1为 -100%命中 1为+100%命中</param>
        /// <returns></returns>
        public bool CanHit(float hit)
        {
            float f = Evasion - hit;
            double d = GameMode.RandomInstance.NextDouble();
            return d > f;
        }

        /// <summary>
        /// 是否可以爆击的判定
        /// </summary>
        /// <returns></returns>
        public bool CanCrit()
        {
            return GameMode.RandomInstance.NextDouble() < Crit;
        }


        /// <summary>
        /// 耐力 提升20血量
        /// </summary>
        public int Stama = 150;

        /// <summary>
        /// 急速. 取值1+ 2则为2倍速,0.5则为0.5倍速
        /// </summary>
        public float Speed = 1.2f;

        /// <summary>
        /// 暴击 取值0-1
        /// </summary>
        public float Crit = 0.2f;

        /// <summary>
        /// 智力 提升蓝上限和回蓝
        /// </summary>
        public int Inte = 100;

        /// <summary>
        /// 精通 神秘效果
        /// </summary>
        public float Master = 0f;

        /// <summary>
        /// 减伤百分比
        /// </summary>
        public float Defense = 0f;

        /// <summary>
        /// 当前正在释放的法术,为null说明当前没有正在释放法术
        /// </summary>
        public Skill CastingSkill = null;

        /// <summary>
        /// 当开始施法时,只有空格可以打断施法.
        /// </summary>
        public bool IsCasting { get { return CastingSkill != null; } }

        /// <summary>
        /// 公cd = 1.5s减急速加成
        /// </summary>
        public float CommonInterval { get { return 1.5f / Speed; } }

        /// <summary>
        /// 公cd剩余时间
        /// </summary>
        public float CommonTime = 1f;

        /// <summary>
        /// 保存对应键位对应的技能
        /// </summary>
        public List<Skill> SkillList = new List<Skill>();

        /// <summary>
        /// 角色职业
        /// </summary>
        public string ClassName = "  ";

        /// <summary>
        /// 角色名字
        /// </summary>
        public string CharacterName = " ";

        /// <summary>
        /// 角色描述
        /// </summary>
        public string Description = "  ";

        private int hp = 1;
        /// <summary>
        /// 当前生命
        /// </summary>
        public int HP
        {
            get => hp;
            set
            {
                hp = value;
                if (hp > MaxHP)
                    hp = MaxHP;
                else if (hp <= 0)
                {
                    hp = 0;
                    IsAlive = false;
                    //OnDeath();
                }
            }
        }

        public bool IsAlive = true;

        /// <summary>
        /// 当前最大生命
        /// </summary>
        public int MaxHP { get { return Stama * 20 + 1000; } }

        private int mp = 0;
        //第二资源条
        public int MP
        {
            get => mp;
            set
            {
                mp = value;
                if (mp > MaxMP)
                    mp = MaxMP;
                else if (mp <= 0)
                {
                    mp = 0;
                }
            }
        }
        public int MaxMP { get { return Inte * 30 + 500; } }
        //第三资源条
        public int AP = 0;
        public int MaxAP = 0;


        /// <summary>
        /// BUFF状态栏
        /// </summary>
        public List<BUFF> Buffs = new List<BUFF>();
    }

    public class BUFF
    {
        /// <summary>
        /// 显示小图标,不超过一个字
        /// </summary>
        public string Icon = "爆";

        public float TotalTime = 0f;

        public float ReleaseTime = 0f;

        public string Description = "";
    }

}

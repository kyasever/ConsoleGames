namespace WizardAdvanture
{
    //只有三种类型 直接伤害 创造对象 治疗目标
    public enum SkillType
    {
        Damage,
        Create,
        Heal,
        Holy,
        Rest,
    }
    //攻击的逻辑判断 一个目标创造一个技能 并传给另一个目标.先检测是否可以打,再检测打造成的效果. skill少放东西 在流程里多判断一下
    public class Skill
    {
        public string name;
        public string discription = "这是技能描述";
        public SkillType type;

        // 攻击类型
        public int damage;
        public int costAct;
        public int costMana;
        // 治疗类型 可以恢复血蓝和行动力
        public int heal;
        public int addMana;
        public int addAct;

        //射程
        public int range;
        //使用准星
        public Scene.CursorType cursorType;

        // 创造类型
        public int summonHP;
        // 释放者
        public Target caster;

        #region 岩山的技能
        public static Skill CreateZhongChui(bool isIncreased)
        {
            Skill skill = new Skill();
            skill.type = SkillType.Damage;

            skill.range = 2;

            if (isIncreased)
            {
                skill.discription = "强化重锤,大范围攻击,吸引仇恨,10攻击";
                skill.damage = 10;
                skill.costMana = 0;
                skill.costAct = 3;
                skill.name = "强化重锤";
                skill.cursorType = Scene.CursorType.Square;
            }
            else
            {
                skill.discription = "重锤,中范围攻击,吸引仇恨,5攻击";
                skill.damage = 5;
                skill.costMana = 0;
                skill.costAct = 3;
                skill.name = "重锤";
                skill.cursorType = Scene.CursorType.Cross;
            }
            return skill;
        }
        public static Skill CreateShanFeng(bool isIncreased)
        {
            Skill skill = new Skill();
            skill.type = SkillType.Create;

            skill.summonHP = 250;
            skill.range = 2;
            if (isIncreased)
            {
                skill.discription = "强化山封,十字封路,消耗极高,路障250HP";
                skill.name = "强化山封";
                skill.costAct = 6;
                skill.costMana = 50;
                skill.cursorType = Scene.CursorType.Cross;
            }
            else
            {
                skill.discription = "山封,封一个格子,用来应急,路障250HP";
                skill.name = "山封";
                skill.costAct = 5;
                skill.costMana = 30;
                skill.cursorType = Scene.CursorType.Point;
            }
            return skill;
        }

        #endregion

        #region 火法的技能
        public static Skill CreateHuoYanDan(bool isIncreased)
        {
            Skill skill = new Skill();
            skill.type = SkillType.Damage;

            skill.costAct = 7;

            skill.cursorType = Scene.CursorType.Point;
            if (isIncreased)
            {
                skill.discription = "强化火球术,90伤害,击杀敌人只耗费10MP";
                skill.costMana = 40;
                skill.name = "强化火球术";
                skill.range = 8;
                skill.damage = 90;
            }
            else
            {
                skill.discription = "火球术,50伤害,击杀敌人回复10MP";
                skill.costMana = 20;
                skill.name = "火球术";
                skill.range = 6;
                skill.damage = 50;
            }
            return skill;
        }

        public static Skill CreateYunShi(bool isIncreased)
        {
            Skill skill = new Skill();
            skill.type = SkillType.Damage;

            skill.costMana = 120;

            if (isIncreased)
            {
                skill.discription = "强化陨石术,大范围120伤害,击杀4个敌人回本";
                skill.costAct = 16;
                skill.name = "强化陨石术";
                skill.range = 10;
                skill.damage = 120;
                skill.cursorType = Scene.CursorType.Square;
            }
            else
            {
                skill.discription = "陨石术,中范围80伤害,击杀4个敌人回本";
                skill.costAct = 12;
                skill.name = "陨石术";
                skill.range = 8;
                skill.damage = 80;
                skill.cursorType = Scene.CursorType.Cross;
            }
            return skill;
        }

        #endregion

        #region 治疗的技能
        public static Skill CreateLingQuan(bool isIncreased)
        {
            Skill skill = new Skill();
            skill.type = SkillType.Heal;
            skill.range = 2;
            skill.cursorType = Scene.CursorType.Point;

            if (isIncreased)
            {
                skill.discription = "强化灵泉,为目标回复5行动力和50MP";
                skill.name = "强化灵泉";
                skill.costAct = 4;
                skill.costMana = 40;
                skill.addAct = 5;
                skill.addMana = 50;
            }
            else
            {
                skill.discription = "灵泉,为目标回复5行动力和20MP";
                skill.name = "灵泉";
                skill.costAct = 5;
                skill.costMana = 20;
                skill.addAct = 5;
                skill.addMana = 20;
            }
            return skill;
        }
        public static Skill CreateYuHe(bool isIncreased)
        {
            Skill skill = new Skill();
            skill.type = SkillType.Heal;

            skill.heal = 100;
            if (isIncreased)
            {
                skill.discription = "快速愈合,2行动回复100HP";
                skill.costMana = 40;
                skill.range = 6;
                skill.costAct = 2;
                skill.name = "快速愈合";
                skill.cursorType = Scene.CursorType.Point;
            }
            else
            {
                skill.discription = "愈合,4行动回复100HP";
                skill.costMana = 50;
                skill.range = 4;
                skill.costAct = 4;
                skill.name = "愈合";
                skill.cursorType = Scene.CursorType.Point;
            }
            return skill;
        }

        #endregion

        #region 冰法的技能
        public static Skill CreateBilzzard(bool isIncreased)
        {
            Skill skill = new Skill();
            skill.type = SkillType.Damage;
            skill.costAct = 6;
            skill.cursorType = Scene.CursorType.Square;
            if (isIncreased)
            {
                skill.discription = "强化暴风雪,大范围40伤害";
                skill.costMana = 40;
                skill.name = "强化暴风雪";
                skill.range = 7;
                skill.damage = 40;
            }
            else
            {
                skill.discription = "暴风雪,大范围30伤害";
                skill.costMana = 50;
                skill.name = "暴风雪";
                skill.range = 4;
                skill.damage = 30;
            }
            return skill;
        }

        public static Skill CreateIceWall(bool isIncreased)
        {
            Skill skill = new Skill();
            skill.type = SkillType.Create;
            skill.costAct = 14;
            skill.costMana = 140;
            skill.range = 6;
            skill.cursorType = Scene.CursorType.Sline;

            if (isIncreased)
            {
                skill.discription = "强化冰墙,直线造墙,墙350HP";
                skill.name = "强化冰墙";
                skill.summonHP = 350;
            }
            else
            {
                skill.discription = "冰墙,直线造墙,墙150HP";
                skill.name = "冰墙";
                skill.summonHP = 150;
            }
            return skill;
        }
        #endregion

        #region 影刺的技能
        //对满血敌人造成4倍伤害 强化背刺 有mp消耗,需要刀扇回能 背刺 送的
        public static Skill CreateBeiCi(bool isIncreased)
        {
            Skill skill = new Skill();
            skill.type = SkillType.Damage;

            skill.range = 1;
            skill.costAct = 2;
            skill.cursorType = Scene.CursorType.Point;
            if (isIncreased)
            {
                skill.discription = "致命伏击,20伤害,对满血目标造成4倍伤害,高耗蓝";
                skill.costMana = 30;
                skill.name = "致命伏击";
                skill.damage = 20;
            }
            else
            {
                skill.discription = "背刺,10伤害,对满血目标造成4倍伤害,低消耗";
                skill.costMana = 10;
                skill.name = "背刺";
                skill.damage = 10;
            }
            return skill;
        }

        public static Skill CreateDaoShan(bool isIncreased)
        {
            Skill skill = new Skill();
            skill.type = SkillType.Damage;

            skill.range = 0;
            skill.cursorType = Scene.CursorType.Square;


            if (isIncreased)
            {
                skill.costAct = 14;
                skill.discription = "狂风刀扇,40伤害,近战AOE,击中4个目标消耗持平";
                skill.costMana = 40;
                skill.damage = 40;
                skill.name = "狂风刀扇";
            }
            else
            {
                skill.costAct = 12;
                skill.discription = "刀扇,10伤害,近战AOE,击中2个目标消耗持平";
                skill.costMana = 20;
                skill.damage = 10;
                skill.name = "刀扇";
            }
            return skill;
        }
        #endregion

        #region 圣者的技能
        public static Skill CreateCuiZi(bool isIncreased)
        {
            Skill skill = new Skill();
            skill.type = SkillType.Damage;

            skill.costMana = 10;
            skill.damage = 10;
            skill.costAct = 2;

            if (isIncreased)
            {
                skill.discription = "神圣之锤,远距离大范围,产生仇恨,10伤害";
                skill.cursorType = Scene.CursorType.Square;
                skill.range = 6;
                skill.name = "神圣之锤";

            }
            else
            {
                skill.discription = "正义之锤,中距离单体,产生仇恨,10伤害";
                skill.cursorType = Scene.CursorType.Point;
                skill.range = 4;
                skill.name = "正义之锤";
            }
            return skill;
        }
        //目标生命值0  蓝耗0   治疗 4倍效果
        //目标生命值满 蓝耗100 治疗 0倍效果
        public static Skill CreateZhengjiu(bool isIncreased)
        {
            Skill skill = new Skill();
            skill.type = SkillType.Heal;

            skill.range = 3;
            skill.cursorType = Scene.CursorType.Point;
            if (isIncreased)
            {
                skill.discription = "拯救之光,根据目标生命百分比产生0-100蓝耗和80-320治疗";
                skill.costAct = 2;
                skill.heal = 80;
                skill.costMana = 60;
                skill.name = "拯救之光";
            }
            else
            {
                skill.discription = "救赎之光,根据目标生命百分比产生0-100蓝耗和50-200治疗";
                skill.costAct = 4;
                skill.heal = 50;
                skill.costMana = 100;
                skill.name = "救赎之光";
            }
            return skill;
        }
        #endregion
        //岩山 单体低消耗,多了个好用的远程拉怪技能
        //辅助 AOE 可以群体生效,在后期狂暴的时候方便处理
        //冰法 单体中消耗 射程中规中矩,可以直接秒杀炸弹
        //火法 中射程极高消耗 炮台 极高伤害 可以点杀boss,但炸弹不好处理
        //需要硬格或者硬吃
        public static Skill CreateShenZhang(string name)
        {
            Skill skill = new Skill();
            skill.type = SkillType.Holy;
            if (name == "岩山")
            {
                skill.discription = "封锁之击,远距离10伤害,产生仇恨";
                skill.name = "神杖-封锁之击";
                skill.range = 6;
                skill.damage = 10;
                skill.costAct = 2;
                skill.costMana = 0;
                skill.cursorType = Scene.CursorType.Point;
            }
            else if (name == "火鸟")
            {
                skill.discription = "天神之击,远距离250伤害,高消耗";
                skill.name = "神杖-天神之击";
                skill.range = 8;
                skill.damage = 250;
                skill.costAct = 10;
                skill.costMana = 100;
                skill.cursorType = Scene.CursorType.Point;
            }
            else if (name == "愈灵")
            {
                skill.discription = "净化之击,中距离10伤害,AOE";
                skill.name = "神杖-净化之击";
                skill.range = 6;
                skill.damage = 10;
                skill.costAct = 3;
                skill.costMana = 40;
                skill.cursorType = Scene.CursorType.Square;
            }
            else if (name == "暴雪")
            {
                skill.discription = "冰刺之击,远距离50伤害";
                skill.name = "神杖-冰刺之击";
                skill.range = 10;
                skill.damage = 50;
                skill.costAct = 4;
                skill.costMana = 40;
                skill.cursorType = Scene.CursorType.Point;
            }
            else if (name == "影刺")
            {
                skill.discription = "极限之击,近距离270伤害,极高消耗";
                skill.name = "神杖-极限之击";
                skill.range = 2;
                skill.damage = 270;
                skill.costAct = 20;
                skill.costMana = 100;
                skill.cursorType = Scene.CursorType.Point;
            }
            else if (name == "圣者")
            {
                skill.discription = "妨碍之击,远距离10伤害,造成仇恨和减速效果";
                skill.name = "神杖-妨碍之击";
                skill.range = 6;
                skill.damage = 20;
                skill.costAct = 2;
                skill.costMana = 20;
                skill.cursorType = Scene.CursorType.Cross;
            }
            return skill;
        }

        //每点行动力可以回复的量
        public static Skill CreateRest(int heal, int mana)
        {
            Skill skill = new Skill();
            skill.type = SkillType.Rest;
            //这个消耗所有行动力并按照消耗回复
            skill.addMana = mana;
            skill.heal = heal;
            skill.name = "休息";
            return skill;
        }
        //回合结束后的回复
        public static Skill CreateRestAfterTurn(int heal, int mana)
        {
            Skill skill = new Skill();
            skill.type = SkillType.Heal;
            skill.heal = heal;
            skill.addMana = mana;
            skill.name = "";
            return skill;
        }


        //小兵的普通攻击
        public static Skill CreateGuardAttack()
        {
            Skill skill = new Skill();
            skill.name = "守卫攻击";
            skill.type = SkillType.Damage;
            skill.costAct = 0;
            skill.costMana = 0;
            skill.damage = 30;
            skill.range = 2;
            return skill;
        }

        public static Skill CreateRookAttack()
        {
            Skill skill = new Skill();
            skill.name = "碾压";
            skill.type = SkillType.Damage;
            skill.damage = 80;
            return skill;
        }

        public static Skill CreateKnightAttack()
        {
            Skill skill = new Skill();
            skill.name = "骑士冲击";
            skill.type = SkillType.Damage;
            skill.damage = 120;
            return skill;
        }

        public static Skill CreateBoom()
        {
            Skill skill = new Skill();
            skill.name = "自毁爆炸";
            skill.type = SkillType.Damage;
            skill.costAct = 0;
            skill.costMana = 0;
            skill.damage = 100;
            skill.range = 2;
            return skill;
        }
        //空技能 占位的
        public static Skill CreateVoid()
        {
            Skill skill = new Skill();
            skill.name = "void";
            return skill;
        }
        //场地火焰攻击
        public static Skill CreateFireGround(int damage, Target caster)
        {
            Skill skill = new Skill();
            skill.name = "场地烈焰";
            skill.type = SkillType.Damage;
            skill.damage = damage;
            skill.caster = caster;
            return skill;
        }

        //最普通的普通攻击
        public static Skill CreateNormalDamage(int damage, Target caster)
        {
            Skill skill = new Skill();
            skill.name = " ";
            skill.type = SkillType.Damage;
            skill.damage = damage;
            skill.caster = caster;
            return skill;
        }

        //最普通的普通攻击
        public static Skill CreateBoss1(int damage, Target caster)
        {
            Skill skill = new Skill();
            skill.name = "泯灭神击";
            skill.type = SkillType.Damage;
            skill.damage = damage;
            skill.caster = caster;
            return skill;
        }

        //最普通的普通攻击
        public static Skill CreateWeaknessHit(int damage, Target caster)
        {
            Skill skill = new Skill();
            skill.name = "弱点破灭";
            skill.type = SkillType.Holy;
            skill.damage = damage;
            skill.caster = caster;
            return skill;
        }

        //场地火焰攻击
        public static Skill CreateBastionRestore(int heal, Target caster)
        {
            Skill skill = new Skill();
            skill.name = "堡垒回复";
            skill.type = SkillType.Heal;
            skill.heal = heal;
            skill.addMana = heal;
            skill.caster = caster;
            return skill;
        }


    }
}

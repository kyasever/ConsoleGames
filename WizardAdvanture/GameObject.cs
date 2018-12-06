using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/* 游戏基础对象
 */
namespace WizardAdvanture
{
    // 以后应该注意 通常==是判断引用,equals是判断是否数字相等.要不就全重载,要不就只重载equal
    // Pos代替Vector2,整数点,自写
    public class Pos
    {
        public int x = 0;
        public int y = 0;
        // 多个初始化函数是为了方便使用
        public Pos()
        {
        }

        public Pos(Pos p)
        {
            x = p.x;
            y = p.y;
        }

        public Pos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int Distanse(Pos p)
        {
            return Math.Abs(p.x - this.x) + Math.Abs(p.y - this.y);
        }


        public override bool Equals(Object o)
        {
            Pos p = (Pos)o;
            return this.x == p.x && y == p.y;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Pos operator -(Pos p1, Pos p2)
        {
            return new Pos(p1.x - p2.x, p1.y - p2.y);
        }

        public static Pos operator +(Pos p1, Pos p2)
        {
            return new Pos(p1.x + p2.x, p1.y + p2.y);
        }

        public static bool operator ==(Pos p1, Pos p2)
        {
            return p1.x == p2.x && p1.y == p2.y;
        }
        public static bool operator !=(Pos p1, Pos p2)
        {
            return !(p1 == p2);
        }


        public override string ToString()
        {
            return x.ToString() + " " + y.ToString();
        }

    }
    //一个方块
    public class Block
    {
        public string name;
        private string pic;
        public string Pic
        {
            get
            {
                if (target != null)
                {
                    return target.pic;
                }
                else
                {
                    return pic;
                }
            }
            set { pic = value; }
        }
        private bool canMove;
        public bool CanMove
        {
            get
            {
                if (target != null)
                {
                    return false;
                }
                else
                {
                    return canMove;
                }
            }
            set { canMove = value; }
        }
        private ConsoleColor backColor;
        public ConsoleColor BackColor
        {
            get
            {
                return backColor;
            }
            set { backColor = value; }
        }
        private ConsoleColor forceColor;
        public ConsoleColor ForceColor
        {
            get
            {
                if (target == null)
                    return forceColor;
                else
                {
                    if (target.isActive)
                    {
                        return target.colorActive;
                    }
                    else
                    {
                        return target.colorNActive;
                    }
                }
            }
            set { forceColor = value; }
        }

        public Target target = null;


        //    位置 
        //:位置贴图(string)
        //:是否可以通过(bool)
        //:是否被实体占位(object)
    }
    //方块工厂,用来返回相应的实例化的方块对象
    public class BlockFactory
    {
        public static Block CreateBlock(char s)
        {
            switch (s)
            {
                case 'X':
                    return CreateWall();
                case 'F':
                    return CreateFire();
                case 'B':
                    return CreateBastion();
                case ' ':
                    return CreateGround();
                default:
                    Block b = CreateGround();
                    b.Pic = s.ToString();
                    b.ForceColor = ConsoleColor.Yellow;
                    return b;
            }

        }
        public static string GetBlockString(Block b)
        {
            switch (b.name)
            {
                case "wall":
                    return "墙壁,不可移动";
                case "ground":
                    return "空地";
                case "fire":
                    return "烈焰之地,危险";
                case "bastion":
                    return "堡垒,加强";
                case "聚能手套":
                    return "聚能手套,强化1技能";
                case "强能手套":
                    return "强能手套,强化2技能";
                case "大盾":
                    return "大盾,被动增加血量";
                case "神杖":
                    return "神杖,增加神杖技能";
                case "door":
                    return "大门,封闭中";
                case "spawn":
                    return "刷怪点,小心";
                default:
                    return " ";
            }
        }

        public static Block CreateWall()
        {
            Block b = new Block
            {
                name = "wall",
                Pic = "■",
                CanMove = false,
                BackColor = ConsoleColor.Black,
                ForceColor = ConsoleColor.White,
                target = null
            };
            return b;
        }

        public static Block CreateSpawn()
        {
            Block b = new Block
            {
                name = "spawn",
                Pic = "井",
                CanMove = true,
                BackColor = ConsoleColor.Black,
                ForceColor = ConsoleColor.White,
                target = null
            };
            return b;
        }

        public static Block CreateGround()
        {
            Block b = new Block
            {
                name = "ground",
                Pic = "  ",
                CanMove = true,
                BackColor = ConsoleColor.Black,
                ForceColor = ConsoleColor.White,
                target = null
            };
            return b;
        }
        public static Block CreateFire()
        {
            Block b = new Block
            {
                name = "fire",
                Pic = "░",
                CanMove = true,
                BackColor = ConsoleColor.Black,
                ForceColor = ConsoleColor.Magenta,
                target = null
            };
            return b;
        }

        public static Block CreateBastion()
        {
            Block b = new Block
            {
                name = "bastion",
                Pic = "▦",
                CanMove = true,
                BackColor = ConsoleColor.Black,
                ForceColor = ConsoleColor.Cyan,
                target = null
            };
            return b;
        }

        public static Block CreateItem()
        {
            Block b = new Block
            {
                name = "item",
                Pic = "  ",
                CanMove = true,
                BackColor = ConsoleColor.Black,
                ForceColor = ConsoleColor.White,
                target = null
            };
            return b;
        }

        public static Block CreateDoor()
        {
            Block b = new Block
            {
                name = "door",
                Pic = "〓",
                CanMove = false,
                BackColor = ConsoleColor.Black,
                ForceColor = ConsoleColor.Red,
                target = null
            };
            return b;
        }

        /* ☆ ♀  〓 █  ▦ ❿ ❾ ❽ ❼ ❻ ❺ ❹ ❸ ❷ ❶
         */

    }
    //用来快速提供输入和输出文件流
    public static class FileController
    {
        private static string rPath = Environment.CurrentDirectory + "\\";
        public static StreamReader GetFileReader(string sPath)
        {
            FileStream fs = new FileStream(rPath + sPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            StreamReader sr = new StreamReader(fs, System.Text.Encoding.UTF8);
            return sr;
        }
        public static StreamWriter GetFileWriter(string sPath)
        {
            FileStream fs = new FileStream(rPath + sPath, System.IO.FileMode.Open, System.IO.FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            return sw;
        }
    }
    //实体对象 有机会应该改写成3个左右的接口 这样继承接口就可以了
    public class Target
    {
        //boss属性
        public bool isWeakness = false;
        //控制分组
        public enum BelongsTo
        {
            PlayerSelf,
            PlayerNet,
            PlayerOther,
            Boss,
            OtherEnemy
        }
        //逻辑检测的分类
        public enum Faction
        {
            Player,
            Friendly,
            Enemy,
        }
        //公有
        public Scene scene;
        //这个对象属于哪个控制器控制
        public BelongsTo belongsTo;
        //这个对象属于哪个阵营是敌是友
        public Faction faction;
        //是否死亡了
        public bool isDead = false;
        //单位的具体数值属性
        public int hp, hpMax;
        public int mp, mpMax;
        public int act, actMax;
        //这个单位的描述文字
        public string discription1 = "这是一条被动描述1";
        public string discription2 = "这是一条被动描述2";
        //暂存,用来表现当前回合还能移动多远,不能超过行动力
        public int moveAct;
        //技能列表
        public List<Skill> skills = new List<Skill>();
        //当前准备释放的技能
        public Skill curretSkill;
        //当前位置
        public Pos location;
        //当前贴图
        public string pic;
        //可以移动的区域
        public List<Pos> canMoveAera;
        //单位名字
        public string name;
        //是否已经持有物品
        public bool hasItem = false;
        //单位颜色
        public ConsoleColor colorActive, colorNActive;
        public bool isActive = true;
        //单位的追踪目标
        public Target focus = null;
        public Target(Scene scene)
        {
            this.scene = scene;
        }
        //单位执行的行动,由不同子类实现
        public virtual void Action()
        {

        }

        public void changeHP(int h)
        {
            hp = hp + h;
            if (hp >= hpMax)
                hp = hpMax;
            if (hp <= 0)
                hp = 0;
        }
        public void changeMP(int m)
        {
            mp = mp + m;
            if (mp >= mpMax)
                mp = mpMax;
            if (mp <= 0)
                mp = 0;
        }
        //最多不超过二倍  不足则不扣除并返回false
        public bool changeAct(int a)
        {
            act = act + a;
            if (name == "愈灵")
            {
                if (act > actMax * 3)
                    act = actMax * 3;
            }
            else
            {
                if (act > actMax * 2)
                    act = actMax * 2;
            }
            if (act < 0)
            {
                act = act - a;
                return false;
            }
            return true;
        }

        //单位死亡执行的行动,清除贴图和状态,后续由子类处理.
        public virtual void Dead()
        {
            scene.SelectBlock(location).target = null;
            isDead = true;
        }
        //被打了执行的检测,后续由子类处理
        public virtual void BeHit(Skill s)
        {
            scene.MoveCursor(location - scene.CursorPos);
            int damage = s.damage;
            //先根据类型检测
            if (s.type == SkillType.Damage || s.type == SkillType.Holy)
            {
                if(s.name == "背刺"|| s.name == "致命伏击")
                {
                    if(hp == hpMax)
                    {
                        damage = damage * 4;
                    }
                }
                if (s.caster.name == "暴雪" || s.name == "神杖-妨碍之击")
                {
                    if (name == "禁地守卫")
                    {
                        actMax = 4;
                        scene.AddDebugMessage("#B" + name + "被减速了");
                    }
                    else if (name == "自爆球")
                    {
                        actMax = 6;
                        scene.AddDebugMessage("#B" + name + "被减速了");
                    }
                    else if (name == "冲击骑士")
                    {
                        actMax = 4;
                        scene.AddDebugMessage("#B" + name + "被剧烈减速了");
                    }
                }

                if(belongsTo == BelongsTo.OtherEnemy)
                {
                    //如果当前目标是坦克单位
                    if(focus.name == "岩山" || focus.name == "圣者")
                    {
                        //如果释放者是坦克单位
                        if(s.caster.name == "岩山" || s.caster.name == "圣者")
                        {
                            //仇恨释放者
                            focus = s.caster;
                        }
                    }
                    //如果当前目标不是坦克单位
                    else
                    {
                        //仇恨释放者
                        focus = s.caster;
                    }
                }

                //岩山具有40%减伤
                if (name == "岩山")
                {
                    if (damage >= 20)
                    {
                        damage = damage *6/10;
                    }
                }
                //boss具有66%减伤
                if (name == "BUG之神")
                {
                    if (damage >= 20)
                    {
                        damage = damage / 3;
                    }
                }
                if(name == "圣者")
                {
                    //如果血量大于1并受到攻击 最多抵挡1000点伤害
                    if(hp!=1&&damage>hp&&damage<hp+1000)
                    {
                        damage = hp - 1;
                        scene.AddDebugMessage("#R圣者触发了拯救者,回复1点HP");
                    }
                }


                int time = 300 / damage;
                if (time < 1)
                {
                    time = 1;
                }
                scene.MoveCursor(new Pos(0, 0));
                for (int xx = 0; xx < damage; xx++)
                {
                    changeHP(-1);
                    //如果是boss则先同步一下血量
                    if (GetType() == typeof(Boss))
                    {
                        Boss b = (Boss)this;
                        b.HpMax = hpMax; b.Hp = hp;
                    }
                    scene.canvas.RefreshUI();
                    Thread.Sleep(time);
                }

                scene.AddDebugMessage(s.name + "对" + this.name + "造成了" + damage.ToString() + "伤害");

                if (hp == 0)
                {
                    Dead();
                    scene.AddDebugMessage(name + "被打死了");
                }

                if (isDead)
                {
                    if (s.caster.name == "火鸟")
                    {
                        scene.AddDebugMessage("#B火鸟由于击杀回复2行动30MP");
                        s.caster.changeAct(2);
                        s.caster.changeMP(30);
                    }
                }
                if (s.name == "狂风刀扇" || s.name == "刀扇")
                {
                    scene.AddDebugMessage("#B刀扇命中回复2行动10MP");
                    s.caster.changeAct(2);
                    s.caster.changeMP(10);
                }

            }
            else if (s.type == SkillType.Heal)
            {
                int heal = s.heal;
                if(s.name == "拯救之光" || s.name == "救赎之光")
                {
                    int percent = hp*100 / hpMax;
                    int reMp = s.costMana * (100 - percent) / 100;
                    heal = s.heal + s.heal * 3 * (100 - percent) / 100;

                    s.caster.changeMP(reMp);
                    scene.AddDebugMessage("#B"+s.name+"消耗了" + (s.costMana - reMp).ToString() + "MP");
                }

                string str = "";
                if (faction == Faction.Enemy)
                {
                    str += "#R";
                }
                else
                {
                    str += "#B";
                }
                str += s.name + "为" + name + "恢复了";
                if (heal > 0)
                {
                    str += heal.ToString() + "生命;";
                }
                if (s.addAct > 0)
                {
                    str += s.addAct.ToString() + "行动;";
                }
                if (s.addMana > 0)
                {
                    str += s.addMana.ToString() + "法力;";
                }
                if (heal > 0)
                {
                    int time = 200 / heal;
                    scene.MoveCursor(new Pos(0, 0));
                    for (int xx = 0; xx < heal; xx++)
                    {
                        changeHP(1);
                        scene.canvas.RefreshUI();
                        Thread.Sleep(time);
                    }
                }

                changeAct(s.addAct);
                changeMP(s.addMana);


                scene.AddDebugMessage(str);
            }
        }
        public virtual bool GetItem(string str)
        {
            if ((belongsTo == BelongsTo.PlayerSelf|| belongsTo == BelongsTo.PlayerNet) && !hasItem)
            {
                switch (str)
                {
                    case "聚能手套":
                        skills[0] = skills[4];
                        break;
                    case "强能手套":
                        skills[1] = skills[5];
                        break;
                    case "大盾":
                        hpMax += 50;
                        changeHP(50);
                        break;
                    case "神杖":
                        skills[2] = Skill.CreateShenZhang(name);
                        skills[2].caster = this;
                        break;
                }
                hasItem = true;
                return true;
            }
            else
            {
                return false;
            }
        }
        //判断这个点是否在BFS搜索中视为可以通过的点
        //不能穿墙,可以穿过队友
        public virtual bool BFSSearchNormal(Pos p)
        {
            try
            {
                bool result =
                scene.blocks[p.x, p.y].CanMove == true ||
                (scene.blocks[p.x, p.y].target != null &&
                 scene.blocks[p.x, p.y].target.faction == faction);
                return result;
            }
            catch
            {
                return false;
            }
        }

        //不能穿墙,可以穿过任何单位
        public virtual bool BFSSearchRoute(Pos p)
        {
            return
                scene.blocks[p.x, p.y].CanMove == true ||
                scene.blocks[p.x, p.y].target != null
                ;
        }
        //除了越界之外都可以
        public virtual bool BFSSearchAll(Pos p)
        {
            return true;
        }

        //指针只能在可移动区域内移动 p为相对向量
        public void MoveCursorInTheAera(Pos p, bool showRoute)
        {
            //scene.MoveCursor(p);
            Pos np = scene.CursorPos + p;
            bool isin = false;
            foreach (var v in canMoveAera)
            {
                if (np == v)
                    isin = true;
            }
            if (isin)
            {
                scene.MoveCursor(p);
                if (showRoute)
                {
                    List<Pos> l = scene.BFS(location, scene.CursorPos, BFSSearchNormal);
                    scene.ShowMoveMessage(this, l.Count - 1);
                    scene.ResetRouteAera(l);
                }
                else
                {
                    scene.ResetRouteAera(new List<Pos>());
                }
                scene.MoveCursor(new Pos(0, 0));
            }
        }



        //显示移动路径
        public bool MoveRoute()
        {
            bool canMove = false;
            foreach (var v in canMoveAera)
            {
                if (scene.CursorPos == v)
                {
                    canMove = true;
                    break;
                }
            }
            scene.BFS(location, scene.CursorPos, BFSSearchNormal);

            return canMove;
        }
        //移动到 to为绝对目标地点
        public void MoveTo(Pos to)
        {
            List<Pos> l = scene.BFS(location, to, BFSSearchNormal);
            int cost = l.Count - 1;
            changeAct(-cost);
            moveAct -= cost;
            scene.SelectBlock(location).target = null;
            if (scene.SelectBlock(to).target == null)
            {
                scene.SelectBlock(to).target = this;
            }
            location = to;
            scene.ResetMoveAera(new List<Pos>());
            scene.ResetRouteAera(new List<Pos>());
            scene.MoveCursor(new Pos(0, 0));
        }

        //准备执行攻击操作 主要是玩家的攻击控制器
        public virtual bool Attack(Pos to, Skill s)
        {
            return false;
        }


        //准备释放技能,与玩家有关
        public virtual bool CostSkill(Skill skill) { return true; }

    }

}

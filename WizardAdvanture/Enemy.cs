using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace WizardAdvanture
{
    //怪物的控制类
    public class EnemyController
    {
        ////C# 4中不可用
        //public int B { get { return 1; } }
        //public int C { get { return 1; } set; }
        //public int D { get; set; }
        //private int e;
        //public int E { get { return e; }set { e = value; } }


        public enum State
        {
            stand,
            follow,
            attack,
        };
        Scene scene;
        //  State state = State.stand;
        public List<Target> lists = new List<Target>();

        public EnemyController(Scene scene)
        {
            this.scene = scene;
        }
        public Enemy CreatGuard(Pos location)
        {
            Enemy s = new Enemy(scene, location);

            s.name = "禁地守卫";
            s.hp = 100; s.hpMax = 100;
            s.mp = 0; s.mpMax = 0;
            s.act = 6; s.actMax = 6;
            s.pic = "♟";

            s.skills.Add(Skill.CreateGuardAttack());
            s.skills[0].caster = s;

            s.aimRange = 2;
            s.nearbyPos = s.SetNearbyPos(s.aimRange);

            lists.Add(s);
            return s;
        }

        public Enemy CreatRook(Pos location)
        {
            Enemy s = new Enemy(scene, location);

            s.name = "重甲骑士";
            s.hp = 300; s.hpMax = 300;
            s.mp = 0; s.mpMax = 0;
            s.act = 4; s.actMax = 4;
            s.pic = "♜";

            s.skills.Add(Skill.CreateRookAttack());
            s.skills[0].caster = s;

            s.aimRange = 2;
            s.nearbyPos = s.SetNearbyPos(s.aimRange);

            lists.Add(s);
            return s;
        }
        //♞
        public Enemy CreatKnight(Pos location)
        {
            Enemy s = new Enemy(scene, location);

            s.name = "冲击骑士";
            s.hp = 100; s.hpMax = 100;
            s.mp = 0; s.mpMax = 0;
            s.act = 10; s.actMax = 10;
            s.pic = "♞";

            s.skills.Add(Skill.CreateGuardAttack());
            s.skills[0].caster = s;
            s.skills.Add(Skill.CreateKnightAttack());
            s.skills[1].caster = s;

            s.aimRange = 0;
            s.nearbyPos = s.SetNearbyPos(s.aimRange);

            lists.Add(s);
            return s;
        }

        public Boom CreateBoom(Pos location)
        {
            Boom b = new Boom(scene, location);
            lists.Add(b);
            return b;
        }

        public void Action()
        {

            //敌人的AI其实是不基于行动力的,是移动+攻击的操作模式
            foreach (var v in lists)
            {
                v.isActive = true;
            }
            for (int i = lists.Count - 1; i > -1; i--)
            {
                try
                {
                    lists[i].Action();
                }
                catch
                {

                }
            }

        }
    }
    //一个普通的怪物
    public class Enemy : Target
    {
        private bool attackFlag = true;
        public int aimRange;

        public List<Pos> route;
        public List<Pos> nearbyPos;

        public Enemy(Scene scene, Pos location) : base(scene)
        {
            colorActive = ConsoleColor.Red;
            colorNActive = ConsoleColor.DarkRed;


            this.location = location;
            faction = Target.Faction.Enemy;
            belongsTo = Target.BelongsTo.OtherEnemy;
            scene.blocks[location.x, location.y].target = this;


            //直接锁定第一个单位 初始化行动力
            int r = scene.random.Next(0, scene.GetPlayers().Count);
            focus = scene.GetPlayers()[r];

        }

        public enum State
        {
            stand,
            move,
            attack,
        };

        public override void BeHit(Skill s)
        {
            base.BeHit(s);
        }

        public override void Dead()
        {
            base.Dead();
            scene.enemyController.lists.Remove(this);
        }

        public List<Pos> SetNearbyPos(int range)
        {
            List<Pos> ll = new List<Pos>();
            switch (range)
            {
                case 1:
                    ll = new List<Pos>(){new Pos(1, 0),new Pos(-1,0),
            new Pos(0,1),new Pos(0,-1) };
                    break;
                case 0:
                    ll = new List<Pos>(){new Pos(1, 0),new Pos(-1,0),
            new Pos(0,1),new Pos(0,-1),new Pos(1, 1),new Pos(-1,1),
            new Pos(1,-1),new Pos(-1,-1) };
                    break;
                case 2:
                    ll = new List<Pos>(){new Pos(1, 0),new Pos(-1,0),
            new Pos(0,1),new Pos(0,-1),new Pos(1, 0),new Pos(-1,0),
            new Pos(0,1),new Pos(0,-1),new Pos(2, 0),new Pos(-2,0),
            new Pos(0,2),new Pos(0,-2)};
                    break;
                default:
                    ll = new List<Pos>();
                    break;
            }
            return ll;
        }


        public Pos FindPosToMove()
        {
            //先取点
            foreach (var v in nearbyPos)
            {
                Block b = scene.SelectBlock(focus.location + v);
                if (b.CanMove)
                {
                    //如果这个点是可以走到的
                    if (scene.BFS(location, focus.location + v, BFSSearchNormal).Count > 1)
                        return focus.location + v;
                }
            }
            return new Pos(0, 0);
        }

        public bool canAttack = false;
        public virtual Pos FindWhereToMove()
        {
            //先寻路
            route = scene.BFS(location, focus.location, BFSSearchNormal);
            //显示寻路路径
            scene.ResetRouteAera(route);
            scene.Show();
            //Thread.Sleep(1000);
            scene.ResetRouteAera(new List<Pos>());
            //int aaaa = route.Count;
            //这是完全搜不到
            if (route.Count == 1)
            {
                route = scene.BFS(location, focus.location, BFSSearchRoute);
                route.RemoveAt(route.Count - 1);
                //如果这次搜索还是找不到
                if (route.Count == 0)
                {
                    return location;
                }
                else
                {
                    foreach (var v in route)
                    {
                        //攻击检测到的第一个敌对单位
                        if (scene.SelectBlock(v).target != null &&
                     scene.SelectBlock(v).target.faction != faction)
                        {
                            focus = scene.SelectBlock(v).target;
                            scene.canvas.RefreshUI();
                            FindWhereToMove();
                        }
                    }
                }
            }
            //这是已经可以攻击了,不用判定了
            if (route.Count < aimRange + 2 && route.Count != 1)
            {
                canAttack = true;
                return location;
            }
            //如果距离上来看能走到,那么试图进入精确定位模式
            if (route.Count <= act + 2 && route.Count != 1)
            {
                //如果能找到可以落脚的点 靠谱的移动路径
                Pos p = FindPosToMove();
                if (p != new Pos(0, 0))
                {
                    canAttack = true;
                    return p;
                }
            }
            //如果精确定位失败,那么执行接近定位策略
            if (act > route.Count - 1)
                act = route.Count - 1;
            for (int i = act; i > 0; i--)
            {
                //检测最远的一个点有没有被遮挡
                Block b = scene.SelectBlock(route[i]);
                //如果最远的点是空的,那么返回移动路线
                if (b.target == null)
                {
                    Pos a = route[i];
                    Pos bb = a;
                    return route[i];
                }
            }
            //真哪都去不了,gg 不动了
            act = 0;
            route = new List<Pos>();
            return location;
        }


        public override void Action()
        {
            canAttack = false;

            act = actMax;

            if (focus.isDead)
            {
                int n = 0;
                if ((n = scene.GetPlayers().Count) > 0)
                {
                    int r = scene.random.Next(0, n);
                    focus = scene.GetPlayers()[r];
                }
                else
                    focus = null;
            }
            //没有仇恨目标 不行动
            if (focus == null)
            {
                return;
            }
            //查找应该走到哪
            Pos to = FindWhereToMove();

            route = scene.BFS(location, to, BFSSearchNormal);

            if (route.Count > 2)
                Thread.Sleep(100);

            MoveTo(route.Last());
            scene.Show();

            if (canAttack)
            {
                if (name == "冲击骑士" && attackFlag)
                {
                    focus.BeHit(skills[1]);
                    attackFlag = false;
                }
                else
                {
                    focus.BeHit(skills[0]);
                }
            }
        }
    }

    public class Boom : Enemy
    {
        public bool isShield = true;
        public Boom(Scene scene, Pos location) : base(scene, location)
        {
            name = "自爆球";
            hp = 50; hpMax = 50;
            mp = 0; mpMax = 0;
            act = 10; actMax = 10;
            pic = "◉";

            aimRange = 0;
            nearbyPos = SetNearbyPos(aimRange);
        }

        public override void BeHit(Skill s)
        {
            //这个免疫治疗技能
            int damage = 0;
            if (s.type == SkillType.Holy)
            {
                damage = s.damage;
                isShield = false;
                pic = "◎";
            }
            //被神杖点过一次则破防
            else if (!isShield)
            {
                damage = s.damage;
            }
            //否则只能受到五分之一的伤害
            else if (s.type == SkillType.Damage)
            {
                damage = s.damage / 5;
            }
            else
            {
                damage = 0;
            }
            Skill sn = Skill.CreateNormalDamage(damage, s.caster);
            sn.name = s.name;
            base.BeHit(sn);
        }

        public override Pos FindWhereToMove()
        {
            //先寻路
            route = scene.BFS(location, focus.location, BFSSearchNormal);
            //显示寻路路径
            scene.ResetRouteAera(route);
            scene.Show();
            scene.ResetRouteAera(new List<Pos>());
            //这是完全搜不到
            if (route.Count == 1)
            {
                route = scene.BFS(location, focus.location, BFSSearchRoute);
                route.RemoveAt(route.Count - 1);
                //如果这次搜索还是找不到
                if (route.Count == 0)
                {
                    return location;
                }
                else
                {
                    foreach (var v in route)
                    {
                        //攻击检测到的第一个敌对单位
                        if (scene.SelectBlock(v).target != null &&
                     scene.SelectBlock(v).target.faction != faction)
                        {
                            focus = scene.SelectBlock(v).target;
                            scene.canvas.RefreshUI();

                            FindWhereToMove();
                        }
                    }
                }
            }

            //路上所有的火都可以增加自己的移动力
            foreach (var v in route)
            {
                if (scene.SelectBlock(v).name == "fire")
                {
                    act++;
                }
            }
            //这是已经可以攻击了,不用判定了
            if (route.Count < aimRange + 2)
            {
                canAttack = true;
                return location;
            }

            //如果距离上来看能走到,那么试图进入精确定位模式
            if (route.Count <= act + 2)
            {
                //如果能找到可以落脚的点 靠谱的移动路径
                Pos p = FindPosToMove();
                if (p != new Pos(0, 0))
                {
                    //如果太远的话只能冲刺到脸上但不能立即攻击
                    if (route.Count > act / 3 + 2)
                    {
                        canAttack = false;
                        return p;
                    }
                    canAttack = true;
                    return p;
                }
            }
            //如果精确定位失败,那么执行接近定位策略
            if (act > route.Count - 1)
                act = route.Count - 1;
            for (int i = act; i > 0; i--)
            {
                //检测最远的一个点有没有被遮挡
                Block b = scene.SelectBlock(route[i]);
                //如果最远的点是空的,那么返回移动路线
                if (b.target == null)
                {
                    Pos a = route[i];
                    Pos bb = a;
                    return route[i];
                }
            }
            //真哪都去不了,gg 不动了
            act = 0;
            route = new List<Pos>();
            return location;
        }


        public override void Action()
        {
            canAttack = false;
            act = actMax;

            if (focus.isDead)
            {
                int n = 0;
                if ((n = scene.GetPlayers().Count) > 0)
                {
                    int r = scene.random.Next(0, n);
                    focus = scene.GetPlayers()[r];
                }
                else
                    focus = null;
            }
            //没有仇恨目标 不行动
            if (focus == null)
            {
                return;
            }

            //查找应在走到哪
            Pos to = FindWhereToMove();

            route = scene.BFS(location, to, BFSSearchNormal);

            if (route.Count > 2)
                Thread.Sleep(100);

            scene.Show();
            MoveTo(route.Last());

            scene.Show();

            if (canAttack)
            {
                scene.MoveCursorTo(location);
                scene.environmentController.ShowMessageBox("自爆球炸了!!!");
                foreach (var v in SetNearbyPos(0))
                {
                    //检测所有爆炸点,如果不为0产生自爆攻击
                    Target t;
                    Skill s;
                    if ((t = scene.SelectBlock(location + v).target) != null)
                    {
                        //对友军造成一半伤害
                        if (t.faction == Faction.Enemy)
                        {
                            s = Skill.CreateBoom();
                            s.caster = this;
                            s.damage = s.damage / 2;
                            t.BeHit(s);
                        }
                        //对岩石造成六倍伤害
                        if (t.faction == Faction.Friendly)
                        {
                            s = Skill.CreateBoom();
                            s.caster = this;
                            s.damage = s.damage * 6;
                            t.BeHit(s);
                        }
                        //对英雄造成一倍伤害
                        if (t.faction == Faction.Player)
                        {
                            s = Skill.CreateBoom();
                            s.caster = this;
                            t.BeHit(s);
                        }
                    }
                }
                Dead();
                isDead = true;
            }

        }

    }

    public class BossCollection
    {
        //boss从33,0 开始刷新 从14列刷到19
        public string[] bossPic = new string[20];
        Scene scene;
        int turns = 0;
        public List<Target> bossList = new List<Target>();
        public Boss bossSelf;
        public int hp = 9999, hpMax = 9999;
        public bool isDead = false;
        public int hugeHandCooldown = 3;
        List<Pos> hugeHandRange = new List<Pos>();

        public int crossFireCooldown = 5;
        List<Pos> crossFireRange = new List<Pos>();
        List<Pos> bornEnemyPoint = new List<Pos>();
        public bool bornFlag = false;
        public int PointContinueTurns = 0;

        public int angry = 0;

        public BossCollection(Scene scene)
        {
            this.scene = scene;
        }

        public void Dead()
        {
            scene.AddDebugMessage("BOSS挂了");
            scene.environmentController.ShowMessageBox("boss在轰鸣声中倒下了");
            isDead = true;
        }

        public void Init()
        {
            //加载boss的贴图
            StreamReader sr = FileController.GetFileReader("Boss.txt");
            for (int i = 0; i < 20; i++)
            {
                bossPic[i] = sr.ReadLine();
            }
            sr.Close();
            //真正的本体
            bossSelf = new Boss(scene, new Pos(33, 0), this);
            //加载boss的模型
            int wx = 33, wy = 0;
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    Boss b = new Boss(scene, new Pos(wx + i, wy + j), this);
                    bossList.Add(b);
                    b.pic = bossPic[14 + j].Substring(2 * i, 2);
                }
            }
            //加载神之掌击的范围 面前4行
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    hugeHandRange.Add(new Pos(wx + i, wy + 6 + j));
                }
            }
            //添加封锁烈焰的施法范围和生成范围
            int sx = 33, sy = 10;
            sr = FileController.GetFileReader("CrossFireRange.txt");
            for (int y = 0; y < 10; y++)
            {
                string str = sr.ReadLine();
                char[] cs = str.ToCharArray();
                for (int x = 0; x < cs.Length; x++)
                {
                    if (cs[x] == 'X')
                    {
                        crossFireRange.Add(new Pos(sx + x, sy + y));
                    }
                    else if (cs[x] == 'O')
                    {
                        bornEnemyPoint.Add(new Pos(sx + x, sy + y));
                    }
                }
            }
            sr.Close();
        }

        public void Action()
        {
            turns++;
            hugeHandCooldown--;
            crossFireCooldown--;
            if (bornFlag)
            {
                PointContinueTurns++;
            }


            //倒计时1释放火焰之地作为提醒
            if (hugeHandCooldown == 1)
            {
                scene.environmentController.ShowMessageBox("泯灭神击!!!!!!!!");
                foreach (var v in hugeHandRange)
                {
                    Target t = scene.blocks[v.x, v.y].target;
                    scene.blocks[v.x, v.y] = BlockFactory.CreateFire();
                    scene.blocks[v.x, v.y].target = t;
                }
                scene.Show();
            }
            //如果神之掌击技能冷却完毕
            if (hugeHandCooldown == 0)
            {
                List<Target> attackListPlayer = new List<Target>();
                List<Target> attackListEnemy = new List<Target>();
                List<Target> attackListOther = new List<Target>();
                //获取攻击发起列表,去除指示器
                foreach (var v in hugeHandRange)
                {
                    Target t = scene.blocks[v.x, v.y].target;
                    if (t != null)
                    {
                        //将t加入list进行下一步处理 攻击分类结算等等等
                        if (t.faction == Target.Faction.Enemy)
                        {
                            attackListEnemy.Add(t);
                        }
                        else if (t.faction == Target.Faction.Player)
                        {
                            attackListPlayer.Add(t);
                        }
                        else if (t.faction == Target.Faction.Friendly)
                        {
                            attackListOther.Add(t);
                        }
                    }
                    scene.blocks[v.x, v.y] = BlockFactory.CreateGround();
                    scene.blocks[v.x, v.y].target = t;
                }

                int sum = attackListEnemy.Count + attackListOther.Count + attackListPlayer.Count;
                if (sum == 0)
                {
                    sum = 1;
                }
                int attack = 0;
                if (scene.difficult == Scene.Difficult.easy)
                {
                    attack = 400 + angry * 50;
                }
                else if (scene.difficult == Scene.Difficult.hard)
                {
                    attack = 600 + angry * 100;
                }
                //每个敌人受到的伤害
                int perAttack = attack / sum;
                //用来释放的技能
                Skill s = Skill.CreateBoss1(perAttack, bossSelf);
                //它可能是被从控制器中移除了,单位应该是不会被销毁的
                //int before = attackListEnemy.Count;
                //for (int i = attackListEnemy.Count-1;i>-1;--i)
                //{
                //    attackListEnemy[i].BeHit(s);
                //}
                foreach (var v in attackListEnemy)
                {
                    v.BeHit(s);
                }
                //统计打死了几个敌人
                int c = 0;
                foreach (var v in attackListEnemy)
                {
                    if (v.isDead)
                    {
                        c++;
                    }
                }
                //无论如何都要加一点愤怒,为了能够更快的刷新弱点攻击
                angry++;
                angry += c;
                //石头直接打就行
                foreach (var v in attackListOther)
                {
                    v.BeHit(s);
                }
                if (attackListPlayer.Count > 0)
                {
                    foreach (var v in attackListPlayer)
                    {
                        v.BeHit(s);
                    }
                }
                //如果没有命中目标
                else
                {
                    //如果还有敌人可以打... 由于均摊数量太少,/2降低每次伤害
                    if (scene.playerController.lists.Count > 0)
                    {
                        perAttack = attack / scene.playerController.lists.Count / 2;
                        //用来释放的技能
                        s = Skill.CreateBoss1(perAttack, bossSelf);

                        List<Target> lists = scene.GetPlayers();
                        //处理交给第三方行动的单位
                        for (int i = lists.Count - 1; i > -1; i--)
                        {
                            lists[i].BeHit(s);
                        }
                    }
                }
                scene.AddDebugMessage("#R泯灭神击一共造成了" + attack.ToString() + "点伤害");
                scene.Show();
                //如果里面有己方英雄,对攻击范围内造成400+50*愤怒值的伤害 由所有单位均摊(不分敌友) 每杀死一个敌人会+1愤怒值
                //如果里面没有己方英雄,追加一次神之掌击 对全屏造成400+5*愤怒值的伤害 由己方所有英雄均摊.
                hugeHandCooldown = 3;

                int count = 0;
                if (scene.difficult == Scene.Difficult.easy)
                {
                    count = 1 + angry / 3;
                }
                if (scene.difficult == Scene.Difficult.hard)
                {
                    count = 0 + angry / 5;
                }
                for (int i = 0; i < count; i++)
                {
                    int r = scene.random.Next(0, bossList.Count - 1);
                    bossList[r].isWeakness = true;
                    bossList[r].colorActive = ConsoleColor.Red;
                    bossList[r].pic = "■";
                    bossList[r].name = "弱点";
                }

                scene.AddDebugMessage("震颤过后,boss露出了弱点,可以使用神杖攻击!");
            }

            if (crossFireCooldown == 3)
            {
                scene.environmentController.ShowMessageBox("封锁烈焰!!!!!!!!!!");
                foreach (var v in crossFireRange)
                {
                    Target t = scene.blocks[v.x, v.y].target;
                    scene.blocks[v.x, v.y] = BlockFactory.CreateFire();
                    scene.blocks[v.x, v.y].target = t;
                }
                scene.environmentController.ShowMessageBox("注意躲开脚下的火墙,会造成致命伤害");
            }

            if (crossFireCooldown == 1)
            {
                scene.environmentController.ShowMessageBox("永远禁锢在火焰之中吧!");
                foreach (var v in crossFireRange)
                {
                    Target t = scene.blocks[v.x, v.y].target;
                    if (t != null)
                    {
                        t.BeHit(Skill.CreateBoss1(1000, bossSelf));
                    }
                    scene.blocks[v.x, v.y] = BlockFactory.CreateDoor();
                }
            }
            if (crossFireCooldown == 0)
            {
                bornFlag = true;
            }

            if (bornFlag)
            {
                int during = 4;
                if (scene.difficult == Scene.Difficult.hard)
                {
                    during = 8;
                }
                //普通持续4个回合 困难持续8个回合
                if (PointContinueTurns == during)
                {
                    scene.environmentController.ShowMessageBox("场地清除");
                    for (int y = 0; y < 10; y++)
                    {
                        for (int x = 0; x < 20; x++)
                        {
                            Target t = scene.blocks[33 + x, 10 + y].target;
                            scene.blocks[33 + x, 10 + y] = BlockFactory.CreateGround();
                            scene.blocks[33 + x, 10 + y].target = t;
                        }
                    }
                    bornFlag = false;
                }
                else
                {
                    List<Pos> fireAera = new List<Pos>();
                    foreach (var v in bornEnemyPoint)
                    {
                        //搜索这个点可以移动的区域
                        //将三个点的所有扩展区域加入aera
                        fireAera.AddRange(scene.ShowAera(v, PointContinueTurns, bossSelf.BFSSearchRoute));
                    }
                    //将所有可移动区域创建为火焰之地
                    foreach (var v in fireAera)
                    {
                        Target t = scene.blocks[v.x, v.y].target;
                        scene.blocks[v.x, v.y] = BlockFactory.CreateFire();
                        scene.blocks[v.x, v.y].target = t;
                        //如果火焰之地的点上没有怪则按照几率刷新怪物
                        if (scene.blocks[v.x, v.y].target == null)
                        {
                            int r = scene.random.Next(1, 100);
                            if (r < 2)
                            {
                                scene.enemyController.CreatKnight(v);
                            }
                            else if (r < 4)
                            {
                                scene.enemyController.CreateBoom(v);
                            }
                            else if (r < 10)
                            {
                                scene.enemyController.CreatGuard(v);
                            }

                        }
                    }
                }
            }

            //    public int hugeHandCooldown = 3;
            //public int crossFireCooldown = 5;
            //public int PointContinueTurns = 0;
            scene.AddDebugMessage("boss技能冷却:神击" + hugeHandCooldown.ToString() + "火墙" + crossFireCooldown.ToString()
                + "刷怪" + PointContinueTurns.ToString() + "愤怒" + angry.ToString());
            /* 简单难度 极大增加封锁烈焰的冷却,只有开始时放一轮. 极大降低泯灭神击的愤怒值加成,将怪都拉进神击中击杀
             * 封锁烈焰 第一个回合按照迷宫形状遍历txt 按照形状创建灼热之地,第二个回合按照形状先对目标的地点进行神之击秒杀站在上面的目标,然后造墙(红门贴图).
               第三个回合选取三个刷怪地点开始铺灼热地面并开始刷三种小怪.持续刷小怪,刷一轮大怪,隔一定时间刷炸弹.
             */

        }
    }

    public class Boss : Target
    {
        BossCollection bossCollection;

        //不能重写父类变量么....
        public int Hp
        {
            get { return bossCollection.hp; }
            set { bossCollection.hp = value; }
        }
        public int HpMax
        {
            get { return bossCollection.hpMax; }
            set { bossCollection.hpMax = value; }
        }
        public override void Dead()
        {
            bossCollection.Dead();
        }


        //这其实只是boss的一个零件,不需要主动行动,只要挨打就行了,挨打掉的是本体的血.允许被AOE伤害
        //target分为弱点和普通两种部件.普通部件只会受到1/4伤害(aoe损失) 
        //弱点会受到全额单体伤害,弱点被神杖点了会被点爆,并造成500点直接伤害.
        public Boss(Scene scene, Pos location, BossCollection bossCollection) : base(scene)
        {
            this.bossCollection = bossCollection;
            hp = Hp; hpMax = HpMax;
            faction = Target.Faction.Enemy;
            //自定义pic 颜色 hp获取母类的hp 只有behit 没有攻击 外部定义location
            name = "BUG之神";
            colorActive = ConsoleColor.White;

            belongsTo = Target.BelongsTo.Boss;

            this.location = location;
            scene.blocks[location.x, location.y].target = this;
            //好像没别的属性了
        }

        public override void BeHit(Skill s)
        {
            if (isWeakness && s.type == SkillType.Holy)
            {
                //先受到普通伤害
                hp = Hp; hpMax = HpMax;
                base.BeHit(s);

                base.BeHit(Skill.CreateWeaknessHit(300, bossCollection.bossSelf));
                scene.AddDebugMessage("#R弱点被击破,受到额外伤害");

                Block b = BlockFactory.CreateFire();
                b.CanMove = false;
                scene.blocks[location.x, location.y] = b;

                Dead();
            }
            else
            {
                hp = Hp; hpMax = HpMax;
                base.BeHit(s);
            }

        }
    }

}

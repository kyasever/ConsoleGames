using System;
using System.Collections.Generic;
using System.Threading;

namespace WizardAdvanture
{
    public class EnvironmentController
    {
        public enum State
        {
            begin,
            level1,
            level2,
        }
        public State state = State.level2;

        Scene scene;
        public BossCollection bossCollection;
        Target world;
        public List<Target> lists = new List<Target>();

        public List<Pos> spawnEnemyPos = new List<Pos>();

        private List<Pos> ItemPos = new List<Pos>()
        {new Pos(2, 4),new Pos(4, 4),new Pos(6, 4),new Pos(8, 4),
            new Pos(4, 6),new Pos(6, 6),};

        private List<Pos> MiutiModeItemPos = new List<Pos>()
        {new Pos(7, 3),new Pos(7, 20),new Pos(9, 3),new Pos(9, 20),
            new Pos(7, 5),new Pos(7, 18),};

        public EnvironmentController(Scene scene)
        {
            this.scene = scene;
            world = new Target(scene);
            world.name = "神之手";

            bossCollection = new BossCollection(scene);
            bossCollection.Init();

            ItemPos = new List<Pos>();

            if (scene.gameMode == Scene.GameMode.miuti)
            {
                ItemPos = MiutiModeItemPos;
            }

            //固定6个刷怪点 困难会极大增加刷新速度
            spawnEnemyPos.Add(new Pos(1, 9));
            spawnEnemyPos.Add(new Pos(1, 13));
            spawnEnemyPos.Add(new Pos(12, 1));
            spawnEnemyPos.Add(new Pos(12, 22));
            spawnEnemyPos.Add(new Pos(31, 17));
            spawnEnemyPos.Add(new Pos(31, 18));

            foreach (var v in spawnEnemyPos)
            {
                Block bb = BlockFactory.CreateSpawn();
                scene.blocks[v.x, v.y] = bb;
            }

            //Block b;
            //b = BlockFactory.CreateItem();
            //b.name = "聚能手套";
            //b.Pic = "➀";
            //scene.blocks[ItemPos[0].x, ItemPos[0].y] = b;

            //b = BlockFactory.CreateItem();
            //b.name = "聚能手套";
            //b.Pic = "➀";
            //scene.blocks[ItemPos[1].x, ItemPos[1].y] = b;

            //b = BlockFactory.CreateItem();
            //b.name = "强能手套";
            //b.Pic = "➁";
            //scene.blocks[ItemPos[2].x, ItemPos[2].y] = b;

            //b = BlockFactory.CreateItem();
            //b.name = "强能手套";
            //b.Pic = "➁";
            //scene.blocks[ItemPos[3].x, ItemPos[3].y] = b;

            //b = BlockFactory.CreateItem();
            //b.name = "神杖";
            //b.Pic = "⛏";
            //scene.blocks[ItemPos[4].x, ItemPos[4].y] = b;

            //b = BlockFactory.CreateItem();
            //b.name = "神杖";
            //b.Pic = "⛏";
            //scene.blocks[ItemPos[5].x, ItemPos[5].y] = b;

            //后两个门的位置
            scene.blocks[32, 17] = BlockFactory.CreateDoor();
            scene.blocks[32, 18] = BlockFactory.CreateDoor();

        }

        private void DeleteItem(int x, int y, string str)
        {
            Target t = scene.blocks[x, y].target;
            Block b = BlockFactory.CreateGround();
            b.target = t;
            scene.blocks[x, y] = b;
            scene.AddDebugMessage(t.name + "捡起了" + str);
        }

        public int consoleSpeed = 10;
        public ConsoleColor consoleColor = ConsoleColor.White;
        public void ShowMessageBox(string str)
        {
            int sl = 0;
            foreach (var v in str.ToCharArray())
            {
                if (scene.canvas.IsChinese(v))
                    sl += 2;
                else
                    sl += 1;
            }
            sl += 2;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, 22);
            Console.Write("┌");
            for (int i = 0; i < sl; i++)
            {
                Console.Write("─");
            }
            Console.Write("┐");
            Console.SetCursorPosition(0, 23);
            Console.Write("│");
            for (int i = 0; i < sl; i++)
            {
                Console.Write(" ");
            }
            Console.Write("│");
            Console.SetCursorPosition(0, 24);
            Console.Write("└");
            for (int i = 0; i < sl; i++)
            {
                Console.Write("─");
            }
            Console.Write("┘");

            Console.SetCursorPosition(1, 23);

            Console.ForegroundColor = consoleColor;

            foreach (var v in str)
            {
                Console.Write(v);
                if (consoleSpeed > 0)
                    Thread.Sleep(consoleSpeed);
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(" ⏳");

            //ConsoleKeyInfo ck;
            //while ((ck = Console.ReadKey()).Key != ConsoleKey.Enter)
            Console.ReadKey();

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, 22);
            for (int i = 0; i < sl + 2; i++)
            {
                Console.Write(" ");
            }
            Console.SetCursorPosition(0, 23);
            for (int i = 0; i < sl + 2; i++)
            {
                Console.Write(" ");
            }
            Console.SetCursorPosition(0, 24);
            for (int i = 0; i < sl + 2; i++)
            {
                Console.Write(" ");
            }

        }
        //第一关是否开始了
        public int levelTurns = 0;
        public int levelContinuance = 10;



        public void Init()
        {
            if (scene.difficult == Scene.Difficult.easy)
            {
                levelContinuance = 10;
            }
            if (scene.difficult == Scene.Difficult.hard)
            {
                levelContinuance = 20;
            }

        }

        public void Action()
        {
            //本地计时器,如果符合条件就清0
            levelTurns++;
            //直接反应在这测试
            scene.turns = levelTurns;

            //执行场地效果结算
            foreach (var v in scene.blocks)
            {
                if (v.name == "fire")
                {
                    if (v.target != null)
                    {
                        if (v.target.faction == Target.Faction.Player)
                            v.target.BeHit(Skill.CreateFireGround(20, world));
                    }
                }
                else if (v.name == "bastion")
                {
                    if (v.target != null)
                    {
                        v.target.BeHit(Skill.CreateBastionRestore(20, world));
                    }
                }
            }
            foreach (var v in ItemPos)
            {
                Block b = scene.SelectBlock(v);
                if (b.target != null)
                {
                    if (b.target.GetItem(b.name))
                    {
                        DeleteItem(v.x, v.y, b.name);
                    }
                }
            }

            //处理交给第三方行动的单位
            for (int i = lists.Count - 1; i > -1; i--)
            {
                lists[i].Action();
            }
            //第一个回合 
            if (scene.turns == 1 && state == State.begin)
            {
                Init();

                scene.Show();
                if (scene.difficult == Scene.Difficult.easy)
                {
                    ShowMessageBox("欢迎来到战棋冒险的世界 [Enter]继续对话");
                    ShowMessageBox("操作说明:[q]取消 [空格]确认 [方向键]移动光标 [esc]结束当前回合 [1-4]释放技能");

                    scene.MoveCursor(new Pos(6, 6));
                    scene.Show();
                    ShowMessageBox("神杖是通关所需的关键装备,使用神杖的攻击可以击破特殊怪物的防御甲壳,不同角色携带神杖可以获得不同的神杖技能");
                }
                else
                {
                    ShowMessageBox("欢迎来到战棋冒险的世界 [Enter]继续对话");
                    ShowMessageBox("这里是丧心病狂的困难难度,为了帮助到达这里的勇士通关,下面是一些有助于通关的小tips");
                    ShowMessageBox("1.岩和寒的2技能造墙可以隔离敌人的视野,但是适当的留缝隙和反复堵迷宫可以改变敌人的行动路线,效果比堵死要好");
                }
                ShowMessageBox("停在道具上可以在第二回合捡起道具,准备开始冒险吧!");
                scene.AddDebugMessage("#C停在道具上可以在第二回合捡起道具");
                scene.AddDebugMessage("#C捡起所有的道具,准备开始冒险吧!");

                state = State.level1;
            }
            //是否开始了第一关
            else if (state == State.level1)
            {
                scene.AddDebugMessage("剩余" + (levelContinuance - levelTurns).ToString() + "回合");
                if (levelTurns == 2)
                {
                    ShowMessageBox("♟卫士,普通敌人♞冲击骑士,第一次攻击造成巨量单体伤害");
                }

                //每两个回合创造一次
                if (levelTurns < 20 && levelTurns > 3 && levelTurns % 2 == 0)
                {
                    foreach (var v in spawnEnemyPos)
                    {
                        if (scene.SelectBlock(v).target == null)
                        {
                            int r = scene.random.Next(0, 99);
                            if (r < 20)
                            {
                                scene.enemyController.CreatKnight(v);
                            }
                            else
                                scene.enemyController.CreatGuard(v);
                        }
                        else if (scene.SelectBlock(v).target.faction != Target.Faction.Enemy)
                        {
                            scene.SelectBlock(v).target.BeHit(Skill.CreateNormalDamage(100, world));
                        }
                    }
                }
                if (levelTurns == 5)
                {
                    ShowMessageBox("◉自爆球,贴近产生AOE自爆攻击.被神杖攻击过会破壳变成◎,此时可以正常受到伤害");
                }

                if (levelTurns == 5 || levelTurns == 9 || levelTurns == 13 || levelTurns == 17)
                {
                    foreach (var v in spawnEnemyPos)
                    {
                        if (scene.SelectBlock(v).target == null)
                        {
                            scene.enemyController.CreateBoom(v);
                        }
                        else if (scene.SelectBlock(v).target.faction != Target.Faction.Enemy)
                        {
                            scene.SelectBlock(v).target.BeHit(Skill.CreateNormalDamage(200, world));
                        }
                    }
                }

                if (levelTurns == 7)
                {
                    ShowMessageBox("♜重甲骑士,具有较高的生命值和攻击力");
                }

                //疯狂刷新骑士
                if (levelTurns == 7 || levelTurns == 11 || levelTurns == 15 || levelTurns == 19)
                {
                    foreach (var v in spawnEnemyPos)
                    {
                        if (scene.SelectBlock(v).target == null)
                        {
                            scene.enemyController.CreatRook(v);
                        }
                        else if (scene.SelectBlock(v).target.faction != Target.Faction.Enemy)
                        {
                            scene.SelectBlock(v).target.BeHit(Skill.CreateNormalDamage(200, world));
                        }
                        else
                        {
                            scene.SelectBlock(v).target.Dead();
                            scene.enemyController.CreatRook(v);
                        }
                    }
                }

                //该结束了
                if (levelTurns == levelContinuance)
                {
                    //打开后两个门
                    scene.blocks[32, 17] = BlockFactory.CreateGround();
                    scene.blocks[32, 18] = BlockFactory.CreateGround();

                    scene.MoveCursor(new Pos(32, 17) - scene.CursorPos);
                    ShowMessageBox("大门已经打开,请加油通过吧!");
                }

                if (levelTurns > levelContinuance)
                {
                    bool isAllRight = true;
                    foreach (var v in scene.playerController.lists)
                    {
                        if (v.location.x < 33)
                        {
                            isAllRight = false;
                        }
                    }
                    //如果全部通过了
                    if (isAllRight)
                    {
                        ShowMessageBox("已经通过,正在封锁通道");
                        for (int i = scene.enemyController.lists.Count - 1; i > -1; i--)
                        {
                            if (scene.enemyController.lists[i].location.x < 33)
                            {
                                scene.MoveCursorTo(scene.enemyController.lists[i].location);
                                scene.Show();
                                Thread.Sleep(300);
                                scene.enemyController.lists[i].BeHit(Skill.CreateNormalDamage(1000, world));
                            }
                        }
                        ShowMessageBox("已经成功执行清扫");

                        Console.Clear();
                        //后两个门的位置
                        scene.blocks[32, 17] = BlockFactory.CreateDoor();
                        scene.blocks[32, 18] = BlockFactory.CreateDoor();
                        levelTurns = 0;
                        state = State.level2;
                    }
                }
            }
            //是否开始了第二关
            else if (state == State.level2)
            {
                if (levelTurns == 1)
                {
                    //片头动画

                    ShowMessageBox("~~~~~哇嘎嘎嘎嘎嘎嘎~~~~~~");
                    Console.Clear();
                    ShowBoss0();
                    ShowMessageBox("BUG之神:哇哈哈哈哈~来互相伤害啊");
                    BossAnimate();
                    ShowMessageBox("BUG之神:来啊,一起快活啊~嘎嘎嘎嘎嘎");
                    BossAnimate();
                    ShowBoss0();
                    ShowMessageBox("加油吧 骚年");

                    Console.Clear();
                    scene.canvas.InitUI();
                    scene.canvas.Refresh();
                    //直接把英雄传送过去
                    List<Pos> pl = new List<Pos>() { new Pos(38, 10), new Pos(40, 10), new Pos(42, 10), new Pos(44, 10), new Pos(46, 10) };
                    for (int i = 0; i < scene.playerController.lists.Count; i++)
                    {
                        scene.playerController.lists[i].MoveTo(pl[i]);
                        scene.playerController.lists[i].changeHP(100);
                        scene.playerController.lists[i].changeMP(100);
                        scene.playerController.lists[i].changeAct(10);
                    }
                    scene.MoveCursorTo(new Pos(48, 6));
                    scene.Show();
                    ShowMessageBox("准备消灭BUG之神吧,BUG之神的任何部位都可以被攻击,其中红色的部位是弱点,用神杖点爆可以造成额外伤害");

                    foreach (var v in scene.GetPlayers())
                    {
                        v.GetItem("神杖");
                    }
                }
                else
                {
                    //之后每回合调用这个来进行boss的操作.
                    bossCollection.Action();
                }

            }

            void BossAnimate()
            {
                for (int i = 0; i < 5; i++)
                {
                    ShowBoss0();
                    Thread.Sleep(100);
                    ShowBoss2();
                    Thread.Sleep(100);
                    ShowBoss1();
                    Thread.Sleep(100);
                    ShowBoss2();
                    Thread.Sleep(100);
                }

                void ShowBoss1()
                {
                    int x = 2, y = 1;
                    foreach (var v in bossCollection.bossPic)
                    {
                        Console.SetCursorPosition(x, y);
                        y++;
                        string s = " " + v;
                        Console.WriteLine(s);
                    }
                }
                void ShowBoss2()
                {
                    int x = 2, y = 1;
                    foreach (var v in bossCollection.bossPic)
                    {
                        string s = "";
                        int a = scene.random.Next(0, 100);
                        if (a < 30)
                        {
                            s = " " + v;
                        }
                        else if (a > 30 && a < 50)
                        {
                            s = "  " + v;
                        }
                        else
                        {
                            s = v;
                        }
                        Console.SetCursorPosition(x, y);
                        y++;

                        Console.WriteLine(s);
                    }
                }
            }
            void ShowBoss0()
            {
                int x = 2, y = 1;
                foreach (var v in bossCollection.bossPic)
                {
                    Console.SetCursorPosition(x, y);
                    y++;
                    Console.WriteLine(v);
                }
            }


        }

    }
}



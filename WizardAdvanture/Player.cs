using System;
using System.Collections.Generic;

namespace WizardAdvanture
{
    public class PlayerController
    {
        public List<Pos> spawnPlayerPos = new List<Pos>()
        {new Pos(7, 7),new Pos(5, 7),new Pos(5, 8),new Pos(7, 8),new Pos(7, 9)};

        public List<Pos> spawnP1Pos = new List<Pos>()
        {new Pos(5, 7),new Pos(7, 7),new Pos(9, 7)};

        public List<Pos> spawnP2Pos = new List<Pos>()
        {new Pos(5, 16),new Pos(7, 16),new Pos(9, 16)};


        Scene scene;
        State state = State.stand;
        public Target currertTarget;
        public List<Target> lists = new List<Target>();


        public string SelectPlayer(string s, int n)
        {
            string str = "";
            if (int.TryParse(s, out int a))
            {
                int[] intA = new int[128];
                char[] cA = s.ToCharArray();
                foreach (var v in cA)
                {
                    if (intA[v] > 0)
                    {
                        continue;
                    }
                    else
                    {
                        str += v;
                        intA[v]++;
                    }
                }
            }
            else
            {
                return "f";
            }
            a = Convert.ToInt32(str);
            Console.WriteLine("有效选择:" + str);
            //数字位数正确才能说明传入参数合理
            if (a > Math.Pow(10, n - 1) && a < Math.Pow(10, n))
            {
                char[] charA = str.ToCharArray();
                int i = 0;
                foreach (var v in charA)
                {
                    Console.WriteLine("this" + v.ToString());
                    if (v == '1')
                    {
                        lists.Add(CreatStoneGiant(spawnPlayerPos[i]));
                    }
                    if (v == '2')
                    {
                        lists.Add(CreatPaladin(spawnPlayerPos[i]));
                    }
                    if (v == '3')
                    {
                        lists.Add(CreatFireWizard(spawnPlayerPos[i]));
                    }
                    if (v == '4')
                    {
                        lists.Add(CreatAssassin(spawnPlayerPos[i]));
                    }
                    if (v == '5')
                    {
                        lists.Add(CreatIceWizard(spawnPlayerPos[i]));
                    }
                    if (v == '6')
                    {
                        lists.Add(CreatHealer(spawnPlayerPos[i]));
                    }
                    i++;
                }
                return str;
            }
            else
            {
                return "f";
            }

        }


        //创建的时候初始化 包含这些单位
        public PlayerController(Scene scene)
        {
            this.scene = scene;

            if (scene.gameMode == Scene.GameMode.miuti)
            {
                if (scene.netPlayer == Scene.NetPlayer.P1)
                {
                    spawnPlayerPos = spawnP1Pos;
                }
                if (scene.netPlayer == Scene.NetPlayer.P2)
                {
                    spawnPlayerPos = spawnP2Pos;
                }
            }
        }

        public enum State
        {
            stand,
            move,
            aim,
            stop,
        };
        public bool Action()
        {
            state = State.stand;
            //己方控制的回合开始了
            //每回合直接增加行动力数值 最多可以积攒两倍
            if (lists.Count == 0)
            {
                return false;
            }

            foreach (var v in lists)
            {
                v.isActive = true;
                v.changeAct(v.actMax);
                v.moveAct = v.actMax;
            }
            //回合开始时光标转回
            scene.MoveCursorTo(lists[0].location);


            while (true)
            {
                Console.SetCursorPosition(5, 25);
                ConsoleKey ck = ConsoleKey.Spacebar;

                if (state != State.stop)
                {
                    ck = Console.ReadKey().Key;
                    if (scene.gameMode == Scene.GameMode.miuti)
                    {
                        //当读取到一个按键之后原封不动的把这个按键顺着网络发出去....
                        var writer = MyTCPClient.Instance.GetSendStream(GameAction.Game, GameCmd.Action);
                        writer.Write((byte)ck);
                        MyTCPClient.Instance.Send(writer);
                    }
                }


                if (ck == ConsoleKey.End)
                {
                    scene.AddDebugMessage("debug");
                    scene.debugMode = !scene.debugMode;
                }

                //站立状态
                if (state == State.stand)
                {

                    if (ck == ConsoleKey.LeftArrow)
                    {
                        scene.MoveCursor(new Pos(-1, 0));
                    }
                    else if (ck == ConsoleKey.RightArrow)
                    {
                        scene.MoveCursor(new Pos(1, 0));
                    }
                    else if (ck == ConsoleKey.UpArrow)
                    {
                        scene.MoveCursor(new Pos(0, -1));
                    }
                    else if (ck == ConsoleKey.DownArrow)
                    {
                        scene.MoveCursor(new Pos(0, 1));
                    }
                    else if (ck == ConsoleKey.D1)
                    {
                        if ((currertTarget = scene.SelectBlock(scene.CursorPos).target) != null)
                        {
                            if (currertTarget.belongsTo == Target.BelongsTo.PlayerSelf && currertTarget.isActive)
                            {
                                if (currertTarget.CostSkill(currertTarget.skills[0]))
                                {
                                    state = State.aim;
                                }
                            }
                        }

                    }
                    else if (ck == ConsoleKey.D2)
                    {
                        if ((currertTarget = scene.SelectBlock(scene.CursorPos).target) != null)
                        {
                            if (currertTarget.belongsTo == Target.BelongsTo.PlayerSelf && currertTarget.isActive)
                            {
                                if (currertTarget.CostSkill(currertTarget.skills[1]))
                                {
                                    state = State.aim;
                                }
                            }
                        }
                    }
                    else if (ck == ConsoleKey.D3)
                    {

                        if ((currertTarget = scene.SelectBlock(scene.CursorPos).target) != null)
                        {
                            if (currertTarget.belongsTo == Target.BelongsTo.PlayerSelf && currertTarget.isActive)
                            {
                                if (currertTarget.skills[2].name != "void")
                                {
                                    if (currertTarget.CostSkill(currertTarget.skills[2]))
                                    {
                                        state = State.aim;
                                    }
                                }
                            }
                        }

                    }
                    else if (ck == ConsoleKey.D4)
                    {
                        if ((currertTarget = scene.SelectBlock(scene.CursorPos).target) != null)
                        {
                            if (currertTarget.belongsTo == Target.BelongsTo.PlayerSelf && currertTarget.isActive)
                            {
                                currertTarget.Attack(currertTarget.location, currertTarget.skills[3]);
                                currertTarget.isActive = false;
                            }
                        }
                    }
                    //果然是底下没加else if
                    else if (ck == ConsoleKey.Spacebar)
                    {
                        if ((currertTarget = scene.SelectBlock(scene.CursorPos).target) != null)
                        {
                            if (currertTarget.isActive && currertTarget.belongsTo == Target.BelongsTo.PlayerSelf)
                            {
                                if (currertTarget.moveAct > 0)
                                {
                                    currertTarget.canMoveAera = scene.ShowAera(currertTarget.location, currertTarget.moveAct, currertTarget.BFSSearchNormal);
                                    scene.ResetMoveAera(currertTarget.canMoveAera);
                                    state = State.move;
                                    scene.ReloadCursor(Scene.CursorType.Point);
                                }
                            }
                        }
                        //如果点中了空地
                        else
                        {
                            Target t = scene.playerController.lists[0];
                            foreach (var v in scene.GetPlayers())
                            {
                                if (v.moveAct > t.moveAct)
                                {
                                    t = v;
                                }
                            }
                            scene.MoveCursorTo(t.location);
                        }
                    }
                    else if (ck == ConsoleKey.Escape)
                    {
                        state = State.stop;
                    }
                }
                else if (state == State.move)
                {

                    if (ck == ConsoleKey.Q)
                    {
                        scene.ResetMoveAera(new List<Pos>());
                        scene.ReloadCursor(Scene.CursorType.Point);
                        state = State.stand;
                    }
                    else if (ck == ConsoleKey.LeftArrow)
                    {
                        currertTarget.MoveCursorInTheAera(new Pos(-1, 0), true);
                    }
                    else if (ck == ConsoleKey.RightArrow)
                    {
                        currertTarget.MoveCursorInTheAera(new Pos(1, 0), true);
                    }
                    else if (ck == ConsoleKey.UpArrow)
                    {
                        currertTarget.MoveCursorInTheAera(new Pos(0, -1), true);
                    }
                    else if (ck == ConsoleKey.DownArrow)
                    {
                        currertTarget.MoveCursorInTheAera(new Pos(0, 1), true);
                    }
                    else if (ck == ConsoleKey.Spacebar)
                    {
                        if (currertTarget.MoveRoute())
                        {
                            state = State.stand;
                            if (scene.SelectBlock(scene.CursorPos).target == null)
                            {
                                currertTarget.MoveTo(scene.CursorPos);
                                if (currertTarget.act == 0)
                                    currertTarget.isActive = false;
                            }
                            else
                            {
                                currertTarget.MoveTo(currertTarget.location);
                            }
                        }
                    }
                }
                else if (state == State.aim)
                {
                    scene.ShowSkillMessage(currertTarget.curretSkill);

                    if (ck == ConsoleKey.Q)
                    {
                        scene.ResetMoveAera(new List<Pos>());
                        scene.ReloadCursor(Scene.CursorType.Point);
                        state = State.stand;
                    }
                    else if (ck == ConsoleKey.LeftArrow)
                    {
                        currertTarget.MoveCursorInTheAera(new Pos(-1, 0), false);
                        SetIceWallCursor();
                    }
                    else if (ck == ConsoleKey.RightArrow)
                    {
                        currertTarget.MoveCursorInTheAera(new Pos(1, 0), false);
                        SetIceWallCursor();
                    }
                    else if (ck == ConsoleKey.UpArrow)
                    {
                        currertTarget.MoveCursorInTheAera(new Pos(0, -1), false);
                        SetIceWallCursor();
                    }
                    else if (ck == ConsoleKey.DownArrow)
                    {
                        currertTarget.MoveCursorInTheAera(new Pos(0, 1), false);
                        SetIceWallCursor();
                    }
                    else if (ck == ConsoleKey.Spacebar)
                    {
                        currertTarget.Attack(scene.CursorPos, currertTarget.curretSkill);
                        state = State.stand;
                        scene.ReloadCursor(Scene.CursorType.Point);
                        scene.ResetMoveAera(new List<Pos>());
                        //if (currertTarget.Attack(scene.CursorPos, currertTarget.curretSkill))

                        if (currertTarget.act == 0)
                            currertTarget.isActive = false;
                    }
                }
                else if (state == State.stop)
                {
                    scene.AddDebugMessage("结束己方回合");
                    foreach (var v in lists)
                    {
                        scene.MoveCursorTo(v.location);
                        v.Attack(v.location, v.skills[6]);
                    }

                    //结束回合,交给其他控制器行动.
                    return true;
                }
                scene.MoveCursor(new Pos(0, 0));
                //scene.AddDebugMessage(state.ToString() + " " + scene.cursorLayer.CursorPos.x.ToString()
                //    + " " + scene.cursorLayer.CursorPos.y.ToString() );
                scene.Show();
            }
        }
        //坐标系解决问题 这个写的真满意. 可以选择放技能的方向
        private void SetIceWallCursor()
        {
            if (currertTarget.curretSkill.name == "冰墙" || currertTarget.curretSkill.name == "强化冰墙")
            {
                Skill s = currertTarget.curretSkill;
                Pos to = scene.CursorPos;
                int dx, dy;
                dx = to.x - currertTarget.location.x;
                dy = to.y - currertTarget.location.y;
                //scene.AddDebugMessage(to.x.ToString() + "cursor" + to.y.ToString());
                //scene.AddDebugMessage(currertTarget.location.x.ToString() + "location" + currertTarget.location.y.ToString());
                //scene.AddDebugMessage(dx.ToString() + " "+dy.ToString());
                if ((dx < 0 && dy > dx && dy < -dx) || (dx > 0 && dy < dx && dy > -dx))
                {
                    scene.cursorLayer.ReloadCurser(Scene.CursorType.Lline);
                }
                else
                {
                    scene.cursorLayer.ReloadCurser(Scene.CursorType.Sline);
                }
                scene.cursorLayer.RefreshMoveAera();
            }

        }
        public PlayerCharacter CreatStoneGiant(Pos location)
        {
            //被动 40%减伤
            PlayerCharacter sg = new PlayerCharacter(scene, location);
            sg.discription1 = "坦克,攻击产生仇恨";
            sg.discription2 = "岩石护甲:减伤40%";
            sg.name = "岩山";
            sg.hp = 350; sg.hpMax = 350;
            sg.mp = 80; sg.mpMax = 80;
            sg.act = 0; sg.actMax = 9;
            sg.pic = "岩";

            //添加三个技能
            sg.skills.Add(Skill.CreateZhongChui(true));
            sg.skills.Add(Skill.CreateShanFeng(true));
            sg.skills.Add(Skill.CreateShenZhang(sg.name));
            sg.skills.Add(Skill.CreateRest(5, 2));
            //把另两个增强版本藏起来算了
            sg.skills.Add(Skill.CreateZhongChui(true));
            sg.skills.Add(Skill.CreateShanFeng(true));

            sg.skills.Add(Skill.CreateRestAfterTurn(40, 10));
            //将技能的释放者设置为自己
            foreach (var v in sg.skills)
                v.caster = sg;
            return sg;
        }
        public PlayerCharacter CreatFireWizard(Pos location)
        {
            //被动 击杀敌人回复2行动30mp
            PlayerCharacter sg = new PlayerCharacter(scene, location);
            sg.discription1 = "主力法师,注意控蓝";
            sg.discription2 = "超载:击杀回复2行动力,30MP";
            sg.name = "火鸟";
            sg.hp = 160; sg.hpMax = 160;
            sg.mp = 200; sg.mpMax = 200;
            sg.act = 0; sg.actMax = 10;
            sg.pic = "烈";

            sg.skills.Add(Skill.CreateHuoYanDan(true));
            sg.skills.Add(Skill.CreateYunShi(true));
            sg.skills.Add(Skill.CreateShenZhang(sg.name));
            sg.skills.Add(Skill.CreateRest(0, 2));
            sg.skills.Add(Skill.CreateHuoYanDan(true));
            sg.skills.Add(Skill.CreateYunShi(true));

            sg.skills.Add(Skill.CreateRestAfterTurn(0, 30));
            foreach (var v in sg.skills)
                v.caster = sg;
            return sg;
        }

        public PlayerCharacter CreatHealer(Pos location)
        {
            //可以积攒3倍行动力
            PlayerCharacter sg = new PlayerCharacter(scene, location);
            sg.discription1 = "治疗者,可以为他人回复资源";
            sg.discription2 = "积蓄:可以积攒3回合行动力";
            sg.name = "愈灵";
            sg.hp = 200; sg.hpMax = 200;
            sg.mp = 150; sg.mpMax = 150;
            sg.act = 0; sg.actMax = 8;
            sg.pic = "愈";

            //添加三个技能
            sg.skills.Add(Skill.CreateLingQuan(true));
            sg.skills.Add(Skill.CreateYuHe(true));
            sg.skills.Add(Skill.CreateShenZhang(sg.name));
            sg.skills.Add(Skill.CreateRest(5, 5));
            sg.skills.Add(Skill.CreateLingQuan(true));
            sg.skills.Add(Skill.CreateYuHe(true));
            sg.skills.Add(Skill.CreateRestAfterTurn(20, 30));
            foreach (var v in sg.skills)
                v.caster = sg;
            return sg;
        }

        public PlayerCharacter CreatIceWizard(Pos location)
        {
            //被技能击中会产生减速
            PlayerCharacter sg = new PlayerCharacter(scene, location);
            sg.discription1 = "控制法师,擅长AOE和控制";
            sg.discription2 = "冰封:技能可以使敌人减速";
            sg.name = "暴雪";
            sg.hp = 200; sg.hpMax = 200;
            sg.mp = 200; sg.mpMax = 200;
            sg.act = 0; sg.actMax = 11;
            sg.pic = "寒";

            //添加三个技能
            sg.skills.Add(Skill.CreateBilzzard(true));
            sg.skills.Add(Skill.CreateIceWall(true));
            sg.skills.Add(Skill.CreateShenZhang(sg.name));
            sg.skills.Add(Skill.CreateRest(0, 10));
            sg.skills.Add(Skill.CreateBilzzard(true));
            sg.skills.Add(Skill.CreateIceWall(true));
            sg.skills.Add(Skill.CreateRestAfterTurn(0, 40));
            foreach (var v in sg.skills)
                v.caster = sg;
            return sg;
        }

        public PlayerCharacter CreatAssassin(Pos location)
        {
            //刀扇每次击中敌人回复2行动力和10mp
            //强化1技能 连续背刺输出伤害 靠刀扇回能 战术AOE输出
            //强化2技能 连续刀扇输出伤害 背刺是送的 AOE输出
            //神杖 消耗能量输出伤害 背刺是送的 刀扇用来回能 单体输出
            PlayerCharacter sg = new PlayerCharacter(scene, location);
            sg.discription1 = "近战刺客脆皮,休息快速回血";
            sg.discription2 = "疾行:刀扇击中敌人回能";
            sg.name = "影刺";
            sg.hp = 160; sg.hpMax = 160;
            sg.mp = 100; sg.mpMax = 100;
            sg.act = 0; sg.actMax = 15;
            sg.pic = "影";

            //添加三个技能
            sg.skills.Add(Skill.CreateBeiCi(true));
            sg.skills.Add(Skill.CreateDaoShan(true));
            sg.skills.Add(Skill.CreateShenZhang(sg.name));
            sg.skills.Add(Skill.CreateRest(10, 0));
            sg.skills.Add(Skill.CreateBeiCi(true));
            sg.skills.Add(Skill.CreateDaoShan(true));
            sg.skills.Add(Skill.CreateRestAfterTurn(0, 20));
            foreach (var v in sg.skills)
                v.caster = sg;
            return sg;
        }

        public PlayerCharacter CreatPaladin(Pos location)
        {
            //击杀的伤害会剩余1点血
            //强化1 : 远程低消耗拉仇恨 被动攒资源 有限的拯救光
            //强化2: 单体拉仇恨,仇恨可能不足 很多高效的拯救光 当奶用
            //神杖: 混合使用单体风筝 作为副T
            PlayerCharacter sg = new PlayerCharacter(scene, location);
            sg.discription1 = "坦克,产生仇恨与治疗";
            sg.discription2 = "拯救者,濒死时抵挡一次伤害";
            sg.name = "圣者";
            sg.hp = 400; sg.hpMax = 400;
            sg.mp = 150; sg.mpMax = 150;
            sg.act = 8; sg.actMax = 8;
            sg.pic = "圣";

            //添加三个技能
            sg.skills.Add(Skill.CreateCuiZi(true));
            sg.skills.Add(Skill.CreateZhengjiu(true));
            sg.skills.Add(Skill.CreateShenZhang(sg.name));
            sg.skills.Add(Skill.CreateRest(0, 5));
            sg.skills.Add(Skill.CreateCuiZi(true));
            sg.skills.Add(Skill.CreateZhengjiu(true));
            sg.skills.Add(Skill.CreateRestAfterTurn(0, 30));
            foreach (var v in sg.skills)
                v.caster = sg;
            return sg;
        }

    }

    public class PlayerCharacter : Target
    {

        public enum Items
        {
            BoostShoos,
            Groves,
            Scepter,
        }
        public PlayerCharacter(Scene scene, Pos p) : base(scene)
        {
            colorActive = ConsoleColor.Blue;
            colorNActive = ConsoleColor.DarkBlue;
            faction = Faction.Player;
            belongsTo = Target.BelongsTo.PlayerSelf;
            location = p;
            scene.SelectBlock(location).target = this;
        }
        //被攻击时进行 敌人可能需要额外的攻击种类
        public override void BeHit(Skill s)
        {
            base.BeHit(s);
        }

        public override void Dead()
        {
            base.Dead();
            scene.playerController.lists.Remove(this);
        }


        //准备执行攻击操作 主要是玩家的攻击控制器
        public override bool Attack(Pos to, Skill s)
        {
            if (s.type == SkillType.Rest)
            {
                int t = act;
                changeAct(-act);
                changeHP(s.heal * t);
                changeMP(s.addMana * t);
                string message = name + "休息:消耗行动" + t.ToString() + ";";
                if (s.heal != 0)
                {
                    message += "回复" + (s.heal * t).ToString() + "HP;";
                }
                if (s.addMana != 0)
                {
                    message += "回复" + (s.addMana * t).ToString() + "MP;";
                }
                scene.AddDebugMessage("#B" + message);
            }
            List<Pos> list = new List<Pos>();
            list.AddRange(scene.cursorLayer.cursor.cursorAera);
            list.Add(new Pos(0, 0));
            bool flag = false;
            if (s.costAct > act)
            {
                scene.AddDebugMessage("#B行动力不足");
                return false;
            }
            if (s.costMana > mp)
            {
                scene.AddDebugMessage("#B法力值不足");
                return false;
            }

            if (s.type == SkillType.Damage || s.type == SkillType.Holy)
            {
                foreach (var v in list)
                {
                    //如果不是空  就执行攻击
                    //之后加入一点更复杂的条件
                    if (scene.SelectBlock(v + to).target != null)
                    {
                        if (scene.SelectBlock(v + to).target.faction != faction)
                        {
                            scene.SelectBlock(v + to).target.BeHit(s);

                            scene.ResetMoveAera(new List<Pos>());
                            scene.ResetRouteAera(new List<Pos>());

                            scene.MoveCursor(new Pos(0, 0));
                            scene.ReloadCursor(Scene.CursorType.Point);

                            flag = true;
                        }
                    }
                }
            }

            if (s.type == SkillType.Heal)
            {
                foreach (var v in list)
                {
                    //如果不是空  就执行攻击
                    //之后加入一点更复杂的条件
                    if (scene.SelectBlock(v + to).target != null)
                    {
                        if (scene.SelectBlock(v + to).target.faction == faction)
                        {
                            scene.SelectBlock(v + to).target.BeHit(s);

                            scene.ResetMoveAera(new List<Pos>());
                            scene.ResetRouteAera(new List<Pos>());

                            scene.MoveCursor(new Pos(0, 0));
                            scene.ReloadCursor(Scene.CursorType.Point);

                            flag = true;
                        }
                    }
                }
            }

            else if (s.type == SkillType.Create)
            {
                foreach (var v in list)
                {
                    //如果是空则执行创造
                    if (scene.SelectBlock(v + to).target == null && scene.SelectBlock(v + to).CanMove == true)
                    {
                        scene.SelectBlock(v + to).target = new Stone(scene, v + to, s.summonHP);

                        scene.environmentController.lists.Add(scene.SelectBlock(v + to).target);

                        scene.ResetMoveAera(new List<Pos>());
                        scene.ResetRouteAera(new List<Pos>());

                        scene.MoveCursor(new Pos(0, 0));
                        scene.ReloadCursor(Scene.CursorType.Point);
                        flag = true;
                    }
                }
            }
            if (flag)
            {
                changeAct(-s.costAct);
                changeMP(-s.costMana);
            }
            return flag;
        }

        //准备释放技能,与玩家有关
        public override bool CostSkill(Skill s)
        {
            if (s.costAct > act)
            {
                scene.AddDebugMessage("#B行动力不足" + act.ToString() + "/" + s.costAct.ToString());
                return false;
            }
            if (s.costMana > mp)
            {
                scene.AddDebugMessage("#B法力值不足" + mp.ToString() + "/" + s.costMana.ToString());
                return false;
            }

            canMoveAera = scene.ShowAera(location, s.range, BFSSearchAll);
            scene.ResetMoveAera(canMoveAera);
            scene.ReloadCursor(s.cursorType);
            curretSkill = s;
            return true;
        }

    }

    public class Stone : Target
    {
        public Stone(Scene scene, Pos location, int HP) : base(scene)
        {
            name = "岩石屏障";
            hp = HP; hpMax = HP;
            pic = "垚";
            this.location = location;
            colorActive = ConsoleColor.White;
            scene.SelectBlock(location).target = this;

            belongsTo = BelongsTo.PlayerOther;
            faction = Faction.Friendly;
        }
        public override void Action()
        {
            changeHP(-100);
            if (hp <= hpMax / 3)
            {
                pic = "土";
            }
            else if (hp <= hpMax * 2 / 3)
            {
                pic = "圭";
            }
            if (hp == 0)
                Dead();
        }
        public override void Dead()
        {
            base.Dead();
            scene.environmentController.lists.Remove(this);
        }
    }
}

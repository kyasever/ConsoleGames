using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;

/**    
 * When I wrote this, only God and I understood what I was doing
 * Now, God only knows
 */


/* todo
 * 这个大门的关卡设计就是不正确的,应该更改地图并取消大门.不管发生了什么都是3回合之后开始刷怪,去掉begin关卡
 * 
 * 把需要实现的功能接口都抽象接口出来
 * 把一些功能性函数抽象为静态类 之后也可以用.
 * 
 * 多考虑游戏上手程度.硬核的东西可以留在后面关卡.
 * 下一作一定要1分钟上手,然后缓慢提升至硬核难度
 * 
 * 待更新
 * 增加一个新手教学关卡
 * 增加结束界面,统计本次战斗的各项数据.(成就系统)
 * 
 * 9/13 更新V2.1 继续更新网络 
 * 
 * 9/5 更新V2.0 网络更新基础
 * 
 * 9/1更新 V1.3
 * 增加了结束界面
 * 增加了影刺和圣者两个角色 近战刺客和自疗坦克
 * 增加了回合结束后会产生自动回复.所以适当削弱了各个职业的休息回复
 * 岩山 由于回复机制的修改,增加封路消耗.去掉了攻击回血,减伤削弱为40%
 * 变为依靠休息和默认回血来抵抗,增加50血上限
 * 火鸟 提升了火球术的消耗和伤害与默认回复平衡.减少了一点休息回蓝,这个角色不应该通过休息回复
 * 暴雪 增加了冰墙的行动力消耗和耗蓝
 * 
 * 
 * 8.20更新 V1.2 这个应该是最终版了
 * 优化了行动力模式
 * 第一关结束之后进行一次回血
 * 优化了控制台适配模式
 * 给boss增加了一个弱点技能,弱点会受到300额外伤害,boss除了弱点只会受到1/3伤害 
 * 
 * 岩山,降低重锤消耗和伤害,主要用来拉怪.降低神杖的消耗,提升射程,可以用神杖拉怪
 * 降低山峰的血量和消耗
 * 火鸟 提升10被动回蓝,略微提升陨石术的消耗
 * 愈灵 强化灵泉的性价比升高 冰法提升休息回复 强化神杖
 * 
 * 8.18更新
 * 增加了boss关卡 boss拥有两个技能 还有一个弱点技能待更新
 * 弱点: 攻击后根据自身的愤怒值产生一定数量的弱点.弱点受到神杖攻击后会自行摧毁并受到额外伤害.
 * 会刷新很多弱点,治疗的aoe神杖可以极大提升清除弱点的速度.
 * 行动力可以积攒两个回合,但是每回合最多只能积攒一次移动
 * 烈焰之地现在不会对敌人和墙造成伤害
 * 
 * 普遍增加了血血量,避免被秒杀
 * 岩山: 增加了50%减伤 攻击命中回复10hp 去除被动减伤和被打回蓝
 * 增强了普通重锤的伤害 可以靠重锤回复血量 削50点墙血 极大增强回复能力,需要主动去拉怪抗
 * 火鸟:增加到20的击杀回蓝 可以永续火球术了 陨石杀五个回本 普通火球术可以用来回蓝
 * 强化火球术30耗蓝,不能永续,但是单体输出较高
 * 愈灵:增强一下愈合和休息,比例不变 增强治疗能力
 * 暴雪:增强暴风雪的输出能力.减少冰墙的血,增强2回蓝,多用休息回蓝
 * 11行动相当于每回合回66mp
 *  
 * 岩山的神杖不耗蓝 可以平常用来补刀 输出还可以,还能回血,一直点就行
 * 火鸟的神杖伤害削弱到200,可以用来点杀精英,弱化的火球术可以用来回蓝
 * 愈灵的神杖可以用来范围驱散和范围杀弱点
 * 暴雪的神杖可以用来点炸弹,和补充单体输出
 * 
 * 
 * enemycontroller action 加了个补丁
 * bfs 寻路配置那里 加了个补丁
 * 
 * 
 * 8.17更新
 * 体验性更新:
 * 增加了血条和掉血动画,有了更平滑的掉血效果
 * 完整更新了一遍UI配色,有更明确的显示
 * 增加了难度和新手引导功能,有明确的指示
 * 优化游戏性调整和操作模式,优化光标移动功能
 * 优化了敌人的行动模式,消耗更少的资源和时间
 * 
 * 游戏性更新:
 * 优化了自爆球的AI表现,自爆球现在可以在火焰之地上获得额外的行动力,但是长距离移动之后不能马上自爆
 * 岩山:
 * 9行动力 增加了血量和蓝量并增加了一点法力回复
 * 减少了重锤的消耗和伤害,应该更频繁的使用这个技能拉怪,强化重锤有更高的消耗,这更倾向于一个输出技能.
 * 增强了一点普通的山封技能,可以灵活的用这个堵一个小缝. 削弱墙的血量
 * 火鸟:
 * 增加了一点血量,别被自爆球秒了.
 * 降低了一点强化火球术的伤害,应该更谨慎的使用这个技能补刀降低蓝耗
 * 降低了陨石术的行动力消耗,尽可能的多砸死敌人来保证不亏
 * 愈灵:
 * 提升了血量和回复效率,回复自足循环可以打出.
 * 强化灵泉该为输送更多的法力值.提升愈合的治疗效率,快速愈合大幅降低行动消耗
 * 暴雪:
 * 提升了蓝量,降低了一点行动力.
 * 降低了冰墙的血量,增加了消耗
 * 削弱了暴雪的神杖,略微优化了其他三人的神杖
 *
 * 补充更新
 * 改了一下AI 可以无脑冲墙了.刚才没给递归一个退出条件 坑了
 * 
 * 
 * 还剩三天
 * 2 游戏内机制还欠缺的东西
 * c.boss的设定.boss使用bossController来控制,不扯淡了.
 * d.平衡性测试调整和单位文本数据定格说明
 * 3.游戏外机制需要补的东西
 * 转场写好了之后开发分支版本,网络联机版.使用操作同步.
 * json又不是不会用,为了练而练就无聊了,何况已经练过了
 * 
 * 检测一方的按键,另一方等待,检测到按键之后开始执行,双方只同步操作
 * 
 * 最后需要达到的效果,boss战斗一定要有 不管流程多简化.
 * 实在不行就精简一下中间关.第一关分配物品,回形关卡风筝怪物第二关直接boss
 * 
 * 先做好一个测试关,把地图扩大.然后使用回形策略通过封堵口子形成塔防策略
 * 在大场景中持续刷怪 看能杀多少... 感觉并没有实现写boss了...
 * 
 * 还差行动力检测和完整的回合检测.
 * 
 * debug框是持续滚动显示的 应该给debug框加一个颜色
 * 
 * others
 * 网络功能最好要加,只供练习,用操作同步,这样可以极大减少网络工作量
 * json读取功能加一下,把人物显示内容用json搞 用作练习 别的不用json存了就 不方便
 * 道具处理使用环境控制器处理.每回合开始直接全局搜索算了.用环境控制器改block的贴图
 * 并创建事件,之后检测是否有单位捡起来了.
 * 环境控制器同样可以考虑兼容成就系统
 * 
 * 配音系统估计不加了,没啥意思...
 * 
 * ♜♟♞ 2560 ☆ ♀  〓 █  ▦ ❿ ❾ ❽ ❼ ❻ ❺ ❹ ❸ ❷ ❶
 * 
    V2.1  
    添加了网络联机功能(测试中,不可用)  
    增加了两个新的角色

 * 
 * 2019/8/9 V2.2
 * 时隔一年进行了一次平衡性更新修正,取消捡起物品的操作,所有角色目前默认12技能的强化版和3技能神杖.
 * 火鸟的回合后回蓝增加10,避免不小心没杀到人宕机了.
 * 愈灵的每回合回蓝增加10,增加队伍容错
 * 冰封 每行动力回蓝增加2,
 * 影刺 每回合回蓝+10 被刺增加1行动消耗.
 * ----增加一点点圣骑的攻击,增加坦度. 没有减伤也没有续航,太脆了.
 * 圣骑 锤子+5攻击(10) 神杖+10攻 回合恢复mp+10 HP上限增加100(400)
 */
namespace WizardAdvanture
{
    class Program
    {
        static void Main(string[] args)
        {

            // Canvas c = new Canvas();
            //System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            //stopwatch.Start(); //  开始监视代码运行时间
            //TimeSpan timespan = stopwatch.Elapsed; //  获取当前实例测量得出的总时间
            //double milliseconds = timespan.TotalMilliseconds;  //  总毫秒数
            //c.Refresh();
            //int n = 0;
            //double last = timespan.TotalMilliseconds;
            //while (true)
            //{
            //    timespan = stopwatch.Elapsed;
            //    milliseconds = timespan.TotalMilliseconds;  //  总毫秒数
            //    n++;
            //    c.AddDebugMessage(n.ToString() + "测试" + (milliseconds - last));
            //    c.Refresh();
            //    last = milliseconds;
            //    Thread.Sleep(1000);
            //}

            new GameController().Action();
        }
    }
    //主战斗场景Scene
    public class Scene
    {
        //随机数生成器 但是因为网络问题不能用
        public Random random = new Random(5);
        //用于保存地图格子信息
        public Block[,] blocks;
        //是否处于调试模式 
        public bool debugMode = false;
        //游戏难度
        public enum Difficult
        {
            easy, hard
        }
        //游戏模式
        public enum GameMode
        {
            single, miuti,
        }
        public GameMode gameMode;


        public Difficult difficult = Difficult.hard;
        public Canvas canvas;
        //光标层
        public CursorLayer cursorLayer;
        //玩家角色控制器
        public PlayerController playerController;
        //另一个玩家的控制器
        public NetController netController;

        public EnvironmentController environmentController;
        public EnemyController enemyController;

        //public bool isGameOver = false;
        //回合数
        public int turns = 0;
        //地图的xy长宽
        public int mapX, mapY;

        public Thread thread;


        public enum NetPlayer
        {
            single = 0,
            P1 = 1,
            P2 = 2,
        }
        public NetPlayer netPlayer;

        public Scene(GameMode gameMode, NetPlayer netPlayer)
        {
            this.gameMode = gameMode;
            this.netPlayer = netPlayer;

            canvas = new Canvas(this, new Pos(5, 5));
            InitMap();
            cursorLayer = new CursorLayer(this, new Pos(5, 5));
            playerController = new PlayerController(this);
            if (gameMode == GameMode.miuti)
            {
                netController = new NetController(this);
            }
            enemyController = new EnemyController(this);
            environmentController = new EnvironmentController(this);
            canvas.InitUI();
            Console.CursorVisible = false;
            Console.OutputEncoding = Encoding.UTF8;
        }


        public void PlayMusic()
        {
            //播放wav音乐模块
            SoundPlayer sp = new SoundPlayer(@"d:\Lemon.wav");
            sp.PlaySync();
        }

        public enum BattleResult
        {
            battle, gameOver, gameWin,
        }
        //返回战斗是否还在进行
        public BattleResult Battle()
        {
            if (GetPlayers().Count == 0)
            {
                return BattleResult.gameOver;
            }
            if (environmentController.bossCollection.isDead)
            {
                return BattleResult.gameWin;
            }
            turns++;
            SetTimes(turns);
            //环境控制器
            environmentController.Action();

            if (gameMode == GameMode.single)
            {
                AddDebugMessage("#D己方回合");
                playerController.Action();
            }
            //多人游戏下P1先行动P2后行动
            else if (gameMode == GameMode.miuti)
            {
                if (netPlayer == NetPlayer.P1)
                {
                    AddDebugMessage("#D己方回合 等待自己行动");
                    playerController.Action();
                    AddDebugMessage("#D友方回合 等待NetC行动");
                    netController.Action();
                }
                else if (netPlayer == NetPlayer.P2)
                {
                    AddDebugMessage("#D友方回合 等待NetC行动");
                    netController.Action();
                    AddDebugMessage("#D己方回合 等待自己行动");
                    playerController.Action();
                }
            }



            AddDebugMessage("#W敌方回合");
            enemyController.Action();

            //AddDebugMessage("envi:" + environmentController.lists.Count.ToString() +
            //    " player" + playerController.lists.Count.ToString()+
            //    " enemy" + enemyController.lists.Count.ToString());
            return BattleResult.battle;
        }

        /// <summary>
        /// 输出Scene画面到控制台 
        /// 一次调用大概34ms,应该考虑一下双缓冲了...
        /// </summary>
        public void Show()
        {
            canvas.Refresh();
        }



        #region 寻路算法
        int[,] bfs;
        /// <summary>
        /// 委托方法,用于调用本地函数.
        /// </summary>
        delegate bool Func(int ox, int oy);
        /// <summary>
        /// 委托方法 p点对于使用者是否是可以通过的
        /// </summary>
        public delegate bool CanMove(Pos p);

        /// <summary>
        /// 基于距离判定的优化DFS算法 起点终点,返回路径点列表
        /// </summary>
        public List<Pos> BFS(Pos start, Pos stop, CanMove canMoveFunc)
        {
            int x = blocks.GetLength(0);//获取维数，这里指行数
            int y = blocks.GetLength(1);//获取指定维度中的元素个数，这里也就是列数了。
            //int x = blocks.GetUpperBound(0) + 1;
            bfs = new int[x, y];
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    bfs[i, j] = short.MaxValue;
                }
            }
            List<Pos> queue = new List<Pos>();

            bfs[start.x, start.y] = 0;
            queue.Add(start);

            Pos p = start;
            bool flag = true;


            Func func = (int ox, int oy) =>
            {
                if (canMoveFunc(p + new Pos(ox, oy)) && isInMap(p + new Pos(ox, oy))
                && bfs[p.x + ox, p.y + oy] == short.MaxValue)
                {

                    bfs[p.x + ox, p.y + oy] = bfs[p.x, p.y] + 1;
                    if (debugMode)
                    {
                        blocks[p.x + ox, p.y + oy].BackColor = ConsoleColor.Blue;
                    }
                    Pos np = new Pos(p.x + ox, p.y + oy);
                    if (np == stop)
                    {
                        flag = false;
                    }
                    else if (np.Distanse(stop) <= p.Distanse(stop))
                        queue.Insert(0, np);
                    else
                        queue.Add(np);
                }
                else if ((p + new Pos(ox, oy)) == stop)
                {
                    flag = false;
                }
                return true;
            };

            while (flag)
            {
                if (queue.Count == 0)
                {
                    return new List<Pos>() { stop };
                }
                else
                {
                    if (debugMode)
                        Show();
                    //初始化当前这个点
                    //取第一个点,值最小的点
                    p = queue[0];
                    //取最后一个点 深度优先
                    //p = queue.Last();

                    func(1, 0);
                    func(-1, 0);
                    func(0, 1);
                    func(0, -1);

                    //去掉这个点
                    queue.Remove(p);
                }
            }

            if (debugMode)
            {
                Show();
                Console.ReadKey();
            }

            //搜寻路径
            List<Pos> path = new List<Pos>();
            path.Add(stop);
            flag = true;
            while (flag)
            {
                if (debugMode)
                    Show();

                p = path.First();

                if (p == start)
                {
                    return path;
                }

                List<Pos> min = new List<Pos>();
                min.Add(p);
                //左面
                if (bfs[p.x - 1, p.y] < bfs[min.Last().x, min.Last().y])
                {
                    min.Add(new Pos(p.x - 1, p.y));
                }
                if (bfs[p.x + 1, p.y] < bfs[min.Last().x, min.Last().y])
                {
                    min.Add(new Pos(p.x + 1, p.y));
                }
                if (bfs[p.x, p.y - 1] < bfs[min.Last().x, min.Last().y])
                {
                    min.Add(new Pos(p.x, p.y - 1));
                }
                if (bfs[p.x, p.y + 1] < bfs[min.Last().x, min.Last().y])
                {
                    min.Add(new Pos(p.x, p.y + 1));
                }
                if (debugMode)
                    blocks[min.Last().x, min.Last().y].BackColor = ConsoleColor.Red;
                path.Insert(0, new Pos(min.Last().x, min.Last().y));
            }
            return path;
        }

        private bool isInMap(Pos pos)
        {
            int x = mapX;
            int y = mapY;
            if (pos.x < 0)
                return false;
            if (pos.x >= x)
                return false;
            if (pos.y < 0)
                return false;
            if (pos.y >= y)
                return false;
            return true;
        }

        /// <summary>
        /// 输入 起始点,最远距离,返回点集合,可以使用的点的集合,之后再使用BFS走到对应点上
        /// </summary>
        public List<Pos> ShowAera(Pos start, int distanse, CanMove canMoveFunc)
        {
            int y = blocks.GetLength(1);//获取指定维度中的元素个数，这里也就是列数了。
            int x = blocks.GetUpperBound(0) + 1;
            bfs = new int[x, y];
            //初始化
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    bfs[i, j] = short.MaxValue;
                }
            }
            List<Pos> queue = new List<Pos>();
            List<Pos> avaliablePos = new List<Pos>();

            bfs[start.x, start.y] = 0;
            queue.Add(start);

            Pos p = start;
            bool flag = true;


            //c# 7.0才支持的本地方法....这个
            Func func = (int ox, int oy) =>
            {
                if (isInMap(p + new Pos(ox, oy)) && canMoveFunc(p + new Pos(ox, oy))
                && bfs[p.x + ox, p.y + oy] == short.MaxValue)
                {

                    if (bfs[p.x, p.y] + 1 > distanse)
                    {
                        //queue.Remove(p);
                        flag = false;
                        return false;
                    }
                    bfs[p.x + ox, p.y + oy] = bfs[p.x, p.y] + 1;
                    //m.blocks[p.x + ox, p.y + oy].isArea = true;
                    Pos np = new Pos(p.x + ox, p.y + oy);
                    queue.Add(np);
                    avaliablePos.Add(new Pos(p.x + ox, p.y + oy));
                }

                return true;
            };
            while (flag)
            {
                if (queue.Count == 0)
                {
                    avaliablePos.Add(start);
                    return avaliablePos;
                }
                else
                {
                    if (debugMode)
                        Show();
                    //取第一个点,值最小的点
                    p = queue[0];
                    //右边 如果右面为撞墙则返回np
                    func(1, 0);
                    func(-1, 0);
                    func(0, 1);
                    func(0, -1);

                    //去掉这个点
                    queue.Remove(p);
                }
            }
            avaliablePos.Add(start);
            return avaliablePos;
        }
        #endregion
        //通过pos对象点搜索block对象,不能超过边界
        public Block SelectBlock(Pos p)
        {
            if (p.x < 0)
                p.x = 0;
            if (p.x > mapX - 1)
                p.x = mapX - 1;
            if (p.y < 0)
                p.y = 0;
            if (p.y > mapY - 1)
                p.y = mapY - 1;
            return blocks[p.x, p.y];
        }

        #region CursorLayer 相关整理调用
        //当前光标位置
        public Pos CursorPos
        {
            get
            {
                return cursorLayer.cursor.centerPos;
            }
            set { cursorLayer.cursor.centerPos = value; }
        }

        public enum CursorType
        {
            Point,
            Cross,
            Sline,
            Square,
            Lline
        }

        public void ReloadCursor(Scene.CursorType type)
        {
            cursorLayer.ReloadCurser(type);
        }

        /// <summary>
        /// 移动光标 绝对位移
        /// </summary>
        public void MoveCursorTo(Pos p)
        {
            MoveCursor(p - CursorPos);
        }

        /// <summary>
        /// 移动光标 相对位移
        /// </summary>
        public void MoveCursor(Pos p)
        {
            cursorLayer.MoveCursor(p);
            //显示指针对着的目标信息
            if (SelectBlock(cursorLayer.CursorPos) != null)
            {
                ShowTargetMessage(SelectBlock(cursorLayer.CursorPos));
            }
            //移动摄像机
            if (CursorPos.x < canvas.startPos.x + canvas.limitToMove)
            {
                canvas.MoveCameraTo(new Pos(CursorPos.x - canvas.startPos.x - canvas.limitToMove, 0));
                //canvas.MoveCamera(new Pos(-1, 0));
            }
            if (CursorPos.x > canvas.startPos.x + (20 - canvas.limitToMove))
            {
                canvas.MoveCameraTo(new Pos(CursorPos.x - canvas.startPos.x - (20 - canvas.limitToMove), 0));
                //canvas.MoveCamera(new Pos(1, 0));
            }
            if (CursorPos.y < canvas.startPos.y + canvas.limitToMove)
            {
                canvas.MoveCameraTo(new Pos(0, CursorPos.y - canvas.startPos.y - canvas.limitToMove));
                //canvas.MoveCamera(new Pos(0, -1));
            }
            if (CursorPos.y > canvas.startPos.y + (20 - canvas.limitToMove))
            {
                canvas.MoveCameraTo(new Pos(0, CursorPos.y - canvas.startPos.y - (20 - canvas.limitToMove)));
                //canvas.MoveCamera(new Pos(0, 1));

            }
        }


        //重新加载可移动区域
        public void ResetMoveAera(List<Pos> aera)
        {
            cursorLayer.ResetMoveAera(aera);
        }
        //重新加载路径区域
        public void ResetRouteAera(List<Pos> aera)
        {
            cursorLayer.ResetRouteAera(aera);
        }
        #endregion

        #region ui显示相关调用
        //设置当前进行到了第几个回合
        public void SetTimes(int n)
        {
            canvas.SetTurns(n);
        }
        public void AddDebugMessage(string s)
        {
            canvas.AddDebugMessage(s);
        }
        public void ShowTargetMessage(Block b)
        {
            canvas.block = b;
            //canvas.ShowTargetMessage(b);
        }
        //这个应该改一下 直接改成技能定制
        public void ShowSkillMessage(Skill s)
        {
            canvas.ShowImmediateMessage(s.discription);
        }

        public void ShowMoveMessage(Target t, int a)
        {
            string str = "#C移动中" + "当前:" + t.act + ",消耗:" + a.ToString();
            canvas.ShowImmediateMessage(str);
        }
        #endregion

        /// <summary>
        /// 根据文件路径读取并生成地图
        /// </summary>
        public void InitMap()
        {
            List<List<Block>> blocksLoad = new List<List<Block>>();
            StreamReader sr = FileController.GetFileReader("Scene1.txt");
            string str = "";
            while ((str = sr.ReadLine()) != null)
            {
                List<Block> list = new List<Block>();
                for (int i = 0; i < str.Length; i++)
                {
                    list.Add(BlockFactory.CreateBlock(str[i]));
                }
                blocksLoad.Add(list);
            }

            mapY = blocksLoad.Count();
            mapX = blocksLoad[0].Count();
            //List<List<Block>> blocks_r = new List<List<Block>>();
            blocks = new Block[mapX, mapY];

            for (int i = 0; i < mapX; i++)
            {
                for (int j = 0; j < mapY; j++)
                {
                    blocks[i, j] = blocksLoad[j][i];
                }
            }

        }

        //返回角色控制器控制的对象
        public List<Target> GetPlayers()
        {
            if (gameMode == GameMode.single)
                return playerController.lists;
            else if (gameMode == GameMode.miuti)
            {
                List<Target> list = new List<Target>();
                list.AddRange(playerController.lists);
                list.AddRange(netController.lists);
                return list;
            }
            return null;
        }
    }

}

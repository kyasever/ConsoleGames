using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace WizardAdvanture
{
    //主游戏控制器
    public class GameController
    {
        public enum Result
        {
            cancel,
            confirm,
            startSingleEasy,
            startSingleHard,
            startMiuti,
            startAchivement,
            startQuit,
        }
        public enum State
        {
            start,
            battle,
            select,
            dead,
            login,
            win,
        };


        State state = State.start;
        Scene scene;
        int turns = 0;

        public GameController()
        {
            Console.SetBufferSize(180, 50);
            scene = new Scene(Scene.GameMode.single, Scene.NetPlayer.single);
            // Thread t = new Thread(scene.PlayMusic);
            //t.Start();
        }
        public void Action()
        {
            Result result;
            bool flag = true;
            while (flag)
            {
                //之后加入不同场景控制
                switch (state)
                {
                    case State.start:
                        result = LoadStartScene();
                        if (result == Result.startSingleEasy)
                        {
                            state = State.select;
                            scene.difficult = Scene.Difficult.easy;
                            Console.Clear();
                        }
                        else if (result == Result.startSingleHard)
                        {
                            state = State.select;
                            scene.difficult = Scene.Difficult.hard;
                            Console.Clear();
                        }
                        else if (result == Result.startMiuti)
                        {
                            state = State.login;
                        }
                        else if (result == Result.startQuit)
                        {
                            flag = false;
                        }
                        break;
                    case State.select:
                        result = LoadSelectScene();
                        if (result == Result.cancel)
                        {
                            state = State.start;
                        }
                        else if (result == Result.confirm)
                        {
                            state = State.battle;
                        }
                        break;
                    case State.battle:
                        Scene.BattleResult re = scene.Battle();
                        if (re == Scene.BattleResult.battle)
                        {
                            break;
                        }
                        else if (re == Scene.BattleResult.gameOver)
                        {
                            turns = scene.turns;
                            state = State.dead;
                        }
                        else if (re == Scene.BattleResult.gameWin)
                        {
                            turns = scene.turns;
                            state = State.win;
                        }
                        break;
                    case State.login:
                        result = LoadLoginScene();
                        if (result == Result.confirm)
                        {
                            state = State.battle;
                            Console.Clear();
                        }
                        else if (result == Result.cancel)
                        {
                            flag = false;
                        }
                        break;
                    case State.dead:
                        result = LoadGameOverScene();
                        if (result == Result.confirm)
                        {
                            state = State.start;
                            Console.Clear();
                        }
                        else if (result == Result.cancel)
                        {
                            flag = false;
                        }
                        break;
                    case State.win:
                        result = LoadWinScene();
                        if (result == Result.confirm)
                        {
                            state = State.start;
                            Console.Clear();
                        }
                        else if (result == Result.cancel)
                        {
                            flag = false;
                        }
                        break;
                }
            }
        }

        #region 网络连接部分
        Scene.NetPlayer netPlayer;
        string otherPlayerSelect;
        MyTCPClient client;
        void OnHeartBeat(ResponseMessage msg)
        {
            //Console.WriteLine("收到心跳响应");
        }

        void OnLoginResult(ResponseMessage msg)
        {
            //直接提取第5个byte作为结果
            byte c = msg.rawMessage[4];
            if (c == 1)
            {
                netPlayer = Scene.NetPlayer.P1;
            }
            else if (c == 2)
            {
                netPlayer = Scene.NetPlayer.P2;
            }
            else
            {
                netPlayer = Scene.NetPlayer.single;
            }
            //直接提取第6,7,8个byte作为另一个角色选择的结果
            otherPlayerSelect = msg.message.Substring(1);
            Console.WriteLine("收到服务器消息,本局游戏你是:" + netPlayer);
            Console.WriteLine("你的队友选择的角色是" + otherPlayerSelect);
            Console.ReadLine();
        }

        void OnReceiveAction(ResponseMessage msg)
        {
            //收到任何操作列表就直接往list里加就算了.
            scene.netController.netInputKeys.Add(msg.rawMessage[4]);
        }

        void InitMessages()
        {
            var client = MyTCPClient.Instance;
            client.AddProcessFunc(GameAction.Game, GameCmd.HeartBeat, OnHeartBeat);
            client.AddProcessFunc(GameAction.Game, GameCmd.LoginGame, OnLoginResult);
            client.AddProcessFunc(GameAction.Game, GameCmd.Action, OnReceiveAction);
        }
        //处理收到的信息
        void DispatchMessage(ResponseMessage msg)
        {
            //Console.WriteLine("收到一个消息" + (GameCmd)msg.act);
            int key = (msg.act << 16) + msg.cmd;
            if (!MyTCPClient.Instance.dictProcessFunc.ContainsKey(key))
            {
                //AddDebugMessage("收到未注册的方法");
                //Debug.WriteLine("收到未注册的方法");
                return;
            }
            // 调用已注册的方法
            MyTCPClient.Instance.dictProcessFunc[key](msg);
        }
        //每隔5秒发送一个心跳包
        public void Send()
        {
            int i = 0;
            while (true)
            {
                i++;
                // 发送
                var writer = MyTCPClient.Instance.GetSendStream(GameAction.Game, GameCmd.HeartBeat);
                MyTCPClient.Instance.Send(writer);
                Thread.Sleep(5000);
            }

        }
        //启动线程不停的接收一秒100次
        public void Receive()
        {
            int i = 0;
            while (true)
            {
                i++;
                // 先接收
                while (client.messageQueue.Count > 0)
                {
                    var msg = client.messageQueue.Dequeue();
                    DispatchMessage(msg);
                }
                Thread.Sleep(10);
            }
        }
        /// <summary>
        /// 作为客户端创建多人连接
        /// </summary>
        public bool ConnectMiutiMode(string ip, int port)
        {
            try
            {
                Console.WriteLine("准备建立连接ip" + ip);
                client = MyTCPClient.Instance;
                client.Init(ip, port);

                client.Connect();

                InitMessages();
                return true;
            }
            catch
            {

                return false;
            }
        }
        public Result LoadLoginScene()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("请输入要连接的ip地址,输入d/default表示使用默认地址q退出");
            Console.ForegroundColor = ConsoleColor.White;
            //第一步 获取ip地址并尝试连接,连接成功进入下一步.
            while (true)
            {
                string str = Console.ReadLine();
                if (str.Length == 0 || str.Substring(0, 1) == "d")
                {
                    if (ConnectMiutiMode("127.0.0.1", 2333))
                    {
                        //开始多人游戏
                        Console.WriteLine("连接成功");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("连接发生错误,按任意键返回");
                        Console.ReadKey();
                        return Result.cancel;
                    }
                }
                else if (str.Substring(0, 1) == "q")
                {
                    return Result.cancel;
                }
                else
                {
                    if (ConnectMiutiMode(str, 2333))
                    {
                        //开始多人游戏
                        Console.WriteLine("连接成功");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("连接发生错误,按任意键返回");
                        Console.ReadKey();
                        return Result.cancel;
                    }
                }
            }
            //创建发送心跳线程
            Thread thread = new Thread(Send);
            thread.IsBackground = true;
            thread.Start();
            //启动接收线程
            Thread thread2 = new Thread(Receive);
            thread2.IsBackground = true;
            thread2.Start();
            Console.WriteLine("-------------开始选人-------------");
            Console.WriteLine("角色列表:1岩山(坦克/控制)2圣者(坦克/治疗)3烈焰(输出)4影刺(输出)5冰封(输出/控制)6愈灵(治疗)");
            Console.WriteLine("请选择要使用的角色,可以选择3个单位");
            Console.WriteLine("输入三个不同的数字之后按Enter.");

            string selectStr = "f";
            //第二步,尝试获取选人信息,选择成功时向服务器发包汇报选人信息,之后进入下一步.
            while (true)
            {
                string selectPlayer = Console.ReadLine();
                //这个其实应该用反射 依然返回bool
                if ((selectStr = scene.playerController.SelectPlayer(selectPlayer, 3)) != "f")
                {
                    //这个选择是有效的,可以进入下一步了
                    break;
                }
                else if (selectStr.Length == 0)
                {
                    Console.WriteLine("自动选择了456");
                    scene.playerController.SelectPlayer("456", 3);
                    break;
                }
                else
                {
                    Console.WriteLine("输入格式有误,请重新尝试");
                }
            }

            //发送一个登陆包 附带选择的三个角色
            var writer = MyTCPClient.Instance.GetSendStream(GameAction.Game, GameCmd.LoginGame);
            foreach (var v in selectStr)
            {
                writer.Write((byte)v);
            }
            MyTCPClient.Instance.Send(writer);

            Console.WriteLine("已发送,正在等待接收服务器返回信息");

            //等待接收登陆信息 获取服务器提供的另一个player的三个角色信息.
            while (true)
            {
                //如果接收过服务器返回信息
                if (netPlayer != Scene.NetPlayer.single)
                {
                    break;
                }
                //if(otherPlayerSelect != "000")
                //{
                //    Console.WriteLine("正在等待另一个玩家的选择");
                //}
                Thread.Sleep(100);
            }

            //初始化游戏 重新初始化一个新的场景作为多人生成场景
            scene = new Scene(Scene.GameMode.miuti, netPlayer);


            //初始化自己的角色
            scene.playerController.SelectPlayer(selectStr, 3);

            //初始化另一个角色
            scene.netController.SelectPlayer(otherPlayerSelect);

            //选择一下自己属于?P

            Console.WriteLine("初始化全部准备完毕.请按任意键开始游戏");
            Console.ReadKey();

            scene.AddDebugMessage("欢迎进入游戏,本局游戏你是" + scene.netPlayer);

            /*快两天过去了...终于可以准备开始写包了..
             *进行初始化的时候添加一个NetController.从网络上获取这次操作的信息.
             *搞一个队列.NetC不停的检测这个队列中有没有进新的操作.检测到就发送新的操作.
             *然后回调接收的时候调用相关方法将序列转换为本次操作的操作符
             *现在是P1的回合 等待服务器广播通知,接收到通知后才能进入PC中->
             *              PlayerC进行了一步操作 发一次包 
             *
            1P playerC发包->服务器收包->发给2P NetC->NetC进行游戏同步操作然后返回一个消息给服务器
            ->服务器接收进行一定的有效判断在发给1P确认->1P才能进行下一步操作...
            简单版本:
            1P 每操作一次就发一个包,服务器直接转发给2P.2P每收一个包就同步一下.
            只有回合开始结束的时候需要双向确认. 
             */
            return Result.confirm;
        }
        #endregion

        public Result LoadWinScene()
        {
            Console.Clear();

            List<Pos> listP = new List<Pos>()
            {new Pos(4, 2),new Pos(4, 3)};
            List<string> listS = new List<string>()
            {scene.canvas.ShowString("1.重新开始",10),
             scene.canvas.ShowString("2.退出游戏",10)};
            int selected = 0;


            Console.WriteLine("恭喜完成游戏!!!用时" + turns.ToString() + "回合");

            while (true)
            {
                Show();
                ConsoleKeyInfo ck = Console.ReadKey();
                if (ck.Key == ConsoleKey.DownArrow)
                {
                    selected++;
                    if (selected == 2)
                        selected = 0;
                }
                else if (ck.Key == ConsoleKey.UpArrow)
                {
                    selected--;
                    if (selected == -1)
                        selected = 1;
                }
                else if (ck.Key == ConsoleKey.Enter)
                {
                    if (selected == 0)
                    {
                        return Result.confirm;
                    }
                    else if (selected == 1)
                    {
                        return Result.cancel;
                    }
                }
            }

            void Show()
            {
                Console.BackgroundColor = ConsoleColor.Black;
                for (int i = 0; i < 2; i++)
                {
                    if (i == selected)
                    {
                        Console.BackgroundColor = ConsoleColor.Cyan;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    Console.SetCursorPosition(listP[i].x, listP[i].y);
                    Console.Write(listS[i]);
                }
            }
        }
        public Result LoadGameOverScene()
        {
            Console.Clear();
            List<Pos> listP = new List<Pos>()
            {new Pos(4, 2),new Pos(4, 3)};
            List<string> listS = new List<string>()
            {scene.canvas.ShowString("1.重新开始",10),
             scene.canvas.ShowString("2.退出游戏",10)};
            int selected = 0;

            StreamReader sr = FileController.GetFileReader("DeadScene.txt");
            string str = "";
            while ((str = sr.ReadLine()) != null)
            {
                Console.WriteLine(str);
            }

            while (true)
            {
                Show();
                ConsoleKeyInfo ck = Console.ReadKey();
                if (ck.Key == ConsoleKey.DownArrow)
                {
                    selected++;
                    if (selected == 2)
                        selected = 0;
                }
                else if (ck.Key == ConsoleKey.UpArrow)
                {
                    selected--;
                    if (selected == -1)
                        selected = 1;
                }
                else if (ck.Key == ConsoleKey.Enter)
                {
                    if (selected == 0)
                    {
                        return Result.confirm;
                    }
                    else if (selected == 1)
                    {
                        return Result.cancel;
                    }
                }
            }

            void Show()
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(1, 1);
                Console.Write(scene.canvas.ShowString("坚持了" + turns + "个回合", 10));
                for (int i = 0; i < 2; i++)
                {
                    if (i == selected)
                    {
                        Console.BackgroundColor = ConsoleColor.Cyan;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    Console.SetCursorPosition(listP[i].x, listP[i].y);
                    Console.Write(listS[i]);
                }
            }

        }
        public Result LoadSelectScene()
        {
            Console.Clear();
            StreamReader sr = FileController.GetFileReader("Help.txt");
            string str = "";
            while ((str = sr.ReadLine()) != null)
            {
                if (str.Length > 0)
                {
                    char c = str[0];
                    if (c == '#')
                    {
                        switch (str[1])
                        {
                            case 'B':
                                Console.ForegroundColor = ConsoleColor.Blue;
                                break;
                            case 'R':
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            default:
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                        }
                        str = str.Substring(2);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                Console.WriteLine(str);
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("使用鼠标滚轮上下查看,按Enter返回主菜单");
            Console.ForegroundColor = ConsoleColor.White;
            while (true)
            {
                Console.WriteLine("请选择要选择的角色: 可以选择五个");
                Console.WriteLine("输入五个不同的数字之后按Enter.");
                string selectPlayer = Console.ReadLine();
                if (selectPlayer == "q")
                {
                    return Result.cancel;
                }
                if (scene.playerController.SelectPlayer(selectPlayer, 5) != "f")
                {
                    Console.Clear();
                    return Result.confirm;
                }
                else
                {
                    Console.WriteLine("输入格式有误,请重新尝试");
                }
            }
        }
        //显示开始界面
        public Result LoadStartScene()
        {

            List<Pos> listP = new List<Pos>() { new Pos(12, 5),
                new Pos(12, 6), new Pos(12, 7), new Pos(12, 8),new Pos(12, 9) };
            List<string> listS = new List<string>()
            {scene.canvas.ShowString("1.开始游戏简单",15),scene.canvas.ShowString("2.开始游戏困难",15),
            scene.canvas.ShowString("3.开始多人游戏",15),
                scene.canvas.ShowString("4.查看成就",15),scene.canvas.ShowString("5.退出游戏",15)};
            List<Result> listR = new List<Result>()
            {
                Result.startSingleEasy,Result.startSingleHard,Result.startMiuti,
                Result.startAchivement,Result.startQuit
            };


            int selected = 1;

            Console.Clear();
            StreamReader sr = FileController.GetFileReader("StartScene.txt");
            string str = "";
            while ((str = sr.ReadLine()) != null)
            {
                Console.WriteLine(str);
            }

            while (true)
            {
                Show();
                ConsoleKeyInfo ck = Console.ReadKey();
                if (ck.Key == ConsoleKey.DownArrow)
                {
                    selected++;
                    if (selected == 5)
                        selected = 0;
                }
                else if (ck.Key == ConsoleKey.UpArrow)
                {
                    selected--;
                    if (selected == -1)
                        selected = 4;
                }
                else if (ck.Key == ConsoleKey.Enter)
                {
                    return listR[selected];
                }
            }
            void Show()
            {
                for (int i = 0; i < 5; i++)
                {
                    if (i == selected)
                    {
                        Console.BackgroundColor = ConsoleColor.Cyan;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    Console.SetCursorPosition(listP[i].x, listP[i].y);
                    Console.Write(listS[i]);
                }
            }
        }


    }

}

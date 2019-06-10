using System;
using System.Collections.Generic;
using System.IO;

namespace WizardAdvanture
{
    public class CanvasPoint
    {
        private static ConsoleColor defaultBackColor = ConsoleColor.Black;
        private static ConsoleColor defaultForceColor = ConsoleColor.White;
        public char pic;
        public ConsoleColor backColor;
        public ConsoleColor forceColor;
        //什么都不给,返回默认空白
        public CanvasPoint()
        {
            pic = ' ';
            backColor = defaultBackColor;
            forceColor = defaultForceColor;
        }
        //给一个贴图,返回默认色的点
        public CanvasPoint(char pic)
        {
            this.pic = pic;
            backColor = defaultBackColor;
            forceColor = defaultForceColor;
        }
        //完整的构造函数
        public CanvasPoint(char pic, ConsoleColor forceColor)
        {
            this.pic = pic;
            this.forceColor = forceColor;
        }

        //完整的构造函数
        public CanvasPoint(char pic, ConsoleColor forceColor, ConsoleColor backColor)
        {
            this.pic = pic;
            this.backColor = backColor;
            this.forceColor = forceColor;
        }
        //判断两个像素点是否相等
        public override bool Equals(Object o)
        {
            CanvasPoint p = (CanvasPoint)o;
            return pic == p.pic && backColor == p.backColor
                && forceColor == p.forceColor;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
    public class Canvas
    {
        #region 双缓存变量
        int height, width;
        CanvasPoint[,] backBuffer;
        public CanvasPoint[,] buffer;
        CanvasPoint[,] uibuffer;
        CanvasPoint empty = new CanvasPoint();
        #endregion

        #region UI变量
        //地图显示位置
        private Pos mapPos;
        //状态显示位置
        private Pos statePos;
        //回合数信息
        private Pos turnsPos;
        //姓名版信息
        private Pos namePos, hpPos;
        private string turns = "00";
        public void SetTurns(int n)
        {
            turns = ShowString(n, 2);
        }
        //Debug信息框
        private Pos DebugLogPos;
        private List<string> debugMessage = new List<string>();
        public void AddDebugMessage(string s)
        {
            string str = "";
            str += s;
            for (int i = 0; i < 20 - s.Length; i++)
            {
                str = str + " ";
            }
            str = ShowString(str, 20);
            debugMessage.Add(str);
            if (debugMessage.Count > 14)
            {
                debugMessage.RemoveAt(0);
            }
        }

        //目标信息框
        private Pos targetPos;
        private Pos immediatePos;
        private string immediateMessage = "#Cq:返回 方向键:移动 1-4:释放技能 esc:结束回合";
        public void ShowImmediateMessage(string str)
        {
            immediateMessage = ShowString(str, 40);
        }

        StreamReader sr;
        #endregion

        #region 字符串处理
        //用来调整数字的格式化显示
        public string ShowString(int n, int length)
        {
            string str = "";
            int nlength = n.ToString().Length;
            if (nlength < length)
            {
                int need = length - n.ToString().Length;
                for (int i = 0; i < need; i++)
                {
                    str += "0";
                }
                str += n.ToString();
            }
            else if (nlength == length)
            {
                str = n.ToString();
            }
            else
            {
                for (int i = 0; i < length; i++)
                {
                    str += "9";
                }
            }
            return str;
        }

        //格式化显示字符串
        public string ShowString(string str, int length)
        {
            int sl = 0;
            foreach (var v in str.ToCharArray())
            {
                if (IsChinese(v))
                    sl += 2;
                else
                    sl += 1;
            }
            if (str.Length <= length)
            {
                int need = length - sl;
                for (int i = 0; i < need; i++)
                {
                    str += " ";
                }
            }
            else
            {
                str = str.Substring(0, length);
            }
            return str;
        }

        //判断一个char是不是一个汉字汉字是占2个位置的
        public bool IsChinese(char c)
        {
            //只要不低于127都算chinese算了
            if (c >= 0x4e00 && c <= 0x9fbb)
                return true;
            else
                return false;
        }


        //这个相当于console.write(); 将字符串写入点p位置
        public void WriteBuffer(Pos p, string str)
        {
            int x = p.x, y = p.y;
            char[] cs = str.ToCharArray();

            for (int i = 0; i < str.Length; i++)
            {
                if (IsChinese(cs[i]))
                {
                    buffer[x, y] = new CanvasPoint(cs[i], color);
                    buffer[x + 1, y] = new CanvasPoint(' ', buffer[x, y].backColor, buffer[x, y].forceColor);
                    x = x + 2;
                }
                else
                {
                    buffer[x, y] = new CanvasPoint(cs[i], color);
                    x++;
                }
            }
        }
        #endregion

        Scene scene;
        //摄像机起始点
        public Pos startPos;
        //距离边缘有多远的时候开始移动摄像机
        public int limitToMove = 5;
        public int range = 20;


        public void InitUI()
        {
            sr = FileController.GetFileReader("UI.txt");
            string str;
            int n = 0;
            while ((str = sr.ReadLine()) != null)
            {
                char[] cs = str.ToCharArray();
                width = str.Length;
                for (int i = 0; i < cs.Length; i++)
                {
                    if (cs[i] == '#')
                    {
                        switch (cs[i + 1])
                        {
                            case 'M':
                                mapPos = new Pos(i, n);
                                break;
                            case 'A':
                                turnsPos = new Pos(i, n);
                                break;
                            case 'B':
                                statePos = new Pos(i, n);
                                break;
                            case 'C':
                                targetPos = new Pos(i, n);
                                break;
                            case 'D':
                                DebugLogPos = new Pos(i, n);
                                break;
                            case 'E':
                                immediatePos = new Pos(i, n);
                                break;
                            case 'N':
                                namePos = new Pos(i, n);
                                break;
                            case 'H':
                                hpPos = new Pos(i, n);
                                break;
                        }
                    }
                }
                n++;
            }
            //给留空10格
            width += 30;
            height = n + 1;
            buffer = new CanvasPoint[width, height];
            backBuffer = new CanvasPoint[width, height];
            uibuffer = new CanvasPoint[width, height];
            //初始化两个buffer
            ClearBuffer();
            ClearUIBuffer();
            //初始化,将边框画在uibuffer里存起来
            sr.Close();
            sr = FileController.GetFileReader("UI.txt");
            n = 0;
            while ((str = sr.ReadLine()) != null)
            {
                char[] cs = str.ToCharArray();
                for (int i = 0; i < cs.Length; i++)
                {
                    if (cs[i] == '#')
                    {
                        //把i往后扔一位然后填两个空格
                        uibuffer[i, n] = new CanvasPoint();
                        i++;
                        uibuffer[i, n] = new CanvasPoint();
                    }
                    else
                    {
                        uibuffer[i, n] = new CanvasPoint(cs[i]);
                    }
                }
                n++;
            }
            sr.Close();

            DrawCoordinate(0, 0);
        }

        public void DrawCoordinate(int x, int y)
        {
            //坐标轴往左上角移一位
            x--;
            y--;
            //画坐标轴
            for (int i = 0; i < 21; i++)
            {
                int first = (i + x) / 10;
                int next = (i + x) % 10;
                if ((i + x) % 2 == 0)
                {
                    //第一个字符
                    uibuffer[2 * i, 0] = new CanvasPoint(first.ToString()[0], ConsoleColor.Black, ConsoleColor.Blue);
                    //第二个字符
                    uibuffer[2 * i + 1, 0] = new CanvasPoint(next.ToString()[0], ConsoleColor.Black, ConsoleColor.Blue);
                }
                else
                {
                    //第一个字符
                    uibuffer[2 * i, 0] = new CanvasPoint(first.ToString()[0], ConsoleColor.Black, ConsoleColor.Red);
                    //第二个字符
                    uibuffer[2 * i + 1, 0] = new CanvasPoint(next.ToString()[0], ConsoleColor.Black, ConsoleColor.Red);
                }
            }
            for (int i = 0; i < 21; i++)
            {
                int first = (i + y) / 10;
                int next = (i + y) % 10;
                if ((i + y) % 2 == 0)
                {
                    //第一个字符
                    uibuffer[0, i] = new CanvasPoint(first.ToString()[0], ConsoleColor.Black, ConsoleColor.Blue);
                    //第二个字符
                    uibuffer[1, i] = new CanvasPoint(next.ToString()[0], ConsoleColor.Black, ConsoleColor.Blue);
                }
                else
                {
                    //第一个字符
                    uibuffer[0, i] = new CanvasPoint(first.ToString()[0], ConsoleColor.Black, ConsoleColor.Red);
                    //第二个字符
                    uibuffer[1, i] = new CanvasPoint(next.ToString()[0], ConsoleColor.Black, ConsoleColor.Red);
                }
            }
        }

        private ConsoleColor color;
        private string StringToSetColor(string str)
        {
            char c = str[0];
            if (c == '#')
            {
                switch (str[1])
                {
                    case 'D':
                        color = ConsoleColor.DarkBlue;
                        break;
                    case 'W':
                        color = ConsoleColor.DarkRed;
                        break;
                    case 'B':
                        color = ConsoleColor.Blue;
                        break;
                    case 'R':
                        color = ConsoleColor.Red;
                        break;
                    case 'C':
                        color = ConsoleColor.Cyan;
                        break;
                    case 'Y':
                        color = ConsoleColor.Yellow;
                        break;
                    case 'M':
                        color = ConsoleColor.Magenta;
                        break;
                    case 'G':
                        color = ConsoleColor.Green;
                        break;
                    default:
                        color = ConsoleColor.White;
                        break;
                }
                return str.Substring(2);
            }
            else
            {
                color = ConsoleColor.White;
                return str;
            }
        }

        public Block block = BlockFactory.CreateGround();
        //绘制UI界面
        public void DrawUI()
        {
            //画坐标轴
            DrawCoordinate(startPos.x, startPos.y);
            // 先把保存好的框架画上去
            CopyArray2D(uibuffer, buffer);
            //显示回合数信息
            WriteBuffer(turnsPos, StringToSetColor(turns));
            WriteBuffer(immediatePos, StringToSetColor(immediateMessage));
            //显示debug信息
            int n = 0;
            foreach (var v in debugMessage)
            {
                WriteBuffer(DebugLogPos + new Pos(0, n), StringToSetColor(v));
                n++;
            }

            //画目标点信息
            WriteBuffer(targetPos, StringToSetColor(ShowString("#Y地形:" + BlockFactory.GetBlockString(block), 25)));
            Target t;
            if ((t = block.target) != null)
            {
                //如果是boss则先同步一下血量
                if (t.GetType() == typeof(Boss))
                {
                    Boss b = (Boss)t;
                    b.hpMax = b.HpMax; b.hp = b.Hp;
                }
                WriteBuffer(hpPos, StringToSetColor(ShowString("#C" + t.hp.ToString(), 6)));
                WriteBuffer(hpPos + new Pos(5, 0), StringToSetColor(ShowString("#C" + t.hpMax.ToString(), 6)));
                //百分比显示血条
                int d = (int)((double)t.hp / t.hpMax * 25);
                string stt = "";
                for (int i = 0; i < d; i++)
                {
                    stt += '>';
                }
                if (d <= 5)
                {
                    color = ConsoleColor.Red;
                }
                else if (d <= 15)
                {
                    color = ConsoleColor.Yellow;
                }
                else
                {
                    color = ConsoleColor.Green;
                }
                WriteBuffer(targetPos + new Pos(0, 2), ShowString(stt, 25));

                if (t.GetType() == typeof(PlayerCharacter))
                {
                    if (t.belongsTo == Target.BelongsTo.PlayerSelf)
                        WriteBuffer(namePos, StringToSetColor(ShowString("#B" + t.name.ToString(), 8)));
                    else if (t.belongsTo == Target.BelongsTo.PlayerNet)
                        WriteBuffer(namePos, StringToSetColor(ShowString("#C" + t.name.ToString() + "[友]", 8)));
                    WriteBuffer(targetPos + new Pos(0, 4), StringToSetColor(ShowString("#BMP: " + t.mp.ToString() + "/" + t.mpMax.ToString(), 24)));

                    //百分比显示血条
                    double acts = ((double)t.act / t.actMax);
                    if (acts <= 0.1)
                    {
                        color = ConsoleColor.Red;
                    }
                    else if (acts < 1)
                    {
                        color = ConsoleColor.Yellow;
                    }
                    else
                    {
                        color = ConsoleColor.Green;
                    }
                    WriteBuffer(targetPos + new Pos(0, 5), ShowString("ACT: " + t.act.ToString() + "/" + t.actMax.ToString() + "[" + t.moveAct.ToString() + "]", 25));

                    for (int i = 0; i < 4; i++)
                    {
                        if (t.skills[i].name == "void")
                        {
                            WriteBuffer(targetPos + new Pos(0, 6 + i), StringToSetColor(ShowString(" ", 24)));
                        }
                        else if (t.skills[i].name == "休息")
                        {
                            int heal = t.skills[i].heal * t.act;
                            int rm = t.skills[i].addMana * t.act;
                            string message = "休息[" + t.act.ToString() + "]";
                            if (heal != 0)
                            {
                                message += "回复" + heal.ToString() + "HP";
                            }
                            if (rm != 0)
                            {
                                message += rm.ToString() + "MP";
                            }
                            WriteBuffer(targetPos + new Pos(0, 6 + i), StringToSetColor(ShowString("#G" + message, 25)));
                        }
                        else
                        {
                            if (t.skills[i].costMana > t.mp || t.skills[i].costAct > t.act)
                            {
                                color = ConsoleColor.Red;
                            }
                            else
                            {
                                color = ConsoleColor.Green;
                            }
                            WriteBuffer(targetPos + new Pos(0, 6 + i), ShowString(t.skills[i].name + " 消耗: " + t.skills[i].costAct.ToString() + "|" + t.skills[i].costMana.ToString(), 24));
                        }

                    }
                    WriteBuffer(targetPos + new Pos(0, 10), StringToSetColor(ShowString("#C" + t.discription1, 20)));
                    WriteBuffer(targetPos + new Pos(0, 11), StringToSetColor(ShowString("#C" + t.discription2, 20)));
                }
                else
                {
                    WriteBuffer(namePos, StringToSetColor(ShowString("#R" + t.name.ToString(), 8)));
                    if (t.belongsTo == Target.BelongsTo.OtherEnemy)
                        WriteBuffer(targetPos + new Pos(0, 5), StringToSetColor(ShowString("#R" + "仇恨目标" + t.focus.name, 20)));
                }

            }


            else
            {
                for (int i = 0; i < 6; i++)
                    WriteBuffer(namePos + new Pos(0, 4), ShowString(" ", 25));
            }

            //画行动力信息
            //statepos
            string str = "";
            foreach (var v in scene.playerController.lists)
            {
                if (v.act >= v.actMax)
                    str += v.pic;
            }
            WriteBuffer(statePos, StringToSetColor("#B可行动:" + ShowString(str, 15)));


        }

        /// <summary>
        /// 显示地图信息并显示地图坐标
        /// </summary>
        public void ShowCamera()
        {
            //坐标轴一会画canvas上
            for (int i = 0; i < range; i++)
            {
                for (int j = 0; j < range; j++)
                {
                    int cx = 2 + j * 2, cy = 1 + i;
                    Block b = scene.SelectBlock(startPos + new Pos(j, i));
                    Console.SetCursorPosition(cx, cy);
                    Console.ForegroundColor = b.ForceColor;
                    Console.BackgroundColor = b.BackColor;
                    Console.Write(b.Pic);
                }
            }
        }

        public Canvas(Scene scene, Pos cameraPos)
        {
            this.scene = scene;
            startPos = cameraPos;
            //初始化 确定长宽 获取点位
            //InitUI();
        }
        //移动摄像机 绝对位置
        public void MoveCameraTo(Pos p)
        {
            startPos = startPos + p;
            if (startPos.x < 0)
                startPos.x = 0;
            if (startPos.x + range > scene.mapX)
                startPos.x = scene.mapX - range;
            if (startPos.y < 0)
                startPos.y = 0;
            if (startPos.y + range > scene.mapY)
                startPos.y = scene.mapY - range;
        }

        public void Refresh()
        {
            // 清除缓存
            ClearBuffer_DoubleBuffer();
            //画UI
            DrawUI();
            //画map
            ShowCamera();
            // 画canvas
            Refresh_DoubleBuffer();
        }
        //轻量处理,只刷新UI,不刷新Map
        public void RefreshUI()
        {
            // 清除缓存
            ClearBuffer_DoubleBuffer();
            //画UI
            DrawUI();
            // 画canvas
            Refresh_DoubleBuffer();
        }
        #region 双缓存辅助方法
        //初始化buffer
        public void ClearBuffer()
        {
            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    buffer[j, i] = empty;
                }
            }
        }

        //初始化buffer
        public void ClearUIBuffer()
        {
            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    uibuffer[j, i] = empty;
                }
            }
        }
        //清除缓存
        public void ClearBuffer_DoubleBuffer()
        {
            CopyArray2D(buffer, backBuffer);
            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    buffer[i, j] = empty;
                }
            }
        }

        public void Refresh_DoubleBuffer()
        {
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    if (!buffer[i, j].Equals(backBuffer[i, j]))
                    {
                        Console.SetCursorPosition(i, j);
                        Console.BackgroundColor = buffer[i, j].backColor;
                        Console.ForegroundColor = buffer[i, j].forceColor;
                        Console.Write(buffer[i, j].pic);
                        if (IsChinese(buffer[i, j].pic))
                            i++;
                    }
                }
            }
        }

        void CopyArray2D(CanvasPoint[,] source, CanvasPoint[,] dest)
        {
            for (int i = 0; i < source.GetLength(0); ++i)
            {
                for (int j = 0; j < source.GetLength(1); ++j)
                {
                    dest[i, j] = source[i, j];
                }
            }
        }
        #endregion
    }
}

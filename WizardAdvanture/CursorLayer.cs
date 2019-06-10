using System;
using System.Collections.Generic;

// Layer集合
namespace WizardAdvanture
{
    //真光标对象,通过初始化不同的光标改变光标形状 和scene无关
    public class Cursor
    {
        public Pos centerPos;
        private Scene scene;
        //相对中心点的相对向量
        public List<Pos> cursorAera = new List<Pos>();
        private ConsoleColor cursorColor = ConsoleColor.DarkRed;

        public Cursor(Scene scene)
        {
            this.scene = scene;
        }
        //重新创建一个一个点的光标对象
        public static Cursor CreatePointCursor(Scene scene)
        {
            Cursor cursor = new Cursor(scene);
            return cursor;
        }
        //重新创建一个十字光标对象
        public static Cursor CreateCrossCursor(Scene scene)
        {
            Cursor cursor = new Cursor(scene);
            cursor.cursorAera = new List<Pos>() { new Pos(1, 0),
            new Pos(-1,0),new Pos(0,1),new Pos(0,-1)};
            return cursor;
        }
        //重新创建一个方块光标对象
        public static Cursor CreateCrossSquare(Scene scene)
        {
            Cursor cursor = new Cursor(scene);
            cursor.cursorAera = new List<Pos>() { new Pos(1, 0),
            new Pos(-1,0),new Pos(0,1),new Pos(0,-1),new Pos(1, -1),
            new Pos(-1,1),new Pos(1,1),new Pos(-1,-1)};
            return cursor;
        }
        //竖线光标
        public static Cursor CreateLLine(Scene scene)
        {
            Cursor cursor = new Cursor(scene);
            cursor.cursorAera = new List<Pos>() { new Pos(0, 1),
            new Pos(0,2),new Pos(0,-1),new Pos(0,-2)};

            return cursor;
        }
        //横线光标
        public static Cursor CreateSLine(Scene scene)
        {
            Cursor cursor = new Cursor(scene);
            cursor.cursorAera = new List<Pos>() { new Pos(1, 0),
            new Pos(2,0),new Pos(-1,0),new Pos(-2,0)};
            return cursor;
        }




        public Block SelectBlock(Pos p)
        {
            return scene.SelectBlock(p);
        }
        //移动光标 p为变化的向量,不是绝对位置
        public void MoveCursor(Pos p)
        {
            SelectBlock(centerPos).BackColor = ConsoleColor.Black;
            centerPos = centerPos + p;
            SelectBlock(centerPos).BackColor = cursorColor;

            //显示指针指向的目标信息

            Pos otherPos;
            foreach (var v in cursorAera)
            {
                otherPos = centerPos + v;

                SelectBlock(otherPos - p).BackColor = ConsoleColor.Black;
                SelectBlock(otherPos).BackColor = cursorColor;
                //不需要处理指向信息
            }
        }
        //清除光标显示
        public void ClearCursor()
        {
            SelectBlock(centerPos).BackColor = ConsoleColor.Black;
            foreach (var v in cursorAera)
            {
                SelectBlock(centerPos + v).BackColor = ConsoleColor.Black;
            }
        }
    }
    //光标层 用来处理光标指示
    public class CursorLayer
    {
        public Cursor cursor;
        public Pos CursorPos
        {
            get
            {
                return cursor.centerPos;
            }
            set { cursor.centerPos = value; }
        }

        private Scene scene;
        private Block[,] blocks;


        private List<Pos> moveAera = new List<Pos>();
        private ConsoleColor moveColor = ConsoleColor.DarkBlue;
        private List<Pos> routeAera = new List<Pos>();
        private ConsoleColor routeColor = ConsoleColor.DarkYellow;

        public CursorLayer(Scene s, Pos p)
        {
            //这种丧心病狂的写法...感觉打开了什么新世界的大门 
            //感觉是什么设计模式但是是我新琢磨出来的...
            //先保存一个先保存自己的scene 再传给Cursor 这样只要改动主类就
            //可以了 子类可以后台替换掉...
            scene = s;

            cursor = Cursor.CreatePointCursor(scene);
            CursorPos = p;

            MoveCursor(new Pos(0, 0));


            blocks = scene.blocks;

            CursorPos = p;
        }
        //重载光标形状
        public void ReloadCurser(Scene.CursorType type)
        {
            Pos p = cursor.centerPos;
            cursor.ClearCursor();
            switch (type)
            {
                case Scene.CursorType.Point:
                    cursor = Cursor.CreatePointCursor(scene);
                    break;
                case Scene.CursorType.Cross:
                    cursor = Cursor.CreateCrossCursor(scene);
                    break;
                case Scene.CursorType.Sline:
                    cursor = Cursor.CreateSLine(scene);
                    break;
                case Scene.CursorType.Lline:
                    cursor = Cursor.CreateLLine(scene);
                    break;
                case Scene.CursorType.Square:
                    cursor = Cursor.CreateCrossSquare(scene);
                    break;
            }
            cursor.centerPos = p;
        }
        //对外使用的移动光标方法
        public void MoveCursor(Pos p)
        {
            cursor.MoveCursor(p);
            //刷新一下光标 确保不要错色.
            //cursor.MoveCursor(new Pos(0, 0));
        }

        //重新加载可移动区域
        public void ResetMoveAera(List<Pos> aera)
        {
            foreach (var v in moveAera)
            {
                SelectBlock(v).BackColor = ConsoleColor.Black;
            }
            moveAera = aera;
            foreach (var v in aera)
            {
                SelectBlock(v).BackColor = moveColor;
            }
        }
        //重新加载路径区域
        public void ResetRouteAera(List<Pos> aera)
        {
            foreach (var v in routeAera)
            {
                SelectBlock(v).BackColor = ConsoleColor.Black;
            }
            ResetMoveAera(moveAera);

            routeAera = aera;
            foreach (var v in aera)
            {
                SelectBlock(v).BackColor = routeColor;
            }
        }

        public void RefreshMoveAera()
        {
            ResetMoveAera(moveAera);
        }
        public Block SelectBlock(Pos p)
        {
            return scene.SelectBlock(p);
        }

    }
}

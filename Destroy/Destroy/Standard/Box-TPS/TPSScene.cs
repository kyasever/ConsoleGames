namespace Destroy.Standard
{
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// 一个用于展示的场景
    /// </summary>
    public class TPSScene : Scene
    {
        /// <summary>
        /// 用new 新建这个场景
        /// </summary>
        public TPSScene() : base("PushBox")
        {

        }

        /// <summary>
        /// 在场景中创建对象
        /// </summary>
        public override void OnStart()
        {
            //主角位于原点
            GameObject player = TPSFactroy.CreateStandardPlayer("吊", new Vector2(0, 0));
            GameObject.CreateWith<CameraController>().followTrans = player;
            player.AddComponent<Shooter>();

            //创建迷宫地图 位于第三象限
            var maze = MazeGeneration.GetMaze(21, 21, new RenderPoint("■", Color.White, Config.DefaultBackColor, (int)Layer.Environment));
            maze.Position = new Vector2(-25, -25);

            //俄罗斯方块位于第一象限
            for(int x = 5;x < 20;x+=5)
            {
                for(int y = 5;y<20;y+=5)
                {
                    TPSFactroy.CreateBox(new Vector2(x, y));
                }
            }

            //系统指示器 位于右上角
            SystemUIFactroy.GetSystemInspector();

            //左侧的三个UI组件
            TPSFactroy.CreateTestLable(new Vector2(0, 26));

            ListBox gunBox =  UIFactroy.CreateListBox(new Vector2(0, 15), 5, 10);
            gunBox.AddComponent<GunBox>();

            var gunUI = UIFactroy.CreateTextBox(new Vector2(0, 21), 3, 10);
            gunUI.AddComponent<GunUI>();
        }
    }

    /// <summary>
    /// 枪列表
    /// </summary>
    public class GunBox : Component
    {
        public int CurrertAM
        {
            get
            {
                return listBoxCom.CurrentItem.GetComponent<Gun>().AM;
            }
            set
            {
                listBoxCom.CurrentItem.GetComponent<Gun>().AM = value;
            }
        }

        /// <summary>
        /// 保存了一个listBox对象
        /// </summary>
        private ListBox listBoxCom;
        /// <summary>
        /// 保存了一个单例,这个类主要就是为了这个了
        /// </summary>
        public static GunBox Instance;

        internal override void Initialize()
        {
            Instance = this;
            listBoxCom = GetComponent<ListBox>();
        }

        internal override void OnAdd()
        {
            for(int i = 0;i<5;i++)
            {
                ListBoxItem gunItem = listBoxCom.CreateLabelItem("手枪"+i.ToString());
                var gun = gunItem.AddComponent<Gun>();
                gun.AM = 6;

                gunItem = listBoxCom.CreateLabelItem("步枪" + i.ToString());
                gun = gunItem.AddComponent<Gun>();
                gun.AM = 16;
            }
        }
    }

    /// <summary>
    /// 枪 位于枪列表的一个底下的组件,代表这把武器的状态
    /// </summary>
    public class Gun : Component
    {
        /// <summary>
        /// 剩余弹药数量
        /// </summary>
        public int AM = 0;
    }

    /// <summary>
    /// 血量和剩余弹药指示UI
    /// </summary>
    public class GunUI : Script
    {
        private TextBox textBox;

        /// <summary>
        /// 
        /// </summary>
        public override void Awake()
        {
            textBox = GetComponent<TextBox>();
            textBox.SetText("lanmbo", 1);
            textBox.SetText("HP:>>>>>>", 2);
            textBox.SetText("AM: >>>>>>>>", 3);
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Update()
        {
            int am = GunBox.Instance.CurrertAM;
            ColorStringBuilder colorStringBuilder = new ColorStringBuilder();
            colorStringBuilder.ForeColor = Color.Blue;
            colorStringBuilder.AppendString("AM: ");

            if(am > 0)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < am; i++)
                {
                    sb.Append('>');
                }
                colorStringBuilder.ForeColor = Color.Green;
                colorStringBuilder.AppendString(sb.ToString());
            }
            else
            {
                colorStringBuilder.ForeColor = Color.Red;
                colorStringBuilder.AppendString("EMPTY");
                //这里展示了组件化的好处(坏处)如何找一个要找的物体
                /*
                 * Gunbox.Instance 获取Gunbox这个脚本的引用.这个脚本有一个单例引用
                 * .GetComponent<ListBox>() 获取这个物体上的ListBox组件
                 * .CurrentItem 组件当前正在被选区的对象
                 * .GetComponent<Renderer>() 由于更改颜色是一个组件本身不提供的功能,这个功能位于同一物体的Renderer组件中
                 * .SetForeColor(Color.Red); 调用这个子物体的Renderer组件完成变色
                 * 这是一个其他的例子
                 * GunBox.Instanse..GetComponent<ListBox>().CurrentItem.GetComponent<Gun>().AM = 0;
                 */
                GunBox.Instance.GetComponent<ListBox>().CurrentItem.GetComponent<Renderer>().SetForeColor(Color.Red);
            }
            textBox.Labels[2].Rendering(colorStringBuilder.ToRenderer());
        }
    }


    /// <summary>
    /// 创建一个典型的组件的实例
    /// </summary>
    public static class TPSFactroy
    {
        /// <summary>
        /// 创建一个标准单格主角
        /// </summary>
        public static GameObject CreateStandardPlayer(string str, Vector2 pos)
        {
            GameObject player = new GameObject("StandardPlayer","Player")
            {
                Position = pos
            };

            player.AddComponent<Mesh>();

            Collider mc = player.AddComponent<Collider>();

            Renderer renderer = player.AddComponent<Renderer>();
            renderer.Rendering(str);
            renderer.Depth = 5;

            CharacterController controller = player.AddComponent<CharacterController>();
            controller.CanMoveInCollider = false;
            return player;
        }

        /// <summary>
        /// 创建一个基于物理系统的标准箱子
        /// </summary>
        public static GameObject CreateBox(Vector2 pos)
        {
            int r = GameMode.Random.Next(0, 3);

            GameObject box = new GameObject("箱子" + r.ToString(),"Box")
            {
                Position = pos
            };

            Mesh mesh = box.AddComponent<Mesh>();

            switch (r)
            {
                case 0:
                    mesh.Init(new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(2, 0), new Vector2(0, -1) });
                    break;
                case 1:
                    mesh.Init(new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, -1) });
                    break;
                case 2:
                    mesh.Init(new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(2, 0), new Vector2(3, 0) });
                    break;
                case 3:
                    mesh.Init(new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1) });
                    break;
                default:
                    break;
            }
            Collider mc = box.AddComponent<Collider>();
            Renderer renderer = box.AddComponent<Renderer>(); ;
            renderer.Init(RendererMode.GameObject, 10);
            switch (r)
            {
                case 0:
                    renderer.Rendering("甲乙丙丁", Color.Blue, Color.Red);
                    break;
                case 1:
                    renderer.Rendering("一二三四", Color.Gray, Color.Green);
                    break;
                case 2:
                    renderer.Rendering("子丑寅卯", Color.Cyan, Color.Red);
                    break;
                case 3:
                    renderer.Rendering("-1-2-3-4", Color.DarkGray, Color.DarkBlue);
                    break;
                default:
                    break;
            }

            return box;
        }

        /// <summary>
        /// 创建一个测试用字符串
        /// </summary>
        public static Renderer CreateTestLable(Vector2 pos)
        {
            string str = "欢迎使用DestroyEngine";
            int length = CharUtils.GetStringWidth(str);
            Renderer renderer = UIFactroy.CreateLabel(pos, "",length / 2 + 1);
            renderer.Rendering(str, Color.Cyan, Color.Blue);
            return renderer;
        }

        /// <summary>
        /// 创建一个测试用文本框
        /// </summary>
        public static TextBox CreateTestTextBox(Vector2 pos)
        {
            TextBox textBox = UIFactroy.CreateTextBox(pos, 5, 10);
            textBox.SetText("Destroy TextBox", 1);
            Renderer renderer = textBox.Labels[0].GetComponent<Renderer>();
            renderer.SetForeColor(Color.Green);
            textBox.SetText("1.这是一个文本框", 2);
            textBox.SetText("2.文本框没有碰撞", 3);
            textBox.SetText("3.不会随摄像机移动", 4);
            textBox.SetText("4.推箱子玩吧", 5);
            return textBox;
        }

        /// <summary>
        /// 创建一个测试用ListBox
        /// </summary>
        public static ListBox CreateTestListBox(Vector2 pos)
        {
            ListBox textBox = UIFactroy.CreateListBox(pos, 5, 10);

            for (int i = 0; i < 10; i++)
            {
                textBox.CreateLabelItem(i.ToString() + "哈哈");
            }
            return textBox;
        }
    }
}
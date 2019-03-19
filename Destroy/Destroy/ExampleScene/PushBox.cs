namespace Destroy.Example
{
    using System.Collections.Generic;

    /// <summary>
    /// 一个用于展示的场景
    /// </summary>
    public class ExampleScenePushBox : Scene
    {
        /// <summary>
        /// 用new 新建这个场景
        /// </summary>
        public ExampleScenePushBox() : base("PushBox")
        {

        }

        /// <summary>
        /// 在场景中创建对象
        /// </summary>
        public override void OnStart()
        {
            GameObject player = TestAssetsFactroy.CreateStandardPlayer("吊", new Vector2Int(5, 10));

            TestAssetsFactroy.CreateBox(TestAssetsFactroy.BoxType.a, new Vector2Int(10, 10));
            TestAssetsFactroy.CreateBox(TestAssetsFactroy.BoxType.b, new Vector2Int(15, 10));
            //StandardAssets.CreateBox(StandardAssets.BoxType.b, new Vector2Int(10, 10));
            TestAssetsFactroy.CreateBox(TestAssetsFactroy.BoxType.c, new Vector2Int(10, 15));
            TestAssetsFactroy.CreateBox(TestAssetsFactroy.BoxType.d, new Vector2Int(15, 15));

            TestAssetsFactroy.CreateTestLable(new Vector2Int(11, 28));

            //StandardAssets.CreateTestTextBox(new Vector2Int(10, 21));
            ListBox listTextBox = TestAssetsFactroy.CreateTestListBox(new Vector2Int(10, 21));


            SystemUIFactroy.GetTimerUI(new Vector2Int(22, 12));

            SystemUIFactroy.GetMouseEventUI(new Vector2Int(22, 6));

            //SystemUIFactroy.GetRendererTestAera(new Vector2Int(1, 1));
        }
    }

    /// <summary>
    /// 创建一个典型的组件的实例
    /// </summary>
    public static class TestAssetsFactroy
    {
        /// <summary>
        /// 创建一个标准单格主角
        /// </summary>
        public static GameObject CreateStandardPlayer(string str, Vector2Int pos)
        {
            GameObject player = new GameObject("StandardPlayer")
            {
                Position = pos
            };

            player.AddComponent<Mesh>();

            Collider mc = player.AddComponent<Collider>();

            Renderer renderer = player.AddComponent<Renderer>();
            renderer.Rendering(str);
            renderer.Depth = 5;

            CharacterController controller = player.AddComponent<CharacterController>();
            controller.CanMoveInCollider = true;
            return player;
        }

        /// <summary>
        /// 标准箱子的形状类型
        /// </summary>
        public enum BoxType
        {
            /// <summary>
            /// 
            /// </summary>
            a,
            /// <summary>
            /// 
            /// </summary>
            b,
            /// <summary>
            /// 
            /// </summary>
            c,
            /// <summary>
            /// 
            /// </summary>
            d,
            /// <summary>
            /// 
            /// </summary>
            e
        }

        /// <summary>
        /// 创建一个基于物理系统的标准箱子
        /// </summary>
        public static GameObject CreateBox(BoxType boxType, Vector2Int pos)
        {
            GameObject box = new GameObject("箱子" + boxType.ToString())
            {
                Position = pos
            };

            Mesh mesh = box.AddComponent<Mesh>();


            switch (boxType)
            {
                case BoxType.a:
                    mesh.Init(new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(0, -1) });
                    break;
                case BoxType.b:
                    mesh.Init(new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(1, -1) });
                    break;
                case BoxType.c:
                    mesh.Init(new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(3, 0) });
                    break;
                case BoxType.d:
                    mesh.Init(new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1) });
                    break;
                default:
                    break;
            }
            Collider mc = box.AddComponent<Collider>();
            Renderer renderer = box.AddComponent<Renderer>(); ;
            renderer.Init(RendererMode.GameObject, 10);
            switch (boxType)
            {
                case BoxType.a:
                    renderer.Rendering("甲乙丙丁", Colour.Blue, Colour.Red);
                    break;
                case BoxType.b:
                    renderer.Rendering("一二三四", Colour.Gray, Colour.Green);
                    break;
                case BoxType.c:
                    renderer.Rendering("子丑寅卯", Colour.Cyan, Colour.Red);
                    break;
                case BoxType.d:
                    renderer.Rendering("-1-2-3-4", Colour.DarkGray, Colour.DarkBlue);
                    break;
                default:
                    break;
            }

            return box;
        }

        /// <summary>
        /// 创建一个测试用字符串
        /// </summary>
        public static Renderer CreateTestLable(Vector2Int pos)
        {
            string str = "欢迎使用DestroyEngine";
            int length = CharUtils.GetStringWidth(str);
            Renderer renderer = UIFactroy.CreateLabel(pos, length / 2 + 1);
            renderer.Rendering(str, Colour.Cyan, Colour.Blue);
            return renderer;
        }

        /// <summary>
        /// 创建一个测试用文本框
        /// </summary>
        public static TextBox CreateTestTextBox(Vector2Int pos)
        {
            TextBox textBox = UIFactroy.CreateTextBox(pos, 5, 10);
            textBox.SetText("Destroy TextBox", 1);
            Renderer renderer = textBox.Labels[0].GetComponent<Renderer>();
            renderer.SetForeColor(Colour.Green);
            textBox.SetText("1.这是一个文本框", 2);
            textBox.SetText("2.文本框没有碰撞", 3);
            textBox.SetText("3.不会随摄像机移动", 4);
            textBox.SetText("4.推箱子玩吧", 5);
            return textBox;
        }

        /// <summary>
        /// 创建一个测试用ListBox
        /// </summary>
        public static ListBox CreateTestListBox(Vector2Int pos)
        {
            ListBox textBox = UIFactroy.CreateListBox(pos, 5, 10);

            for (int i = 0; i < 10; i++)
            {
                textBox.Items.Add(textBox.CreateLabelItem(i.ToString() + "哈哈"));
            }
            return textBox;
        }
    }
}
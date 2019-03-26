namespace Destroy.Standard
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
            GameObject player = TestAssetsFactroy.CreateStandardPlayer("吊", new Vector2(5, 10));
            CameraController.Instance.followTrans = player;



            TestAssetsFactroy.CreateBox(new Vector2(10, 10));
            TestAssetsFactroy.CreateBox(new Vector2(15, 10));
            //StandardAssets.CreateBox(StandardAssets.BoxType.b, new Vector2Int(10, 10));
            TestAssetsFactroy.CreateBox(new Vector2(10, 15));
            TestAssetsFactroy.CreateBox(new Vector2(15, 15));

            TestAssetsFactroy.CreateTestLable(new Vector2(11, 28));

            //StandardAssets.CreateTestTextBox(new Vector2Int(10, 21));
            ListBox listTextBox = TestAssetsFactroy.CreateTestListBox(new Vector2(10, 21));


            //SystemUIFactroy.GetTimerUI(new Vector2(22, 12));
            //SystemUIFactroy.GetMouseEventUI(new Vector2(22, 6));
            SystemUIFactroy.GetSystemInspector();

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
        public static GameObject CreateStandardPlayer(string str, Vector2 pos)
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
            controller.CanMoveInCollider = false;
            return player;
        }

        /// <summary>
        /// 创建一个基于物理系统的标准箱子
        /// </summary>
        public static GameObject CreateBox(Vector2 pos)
        {
            int r = GameMode.Random.Next(0, 3);

            GameObject box = new GameObject("箱子" + r.ToString())
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
            Renderer renderer = UIFactroy.CreateLabel(pos, length / 2 + 1);
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
                textBox.Items.Add(textBox.CreateLabelItem(i.ToString() + "哈哈"));
            }
            return textBox;
        }
    }
}
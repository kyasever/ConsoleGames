namespace Destroy
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 系统的调试窗口组件,可以通过一行命令进行调出,使用时请注意注释提醒的窗口大小,给窗口留出足够的位置
    /// </summary>
    public static class SystemUIFactroy
    {
        /// <summary>
        /// 显示系统中各个系统耗时
        /// 范围14*17
        /// </summary>
        public static GameObject GetTimerUI(Vector2Int pos, int interval = 10)
        {
            TextBox textBox = UIFactroy.CreateTextBox(pos, 12, 15);
            TimerBox script = textBox.AddComponent<TimerBox>();
            script.Interval = interval;
            return textBox.GameObject;
        }

        /// <summary>
        /// 用于显示鼠标检测到的事件
        /// 范围5*10
        /// </summary>
        public static GameObject GetMouseEventUI(Vector2Int pos)
        {
            TextBox textBox = UIFactroy.CreateTextBox(pos, 5, 10);
            textBox.AddComponent<MouseEventBox>();
            return textBox.GameObject;
        }

        /// <summary>
        /// 一片用于测试渲染器压力测试的组件,每帧都会进行200个彩色格子的刷新,刷新率取决于帧率
        /// 范围20*20
        /// </summary>
        public static GameObject GetRendererTestAera(Vector2Int pos)
        {
            Random random = new Random();
            string EsString = "临兵斗者皆阵列在前";

            GameObject testGameObject = new GameObject("RendererTest", "UI");
            List<Vector2Int> posList = new List<Vector2Int>();
            for (int i = 0; i < 400; i++)
            {
                posList.Add(new Vector2Int(i % 20, i / 20));
            }

            List<Vector2Int> posListC1 = new List<Vector2Int>();
            List<Vector2Int> posListC2 = new List<Vector2Int>();
            posListC2.Add(new Vector2Int(0, 0));

            for (int i = 0; i < 400; i++)
            {
                if (random.Next(0,100) > 50)
                {
                    posListC1.Add(posList[i]);
                }
                else
                    posListC2.Add(posList[i]);
            }

            List<RenderPoint> rpListC1 = new List<RenderPoint>();
            List<RenderPoint> rpListC2 = new List<RenderPoint>();

            for (int i = 0; i < 200; i++)
            {
                rpListC1.Add(GetARandomPoint());
                rpListC2.Add(GetARandomPoint());
            }

            RendererTestScript script = testGameObject.AddComponent<RendererTestScript>();
            script.R1 = CreateChildR(posListC1, rpListC1);
            script.R2 = CreateChildR(posListC2, rpListC2);
            return testGameObject;

            RenderPoint GetARandomPoint()
            {
                int r = random.Next(0, EsString.Length - 1);
                Colour fc = new Colour((ConsoleColor)random.Next(0, 15));
                Colour bc = new Colour((ConsoleColor)random.Next(0, 15));
                return new RenderPoint(EsString[r].ToString(), fc, bc, -1);
            }

            GameObject CreateChildR(List<Vector2Int> meshList, List<RenderPoint> rendererList)
            {
                GameObject childR = new GameObject("ChildTest", "UI");
                childR.AddComponent<Mesh>().Init(meshList);
                Renderer renderer = childR.AddComponent<Renderer>();
                renderer.Init(RendererMode.UI, -10);
                renderer.Rendering(rendererList);
                childR.Parent = testGameObject;
                return childR;
            }
        }
    }

    internal class RendererTestScript : Script
    {
        public GameObject R1, R2;
        public bool flag = true;
        public override void Update()
        {
            if (flag)
            {
                R1.SetActive(true);
                R2.SetActive(false);
            }
            else
            {
                R1.SetActive(false);
                R2.SetActive(true);
            }
            flag = !flag;
        }
    }

    internal class MouseEventBox : Script
    {
        private TextBox textBox;

        public override void Start()
        {
            textBox = GetComponent<TextBox>();
        }
        public override void Update()
        {
            textBox.SetText(Input.MousePosition.ToString(), 1);
            textBox.SetText(Input.MousePositionInPixel.ToString(), 2);
        }
    }

    internal class TimerBox : Script
    {
        public int Interval = 10;

        private int count = 0;

        public override void Start()
        {
            GetComponent<TextBox>().SetText("高精度计时器- 帧率统计 ms", 1);
        }

        public override void Update()
        {
            count++;
            if (count == Interval)
            {
                count = 0;

                int i = 2;
                foreach (KeyValuePair<string, float> v in Time.SystemsSpendTimeDict)
                {
                    GetComponent<TextBox>().SetText(v.Key + (v.Value*1000).ToString(), i);
                    i++;
                }
                GetComponent<TextBox>().SetText("引擎用时 (秒):" + Time.AllSystemsSpendTime.ToString(), i);
                i++;
                GetComponent<TextBox>().SetText("最大帧率:" + Time.MaxFrameRate.ToString(), i);
                i++;
                GetComponent<TextBox>().SetText("当前帧率:" + Time.CurFrameRate.ToString(), i);
                i++;
            }
        }
    }
}

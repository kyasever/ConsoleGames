using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Destroy;

namespace Destroy
{
    public class RawProgressBar : Script
    {
        /// <summary>
        /// 进度条的总长度
        /// </summary>
        public int Length;

        protected float progress;
        /// <summary>
        /// 进度条的进度,取值0/1 当进度改变的时候,自动改变进度条的渲染
        /// </summary>
        public float Progress
        {
            get { return progress; }
            set
            {
                progress = value;
                DrawProgress();
            }
        }


        protected float maxValue;
        public float MaxValue
        {
            get
            {
                return maxValue;
            }
            set
            {
                maxValue = value;
                progress = curValue / maxValue;
                DrawProgress();
            }
        }

        protected float curValue;
        public float Value
        {
            get
            {
                return curValue;
            }
            set
            {
                if (value > maxValue)
                {
                    curValue = maxValue;
                    return;
                }
                if (value < 0)
                {
                    curValue = 0;
                    return;
                }

                curValue = value;
                progress = curValue / maxValue;
                DrawProgress();
            }
        }

        /// <summary>
        /// 上次绘图的长度,如果长度没有改变,则不需要重新绘制
        /// </summary>
        protected int pastCount = 0;

        /// <summary>
        /// 重新绘制进度条,根据不同样式的进度条进行不同的重载操作
        /// </summary>
        public virtual void DrawProgress()
        {
            
        }

        public override string ToString()
        {
            return Value.ToString() + "/" + MaxValue.ToString() + "  ";
        }
    }

    public class ProgressBar : RawProgressBar
    {
        public static ProgressBar Create(int length = 10, float maxValue = 100, float value = 50)
        {
            ProgressBar progressBar = GameObject.CreateWith<ProgressBar>("ProgressBar", "UI", posList: UIUtils.CreateLineMesh(length));
            progressBar.AddComponent<Renderer>().Init(RendererMode.UI, -5);

            progressBar.maxValue = maxValue;
            progressBar.curValue = value;
            progressBar.Length = length;
            progressBar.progress = value / maxValue;

            progressBar.DrawProgress();
            return progressBar;
        }

        public override void DrawProgress()
        {
            int needCount = (int)(Length * 8 * progress);
            if (needCount == pastCount)
                return;

            int a = needCount / 8;
            int b = needCount % 8;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < a; i++)
            {
                sb.Append(UnicodeDrawing.Block8_8);
            }
            switch (b)
            {
                case 0:
                    sb.Append(UnicodeDrawing.Block0_8);
                    break;
                case 1:
                    sb.Append(UnicodeDrawing.Block1_8);
                    break;
                case 2:
                    sb.Append(UnicodeDrawing.Block2_8);
                    break;
                case 3:
                    sb.Append(UnicodeDrawing.Block3_8);
                    break;
                case 4:
                    sb.Append(UnicodeDrawing.Block4_8);
                    break;
                case 5:
                    sb.Append(UnicodeDrawing.Block5_8);
                    break;
                case 6:
                    sb.Append(UnicodeDrawing.Block6_8);
                    break;
                case 7:
                    sb.Append(UnicodeDrawing.Block7_8);
                    break;
            }

            //Debug.Log(needCount.ToString() + "  " + sb.ToString());

            GetComponent<Renderer>().Rendering(sb.ToString(), Color.Red, Config.DefaultBackColor);

            pastCount = needCount;
        }
    }

    public class ProgressBarChar : RawProgressBar
    {
        public char fillChar;
        public char emptyChar;

        public bool enableColor = true;
        public Color fullColor = Color.Green;
        public Color halfColor = Color.Yellow;
        public Color emptyColor = Color.Red;

        public override void DrawProgress()
        {
            int totalCount = (Length - 1) * 2;
            int fillCount = (int)(totalCount * progress);
            int emptyCount = totalCount - fillCount;

            if (fillCount == pastCount)
                return;

            StringBuilder sb = new StringBuilder();

            sb.Append('[');

            for (int i = 0; i < fillCount; i++)
            {
                sb.Append(fillChar);
            }

            for (int i = 0; i < emptyCount; i++)
            {
                sb.Append(emptyChar);
            }

            sb.Append(']');

            if(enableColor)
            {
                if (progress < 0.2f)
                {
                    GetComponent<Renderer>().Rendering(sb.ToString(), emptyColor);
                }
                else if (progress < 0.5f)
                {
                    GetComponent<Renderer>().Rendering(sb.ToString(), halfColor);
                }
                else
                {
                    GetComponent<Renderer>().Rendering(sb.ToString(), fullColor);
                }
            }
            else
            {
                GetComponent<Renderer>().Rendering(sb.ToString(), fullColor);
            }




            pastCount = fillCount;
        }

        public static ProgressBarChar Create(int length = 10, float maxValue = 100, float value = 50,char fillChar = '>',char emptyChar = '-')
        {
            ProgressBarChar progressBar = GameObject.CreateWith<ProgressBarChar>("ProgressBar", "UI", posList: UIUtils.CreateLineMesh(length));
            progressBar.AddComponent<Renderer>().Init(RendererMode.UI, -5);

            progressBar.maxValue = maxValue;
            progressBar.curValue = value;
            progressBar.Length = length;
            progressBar.progress = value / maxValue;
            progressBar.fillChar = fillChar;
            progressBar.emptyChar = emptyChar;

            progressBar.DrawProgress();
            return progressBar;
        }
    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destroy
{

    public class ProgressBar : Script
    {
        /// <summary>
        /// 进度条的总长度
        /// </summary>
        public int Length;

        private float progress;
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


        private float maxValue;
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

        private float curValue;
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
        private int pastCount = 0;
        /// <summary>
        /// 重新绘制进度条 根据已经弄好的变量来绘制
        /// </summary>
        public void DrawProgress()
        {
            int needCount = (int)(Length * 8 * progress);
            if (needCount == pastCount)
                return;

            ClearRenderer();

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

            DrawString(sb.ToString(),Color.Red,Config.DefaultBackColor);


            pastCount = needCount;
        }

        public static ProgressBar Create(int length = 10, float maxValue = 100, float value = 50)
        {
            ProgressBar progressBar = UIObject.CreateWith<ProgressBar>("ProgressBar", "UI");

            progressBar.maxValue = maxValue;
            progressBar.curValue = value;
            progressBar.Length = length;

            progressBar.DrawProgress();

            return progressBar;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Destroy.Winform
{
    /// <summary>
    /// 将引擎传来的数据处理为图像,受独立线程管辖。
    /// Core -> this -> form
    /// </summary>
    public class EditorRuntime
    {
        /// <summary>
        /// 运行编辑器生命周期
        /// </summary>
        public static void Run()
        {
            var form = FormEditor.Instanse;
            while (true)
            {
                if (!RuntimeEngine.Enabled)
                    break;
                Thread.Sleep(0);
                //将引擎提供的原始数据绘制到图片上
                PreDraw();
                //将图片绘制到界面上
                if (bufferBitmap != null)
                    form.Draw(bufferBitmap);
            }
        }

        /// <summary>
        /// 当前渲染的数据来源，先将其复制进一级缓存
        /// </summary>
        public static List<RenderPoint> renderList = new List<RenderPoint>();

        /// <summary>
        /// 本次要渲染的结果，一级缓存
        /// </summary>
        public static List<RenderPoint> buffer = new List<RenderPoint>();

        /// <summary>
        /// 上次的渲染结果，二级缓存，用于进行对比输出
        /// </summary>
        public static List<RenderPoint> bufferbuffer = new List<RenderPoint>();

        /// <summary>
        /// 第一次调用渲染的时候初始化buffer
        /// </summary>
        public static bool firstLoad = true;

        /// <summary>
        /// 用于缓存的图片
        /// </summary>
        public static Bitmap bufferBitmap;



        /// <summary>
        /// 将引擎提供的原始数据绘制到图片上
        /// </summary>
        public static void PreDraw()
        {
            if (renderList.Count == 0)
            {
                return;
            }

            lock(renderList)
            {
                buffer.Clear();
                foreach(var v in renderList)
                {
                    buffer.Add(v);
                }
            }


            Graphics g;
            if (firstLoad)
            {
                bufferBitmap = new Bitmap(Config.WindowsSize.X, Config.WindowsSize.Y);

                g = Graphics.FromImage(bufferBitmap);
                g.FillRectangle(new SolidBrush(Config.DefaultBackColor.ToColor()),
                    new RectangleF(new Point(0, 0), new SizeF(Config.WindowsSize.X, Config.WindowsSize.Y)));
                foreach (var r in buffer)
                {
                    bufferbuffer.Add(new RenderPoint());
                }
                firstLoad = false;
            }

            g = Graphics.FromImage(bufferBitmap);

            //初始化刷子
            SolidBrush solidBrushFore = new SolidBrush(Config.DefaultForeColor.ToColor());
            SolidBrush solidBrushBack = new SolidBrush(Config.DefaultBackColor.ToColor());

            #region 可能用到的font
            //这个字体制表符是占两位的..... 新宋体的制表符占2位,显示可能有点问题
            //if (CharUtils.IsTabChar(rp.Str[0]))
            //{
            //    font = new Font("Consolas", 12, FontStyle.Bold);
            //}
            //else
            //{

            //    font = new Font("新宋体", 12, FontStyle.Bold);
            //}
            #endregion
            //初始化字体
            Font fontCn = new Font("新宋体", 12, FontStyle.Bold);
            Font fontEn = new Font("Courier New", 12, FontStyle.Bold);

            #region 非双缓冲模式
            /*
            for (int i = 0; i < drawList.Count; i++)
            {
                //如果是单个汉字的话,那么直接跳过第二个字符的渲染
                if (i > 0 && CharUtils.GetCharWidth(drawList[i - 1].Str[0]) == 2)
                {
                    continue;
                }
                RenderPoint rp = drawList[i];

                solidBrushFore.Color = rp.ForeColor.ToColor();
                solidBrushBack.Color = rp.BackColor.ToColor();

                int pixelX = i % (Config.ScreenWidth * (int)Config.CharWidth) * Config.RendererSize.X / 2;
                int pixelY = i / (Config.ScreenWidth * (int)Config.CharWidth) * Config.RendererSize.Y;
                //DebugLog(new Vector2Int(pixelX, pixelY).ToString());
                Point point = new Point(pixelX, pixelY);

                g.FillRectangle(solidBrushBack, new RectangleF(new Point(point.X + 4, point.Y),
                    new SizeF(Config.RendererSize.X, Config.RendererSize.Y)));
                g.DrawString(rp.Str, font, solidBrushFore, point);
            }
            */
            #endregion


            List<int> diffIndex = new List<int>();

            //挑出不同的点
            for (int i = 0; i < buffer.Count; i++)
            {
                if (!bufferbuffer[i].Equals(buffer[i]))
                {
                    diffIndex.Add(i);
                }
            }

            void DrawPoint(RenderPoint rp,Point point) {
                solidBrushFore.Color = rp.ForeColor.ToColor();
                solidBrushBack.Color = rp.BackColor.ToColor();


                g.FillRectangle(solidBrushBack, new RectangleF(new Point(point.X + 3, point.Y),
                    new SizeF(Config.RendererSize.X, Config.RendererSize.Y)));
                //if (CharUtils.GetCharWidth(rp.Str[0]) == 2)
                if (rp.Str.Length == 1) {
                    g.DrawString(rp.Str, fontCn, solidBrushFore, point);
                }
                else {
                    g.DrawString(rp.Str[0].ToString(), fontEn, solidBrushFore, point);
                    g.DrawString(rp.Str[1].ToString(), fontEn, solidBrushFore, new Point(point.X + 8, point.Y));
                }
            }

            foreach (int index in diffIndex)
            {
                RenderPoint rp = buffer[index];

                int pixelX = index % (Config.ScreenWidth) * Config.RendererSize.X;
                int pixelY = index / (Config.ScreenWidth) * Config.RendererSize.Y;
                Point point = new Point(pixelX, pixelY);
                
                DrawPoint(rp, point);
            }

            //GameObject obj = FormEditor.Instanse.CurrertGameObject;
            //if (obj != null) {
            //    Vector2 pos = Utils.World2Screen(obj.Position);
            //    Point center = new Point(pos.X + 8, pos.Y + 8);
            //    Pen linePen = new Pen(System.Drawing.Color.Red);
            //    g.DrawLine(linePen, center, new Point(center.X + 100, center.Y));
            //    linePen.Color = System.Drawing.Color.Blue;
            //    g.DrawLine(linePen, center, new Point(center.X, center.Y + 100));
            //}
            //firstLoad = true;

            //复制缓存
            for (int i = 0; i < buffer.Count; i++)
            {
                bufferbuffer[i] = buffer[i];
            }
            EditorSystem.PreRenderCount++;
        }

    }
}

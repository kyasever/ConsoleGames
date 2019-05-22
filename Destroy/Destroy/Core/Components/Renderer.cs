namespace Destroy
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /*
     * 12/7 by Kyasever
     * Renderer通过继承分为了三个组件:
     * StringRenderer 用于渲染一句字符串
     * PosRenderer 用于渲染一个点
     * GroupRenderer 用于渲染一组图形
     * 之后版本要重新优化
     *
     * 12/8 重写了Renderer的API
     * 
     * 12/27 by kyasever
     * 修改了Renderer的API 现在默认初始化不需要Shader了,但是依然可以手动修改,可能这个引擎以后也再也不需要第二种Shader了,但是不想删...
     * 增加了一种直接从白模初始化的方式,
     * 将四种构造函数合并成一种,现在一共有三种初始化方式,使用字符串和颜色初始化-使用材质和贴图初始化-使用白模和材质初始化
     * 白模表示一个字典,初始化的时候由手动获取指定,标准情况下Key和元素数量和Mesh是一样的,Value是这个点上的字符元素
     * 
     * 4/24 准备重写Renderer和RSystem
     * renderer的渲染结果是一系列的 Vector2-RP
     * RS里保存的结构类型为Dic Vector To RP结果
     * RS交给渲染器的东西不包括空白点
    */

    /// <summary>
    /// 渲染组件,最基础的渲染组件只负责维护这点东西,理论上这个也是能用的.甚至不依赖Mesh组件,直接编辑渲染结果就好了
    /// </summary>
    public class Renderer : RawComponent
    {
        /// <summary>
        /// 务必初始化. 通常情况下在创建GO的同时就生成好准确的Depth
        /// </summary>
        public int Depth = int.MaxValue;

        /// <summary>
        /// 渲染结果,允许直接操作它来进行渲染.
        /// 没毛病 本质上所有对Renderer的操作都是为了给这个字典添加值
        /// </summary>
        [HideInInspector]
        public virtual Dictionary<Vector2, RenderPoint> RendererPoints { get; set; } = new Dictionary<Vector2, RenderPoint>();

        /// <summary>
        /// 核心DrawString方法.从左至右的给对象增加一个字符串的渲染
        /// </summary>
        /// <param name="str">要渲染的字符串内容</param>
        /// <param name="foreColor">字符串前景色</param>
        /// <param name="backColor">字符串背景色</param>
        /// <param name="StartPosition">字符串渲染的起始点(相对中心点的偏移量)</param>
        /// <param name="MaxWidth">字符串最大渲染宽度</param>
        /// <param name="MinWidth">字符串最小渲染宽度</param>
        public void DrawString(string str, Color foreColor, Color backColor, Vector2 StartPosition, int MaxWidth = int.MaxValue, int MinWidth = 0)
        {
            List<string> grids = CharUtils.DivideString(str);
            int index = 0;
            foreach (string s in grids)
            {
                Vector2 key = StartPosition + new Vector2(index, 0);
                RenderPoint value = new RenderPoint(s, foreColor, backColor, Depth);
                SafeAdd(key, value);
                index++;
                if (index == MaxWidth - 1)
                    break;
            }
            if (grids.Count < MinWidth)
            {
                for (int i = grids.Count; i < MinWidth; i++)
                {
                    Vector2 key = StartPosition + new Vector2(i, 0);
                    RenderPoint value = new RenderPoint("  ", foreColor, backColor, Depth);
                    SafeAdd(key, value);
                }
            }
        }

        private bool SafeAdd(Vector2 point, RenderPoint renderPoint)
        {
            if (RendererPoints.ContainsKey(point))
            {
                RendererPoints[point] = RendererPoints[point] + renderPoint;
                return true;
            }
            else
            {
                RendererPoints[point] = renderPoint;
                return false;
            }
        }

        /// <summary>
        /// 第一个参数 从Vector2/Line/Rectangle 中选取一个,代表要渲染的形状
        /// 第二个参数 从Void(没有参数)/RenderPoint/Color 中选取一个,代表使用制表符/指定的渲染点/背景色中选取一个,代表渲染的填充物
        /// </summary>
        public void Draw(params object[] args)
        {
            //第一个参数是Vector2 代表要渲染一个点
            if (args[0].GetType() == typeof(Vector2))
            {
                Vector2 point = (Vector2)args[0];
                if (args.Length == 1)
                {
                    string doubleHori = BoxDrawingCharacter.BoxHorizontal.ToString() + BoxDrawingCharacter.BoxHorizontal.ToString();
                    SafeAdd(point, new RenderPoint(doubleHori, Depth));
                }
                else if (args[1].GetType() == typeof(RenderPoint))
                {
                    SafeAdd(point, (RenderPoint)args[1]);
                }
                else if (args[1].GetType() == typeof(Color))
                {
                    SafeAdd(point, new RenderPoint("  ", Config.DefaultForeColor, (Color)args[1], Depth));
                }
            }
            else if (args[0].GetType() == typeof(Line))
            {
                Line line = (Line)args[0];
                RenderPoint renderPoint = RenderPoint.Block;
                if (args.Length == 1)
                {
                    renderPoint = new RenderPoint(line.GetStr(), Depth);
                }
                else if (args[1].GetType() == typeof(RenderPoint))
                {
                    renderPoint = (RenderPoint)args[1];

                }
                else if (args[1].GetType() == typeof(Color))
                {
                    renderPoint = new RenderPoint("  ", Config.DefaultForeColor, (Color)args[1], Depth);
                }
                foreach (var v in line.PosList)
                {
                    SafeAdd(v, renderPoint);
                }
            }
            else if (args[0].GetType() == typeof(Rectangle))
            {
                Rectangle rect = (Rectangle)args[0];

                if (args.Length == 1)
                {
                    var list = CharUtils.DivideString(rect.GetStr());
                    for (int i = 0; i < rect.PosList.Count; i++)
                    {
                        SafeAdd(rect.PosList[i], new RenderPoint(list[i], Depth));
                    }
                }
                else if (args[1].GetType() == typeof(RenderPoint))
                {
                    foreach(var v in rect.PosList)
                    {
                        SafeAdd(v, (RenderPoint)args[1]);
                    }
                }
                else if (args[1].GetType() == typeof(Color))
                {
                    var renderPoint = new RenderPoint("  ", Config.DefaultForeColor, (Color)args[1], Depth);
                    foreach (var v in rect.PosList)
                    {
                        SafeAdd(v, (RenderPoint)args[1]);
                    }
                }
            }
        }

        #region 设置基础属性,通常用于快捷更改,避免用这些来进行初始化
        /// <summary>
        /// 更改背景色
        /// </summary>
        public void SetBackColor(Color backColor)
        {
            Dictionary<Vector2, RenderPoint> newdic = new Dictionary<Vector2, RenderPoint>();
            foreach (Vector2 point in RendererPoints.Keys)
            {
                RenderPoint rp = RendererPoints[point];
                rp.BackColor = backColor;
                newdic.Add(point, rp);
            }
            RendererPoints = newdic;
        }

        /// <summary>
        /// 更改前景色
        /// </summary>
        public void SetForeColor(Color foreColor)
        {
            Dictionary<Vector2, RenderPoint> newdic = new Dictionary<Vector2, RenderPoint>();
            foreach (Vector2 point in RendererPoints.Keys)
            {
                RenderPoint rp = RendererPoints[point];
                rp.ForeColor = foreColor;
                newdic.Add(point, rp);
            }
            RendererPoints = newdic;
        }

        /// <summary>
        /// 更改渲染深度
        /// </summary>
        public void SetDepth(int depth)
        {
            Dictionary<Vector2, RenderPoint> newdic = new Dictionary<Vector2, RenderPoint>();
            foreach (Vector2 point in RendererPoints.Keys)
            {
                RenderPoint rp = RendererPoints[point];
                rp.Depth = depth;
                newdic.Add(point, rp);
            }
            RendererPoints = newdic;
        }
        #endregion
    }

    /// <summary>
    /// 标准输出点结构.所有的Renderer组件都会被处理为RenderPos的集合
    /// </summary>
    public struct RenderPoint
    {

        /// <summary>
        /// 默认的空白渲染点,显示在最底层
        /// </summary>
        public static RenderPoint Block = new RenderPoint("  ", int.MaxValue);

        /// <summary>
        /// 默认的UI渲染点 显示在所有UI的最底层
        /// </summary>
        public static RenderPoint UIBlock = new RenderPoint("  ", -1);

        /// <summary>
        /// 这个点的信息,不长于Width
        /// </summary>
        public string Str;

        /// <summary>
        /// 前景色
        /// </summary>
        public Color ForeColor;

        /// <summary>
        /// 背景色
        /// </summary>
        public Color BackColor;

        /// <summary>
        /// 渲染优先级(为0时脚本显示优先级最高(最后被渲染), 负数表示这个是UI(在游戏物体之前渲染))
        /// </summary>
        public int Depth;

        /// <summary>
        /// 三项属性相同就视为相等,渲染不需要知道Depth
        /// </summary>
        public override bool Equals(object obj)
        {
            RenderPoint renderPoint = (RenderPoint)obj;
            return Str == renderPoint.Str && ForeColor == renderPoint.ForeColor && BackColor == renderPoint.BackColor;
        }

        /// <summary>
        /// 
        /// </summary>
        public override int GetHashCode() => base.GetHashCode();

        /// <summary>
        /// 渲染深度比较,depth越低的,渲染优先级越高.
        /// 通常情况下默认的UI深度是-1,负数越低的,越优先渲染.
        /// 默认的游戏物体深度为int.max 数字越小的,越优先渲染.
        /// 尽量避免非UI对象深度为负数
        /// </summary>
        public static RenderPoint operator +(RenderPoint left, RenderPoint right)
        {
            //考虑了一些乱七八糟的问题,目前先都删了,只保留深度比较
            if (left.Depth < right.Depth)
            {
                if (left.Depth < 0)
                    return left;
                RenderPoint rp = new RenderPoint();
                if (left.BackColor == Config.DefaultBackColor)
                {
                    rp.BackColor = right.BackColor;
                }
                else
                {
                    rp.BackColor = left.BackColor;
                }

                if (left.Str == "  ")
                {
                    rp.ForeColor = right.ForeColor;
                    rp.Str = right.Str;
                }
                else
                {
                    rp.ForeColor = left.ForeColor;
                    rp.Str = left.Str;
                }
                rp.Depth = left.Depth;

                return rp;
            }
            else if (left.Depth == right.Depth)
            {
                if (right.Str.Length == 2)
                {
                    if (left.Str.Length == 1)
                    {
                        return right;
                    }
                    //两位字符
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < right.Str.Length; i++)
                    {
                        sb.Append(CharUtils.BoxDrawingAdd(left.Str[i], right.Str[i]));
                    }
                    right.Str = sb.ToString();
                    return right;
                }
                return right;
            }
            else
            {
                if (right.Depth < 0)
                    return right;
                RenderPoint rp = new RenderPoint();
                if (right.BackColor == Config.DefaultBackColor)
                {
                    rp.BackColor = left.BackColor;
                }
                else
                {
                    rp.BackColor = right.BackColor;
                }

                if (right.Str == "  ")
                {
                    rp.ForeColor = left.ForeColor;
                    rp.Str = left.Str;
                }
                else
                {
                    rp.ForeColor = right.ForeColor;
                    rp.Str = right.Str;
                }
                rp.Depth = right.Depth;

                return rp;
            }
        }

        /// <summary>
        /// 使用默认颜色的字符串初始化
        /// </summary>
        public RenderPoint(string str, int depth)
        {
            Str = str;
            BackColor = Config.DefaultBackColor;
            ForeColor = Config.DefaultForeColor;
            Depth = depth;
        }

        /// <summary>
        /// 使用前景颜色的字符串初始化
        /// </summary>
        public RenderPoint(string str, Color foreColor, int depth)
        {
            Str = str;
            ForeColor = foreColor;
            BackColor = Config.DefaultBackColor;
            Depth = depth;
        }

        /// <summary>
        /// 完整的初始化
        /// </summary>
        public RenderPoint(string str, Color foreColor, Color backColor, int depth = int.MaxValue)
        {
            Str = str;
            ForeColor = foreColor;
            BackColor = backColor;
            Depth = depth;
        }
    }

}
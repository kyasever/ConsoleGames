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
    */

    /// <summary>
    /// 渲染模式
    /// </summary>
    public enum RendererMode
    {
        /// <summary>
        /// 原点:屏幕左下角世界坐标(0,0)的坐标,不随摄像机运动
        /// </summary>
        UI,
        /// <summary>
        /// 原点:屏幕左下角世界坐标(0,0)的坐标,随摄像机运动
        /// </summary>
        GameObject,
    }

    /// <summary>
    /// 使用这个工具类来生成一个组合颜色的字符串.
    /// 改变颜色 - 添加字符 - 改变颜色 - 添加字符 - 输出供Renderer使用
    /// </summary>
    public class ColorStringBuilder
    {
        /// <summary>
        /// 前景色
        /// </summary>
        public Color ForeColor = Config.DefaultForeColor;
        /// <summary>
        /// 背景色
        /// </summary>
        public Color BackColor = Config.DefaultBackColor;
        private List<RenderPoint> result = new List<RenderPoint>();

        /// <summary>
        /// 使用这个工具类来生成一个组合颜色的字符串
        /// </summary>
        public ColorStringBuilder() { }

        /// <summary>
        /// 使用这个工具类来生成一个组合颜色的字符串
        /// </summary>
        /// <param name="fore">初始的前景色</param>
        /// <param name="back">初始的背景色</param>
        public ColorStringBuilder(Color fore, Color back)
        {
            ForeColor = fore;
            BackColor = back;
        }

        /// <summary>
        /// 添加一个char
        /// </summary>
        public ColorStringBuilder AppendChar(char c)
        {
            result.Add(new RenderPoint(c.ToString(), ForeColor, BackColor));
            return this;
        }

        /// <summary>
        /// 添加一个字符串
        /// </summary>
        public ColorStringBuilder AppendString(string str)
        {
            //从贴图加载字符串信息,并切分成List String
            List<string> grids = CharUtils.DivideString(str);
            foreach (string s in grids)
            {
                result.Add(new RenderPoint(s, ForeColor, BackColor));
            }
            return this;
        }

        /// <summary>
        /// 添加一个字符串,指定特定的前景色,不影响别的,加个塞
        /// </summary>
        public ColorStringBuilder AppendString(string str,Color foreColor)
        {
            //从贴图加载字符串信息,并切分成List String
            List<string> grids = CharUtils.DivideString(str);
            foreach (string s in grids)
            {
                result.Add(new RenderPoint(s, foreColor, BackColor));
            }
            return this;
        }

        /// <summary>
        /// 输出为List,供Renderer使用
        /// </summary>
        public List<RenderPoint> ToRenderer()
        {
            return result;
        }
    }

    /// <summary>
    /// 搞一个RawRenderer,尽可能的精简功能. 另外研究删除一下RP的depth,这个可能得等到alpha更新之后再说了
    /// </summary>
    public class RawRenderer : Component
    {
        /// <summary>
        /// 
        /// </summary>
        protected int depth;
        /// <summary>
        /// 渲染深度 越低的渲染优先级越高
        /// </summary>
        public int Depth
        {
            get => depth;
            set
            {
                depth = value;
                SetDepth(value);
                if (Mode == RendererMode.GameObject && value <= 0)
                {
                    Debug.Warning(GameObject.Name + "不建议渲染模式为GameObject的对象渲染深度为负数");
                }
                else if (Mode == RendererMode.UI && value >= 0)
                {
                    Debug.Warning(GameObject.Name + "不建议渲染模式为UI的对象渲染深度为正数");
                }
            }
        }


        /// <summary>
        /// 渲染模式
        /// </summary>
        public virtual RendererMode Mode { get; set; }

        /// <summary>
        /// 渲染结果,允许直接操作它来进行渲染.没毛病
        /// </summary>
        [HideInInspector]
        public virtual Dictionary<Vector2, RenderPoint> RendererPoints { get; set; }

        #region 设置四项基础属性,通常用于快捷更改,避免用这些来进行初始化
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

        /// <summary>
        /// 更改渲染的字符 将string分割放入字典.不太好
        /// </summary>
        public void SetString(string str)
        {
            List<string> list = CharUtils.DivideString(str);
            int i = 0;
            foreach (Vector2 point in RendererPoints.Keys)
            {
                RenderPoint rp = RendererPoints[point];
                rp.Str = list[i];
                RendererPoints[point] = rp;
                i++;
                if (i == list.Count)
                    break;
            }
        }
        #endregion

    }

    /// <summary>
    /// 渲染组件,最基础的渲染组件只负责维护这点东西,理论上这个也是能用的.甚至不依赖Mesh组件,直接编辑渲染结果就好了
    /// </summary>
    public class Renderer : RawRenderer
    {
        /// <summary>
        /// 修改这个API,变为使用这个点填充所有Mesh
        /// </summary>
        /// <param name="renderPoint">使用的RenderPoint参数</param>
        public void Rendering(RenderPoint renderPoint)
        {
            RendererPoints = new Dictionary<Vector2, RenderPoint>();
            foreach (var v in GameObject.PosList)
            {
                RendererPoints.Add(v, renderPoint);
            }
        }

        /// <summary>
        /// 基于默认颜色和默认Mesh进行渲染
        /// </summary>
        /// <param name="str">使用的字符串</param>
        public void Rendering(string str)
        {
            Rendering(str, Config.DefaultForeColor, Config.DefaultBackColor);
        }

        /// <summary>
        /// 基于Mesh和默认Depth
        /// </summary>
        /// <param name="str">使用的字符串</param>
        /// <param name="foreColor">前景色</param>
        /// <param name="backColor">背景色</param>
        public void Rendering(string str, Color foreColor = default(Color), Color backColor = default(Color))
        {
            if(foreColor == null)
            {
                foreColor = Config.DefaultForeColor;
            }
            if(backColor == null)
            {
                backColor = Config.DefaultBackColor;
            }

            List<RenderPoint> result = new List<RenderPoint>();
            //从贴图加载字符串信息,并切分成List String
            List<string> grids = CharUtils.DivideString(str);
            foreach (string s in grids)
            {
                result.Add(new RenderPoint(s, foreColor, backColor));
            }
            Rendering(result);
        }

        /// <summary>
        /// 基于Mesh
        /// 通过RenderPointList进行渲染,可以通过ColorStringBiulder提供参数.
        /// </summary>
        public void Rendering(List<RenderPoint> list)
        {
            RendererPoints = new Dictionary<Vector2, RenderPoint>();
            int length = GameObject.PosList.Count;
            for (int i = 0; i < length; i++)
            {
                if (i < list.Count)
                {
                    RenderPoint rp = list[i];
                    rp.Depth = Depth;
                    RendererPoints.Add(GameObject.PosList[i], rp);
                }
                else
                {
                    RendererPoints.Add(GameObject.PosList[i], RenderPoint.UIBlock);
                }
            }
        }

        /// <summary>
        /// 简单粗暴,自己来把.直接指定结果
        /// </summary>
        public void Rendering(Dictionary<Vector2, RenderPoint> dic)
        {
            RendererPoints = dic;
        }
       

        /// <summary>
        /// 初始化的时候指定mode和深度.
        /// </summary>
        public void Init(RendererMode mode = RendererMode.GameObject, int depth = int.MaxValue)
        {
            Mode = mode;
            this.depth = depth;
        }

        internal override void Initialize()
        {
            Mode = RendererMode.GameObject;
            depth = int.MaxValue;

            RendererPoints = new Dictionary<Vector2, RenderPoint>();
            foreach (Vector2 pos in GameObject.PosList)
            {
                RendererPoints.Add(pos, RenderPoint.Block);
            }
        }

        internal override void OnAdd()
        {
            RuntimeEngine.GetSystem<RendererSystem>().RendererCollection.Add(this);
        }

        internal override void OnRemove()
        {
            RuntimeEngine.GetSystem<RendererSystem>().RendererCollection.Remove(this);
        }
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
                    if(left.Str.Length == 1)
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
        public RenderPoint(string str = "  ", Color foreColor = default(Color), Color backColor = default(Color), int depth = int.MaxValue)
        {
            Str = str;
            
            if(foreColor == null)
            {
                ForeColor = Config.DefaultForeColor;
            }
            else
                ForeColor = foreColor;

            if(backColor == null)
            {
                BackColor = Config.DefaultBackColor;
            }
            else
                BackColor = backColor;

            Depth = depth;
        }
    }

}
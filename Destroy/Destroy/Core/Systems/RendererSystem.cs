namespace Destroy
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    #region 垃圾注释 舍不得删
    /*
     * 12/7 by Kyasever
     * 新增了复杂渲染系统,暂时使用了清空刷新,当检测到变动时,无条件先输出一行空格清空,然后再输入改动后的内容
     * 之后版本要重新优化
     *
     * TODO:
     * Mesh
     * 所有东西都是从Mesh组件上引申出来的,Mesh是自动生成挂上去的.
     * 内部存储格式 Vector2Int List 必须包含(0,0) 表示点集合与中心点的相对位置
     * PosMesh或者属性表示,表示这个是否是一个单点Mesh.其他组件判断的时候也单独处理
     *
     * MeshCollider 这里面的Vector2Int就是直接获取Mesh就行了
     * 当然也可以作死编辑,无所谓...
     *
     * RigidBody先检测这个东西是不是单点Mesh,如果不是
     * 检测它的MeshCollider,并按着MeshCollier挨个判断过去
     *
     * Material 本质上是Model,Material,Texture的结合体,表现上是一个字符串,可以通过包含换行符来进行多行显示.
     *      对对对,要进行Textures和Material分离,Texture表现上是一个字符串,包含换行符,没了.
     *      Material本质上是对字符串的颜色和格式处理.
     * 例如:
     * Mesh [1,2,3,4],[6]
     * BlockMaterial [绿,蓝,红,白,绿],[青,青,绿,蓝,红]
     * Texture "一二三四五六"
     * Renderer 一二三四
     *          五
     *
     * TODO: \n会进行强制换行,总是会按照矩阵顺序来进行渲染.如果Material比Mesh大,那么截断不需要的部分,如果Material比Mesh小,那么用默认颜色补充
     *
     * MeshRenderer 通过Material来渲染Texture. 改变Mesh,Material或者Texture时都会重新计算
     *     内部存储格式 RenderPoint和Vector2Int list保存
     *     
     *     
     * UI使用窗口坐标系.锚点始终是左上角.这样就没有问题了.
     * 
     * 保存一个UI边框的缓冲. 这东西初始化之后就不动了. Render刷新的时候会先调用这个覆盖.
     * String依然使用标准渲染
    */
    #endregion


    /// <summary>
    /// 渲染系统
    /// </summary>
    public class RendererSystem : DestroySystem
    {
        internal List<Renderer> RendererCollection { get; set; } = new List<Renderer>();

        private RenderPoint[,] renderers;

        /// <summary>
        /// 
        /// </summary>
        public override void Start()
        {
            int screenWidth = Config.ScreenWidth;
            int screenHeight = Config.ScreenHeight;
            int charWidth = (int)Config.CharWidth;

            renderers = new RenderPoint[screenWidth, screenHeight];

            //初始化空渲染点的定义
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < charWidth; i++)
            {
                sb.Append(' ');
            }
            RenderPoint.Block.Str = sb.ToString();
            RenderPoint.UIBlock.Str = sb.ToString();

            //将Buffer复制,Render清空为Block
            for (int i = 0; i < renderers.GetLength(0); i++)
            {
                for (int j = 0; j < renderers.GetLength(1); j++)
                {
                    renderers[i, j] = RenderPoint.Block;
                }
            }
        }

        /// <summary>
        /// 重置系统,重新刷新屏幕.一般来说最小化窗口之后可能需要这种操作,和一些迷之显示bug需要
        /// </summary>
        public virtual void Reset()
        {
            Console.Clear();
            Start();
            Update();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update()
        {
            if (!Camera.Main.Enable)
                return;

            //处理 将Renderer信息画到画布上
            foreach (Renderer renderer in RendererCollection)
            {
                if (!renderer.Enable)
                    continue;
                foreach (KeyValuePair<Vector2, RenderPoint> rendererPoint in renderer.RendererPoints)
                {
                    Vector2 pos = new Vector2();
                    if (renderer.Mode == RendererMode.GameObject)
                    {
                        //相对于摄像机的位置
                        pos = renderer.Position + rendererPoint.Key - Camera.Main.Position;
                    }
                    else if (renderer.Mode == RendererMode.UI)
                    {
                        //相对于原点的位置
                        pos = renderer.Position + rendererPoint.Key;
                    }
                    //防止数组越界
                    if (pos.X >= 0 && pos.X < Config.ScreenWidth && pos.Y >= 0 && pos.Y < Config.ScreenHeight)
                    {
                        //世界坐标:[右X, 上Y] -> 屏幕坐标:[右X, 下Y]
                        int x = pos.X;
                        int y = Config.ScreenHeight - 1 - pos.Y;
                        //这步操作非常重要. 内部进行了很多运算处理,并不是覆盖或者别的.
                        renderers[x, y] += rendererPoint.Value;
                    }
                }
            }

            List<RenderPoint> list = new List<RenderPoint>();

            for (int j = 0; j < renderers.GetLength(1); j++)
            {
                for (int i = 0; i < renderers.GetLength(0); i++)
                {
                    RenderPoint rp1 = renderers[i, j];
                    RenderPoint rp2 = renderers[i, j];

                    if (renderers[i, j].Str.Length == 2)
                    {
                        rp1.Str = renderers[i, j].Str[0].ToString();
                        rp2.Str = renderers[i, j].Str[1].ToString();
                    }
                    else if (renderers[i, j].Str.Length == 1)
                    {
                        rp1.Str = renderers[i, j].Str[0].ToString();
                        rp2.Str = ' '.ToString();
                    }

                    list.Add(rp1);
                    list.Add(rp2);
                }
            }

            //这里只负责调用这个事件来通知关注它的组件进行渲染,事件怎么实现的不管
            StandardIO.RendererEvent?.Invoke(list);

            //清空Render
            for (int i = 0; i < renderers.GetLength(0); i++)
            {
                for (int j = 0; j < renderers.GetLength(1); j++)
                {
                    renderers[i, j] = RenderPoint.Block;
                }
            }
        }
    }

}
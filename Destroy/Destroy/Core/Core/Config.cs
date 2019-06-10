namespace Destroy
{
    /// <summary>
    /// 引擎配置
    /// </summary>
    public class Config
    {
        /// <summary>
        /// 配置引擎
        /// </summary>
        static Config()
        {
            //引擎
            TickPerSecond = 60;
            ScreenWidth = 30;
            ScreenHeight = 30;
            CharWidth = CharWidthEnum.Double;
            //网络
            Net = new NetConfig();
        }
        /// <summary>
        /// 引擎每秒Update的频率
        /// </summary>
        public static int TickPerSecond { get; set; }

        /// <summary>
        /// 引擎单个字符宽度
        /// </summary>
        public static CharWidthEnum CharWidth { get; set; }

        /// <summary>
        /// 网络设置
        /// </summary>
        public static NetConfig Net { get; set; }

        /// <summary>
        /// 屏幕宽度
        /// </summary>
        public static int ScreenWidth { get; set; }

        /// <summary>
        /// 屏幕高度
        /// </summary>
        public static int ScreenHeight { get; set; }

        /// <summary>
        /// 系统默认颜色
        /// </summary>
        public static Color DefaultForeColor = Color.Gray;

        /// <summary>
        /// 系统默认背景色
        /// </summary>
        public static Color DefaultBackColor = Color.Black;

        /// <summary>
        /// 比如被初始化,表示一个RenderPoint在屏幕上占有的像素点
        /// 单宽推荐值8*16 双宽推荐值16*16
        /// </summary>
        public static Vector2 RendererSize = new Vector2(16, 16);
    }

    /// <summary>
    /// 字符宽度
    /// </summary>
    public enum CharWidthEnum
    {
        /// <summary>
        /// 单宽字符(比如英文)
        /// </summary>
        Single = 1,
        /// <summary>
        /// //双宽字符(比如中文)
        /// </summary>
        Double = 2,
    }

    /// <summary>
    /// 网络设置
    /// </summary>
    public class NetConfig
    {
        /// <summary>
        /// 游戏端口
        /// </summary>
        public int GamePort;
        /// <summary>
        /// //客户端同步频率
        /// </summary>
        public int ClientSyncRate;
        /// <summary>
        /// //服务器同步频率
        /// </summary>
        public int ServerSyncRate;

        /// <summary>
        /// 
        /// </summary>
        public NetConfig()
        {
            GamePort = 8848;
            ServerSyncRate = 20;
            ClientSyncRate = 50;
        }
    }
}
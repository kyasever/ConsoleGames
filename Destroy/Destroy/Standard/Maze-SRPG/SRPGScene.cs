using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Destroy;
using Destroy.Winform;
namespace Destroy.Standard
{
    /// <summary>
    /// SRPG游戏范例
    /// </summary>
    public class SRPGScene : Scene
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public SRPGScene(string name):base(name)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnDestroy()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnStart()
        {
            //检查资源文件数量
            Resource.Init();
            Debug.Log("查找目录Resouce,资源数量:" + Resource.Name_Path.Keys.Count);
            //开启贴图转换器 GDI渲染可用,使用贴图文件代替原字符显示
            ImageConvertor.Init();

            //创建角色
            SRPGAgent wizardAgent = GameObject.CreateWith<SRPGAgent>();
            wizardAgent.LocalPosition = new Vector2(12, 12);
            //创建系统监视器
            SystemUIFactroy.GetSystemInspector(new Vector2(28, 13));
            //创建迷宫地图
            var maze = MazeGeneration.GetMaze(21, 21, new RenderPoint("■", Color.White, Config.DefaultBackColor, (int)Layer.Environment));
            maze.Position = new Vector2(5, 5);
        }
    }

}

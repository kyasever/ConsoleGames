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
        public SRPGScene():base("SRPGScene")
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
            Camera.Main.Position = Vector2.Zero;

            //检查资源文件数量
            Resource.Init();
            Debug.Log("查找目录Resouce,资源数量:" + Resource.Name_Path.Keys.Count);
            //开启贴图转换器 GDI渲染可用,使用贴图文件代替原字符显示
            ImageConvertor.Init();

            //创建动作选择框
            GameMode.curStateDlg = StateNoticeBox.Create();

            //var btn = UIFactroy.CreateButton(new Vector2(3, 3), "回调测试", ()=> { EventHandlerSystem.CreateTest(); });

            //创建角色
            SRPGAgent wizardAgent = GameObject.CreateWith<SRPGAgent>();
            wizardAgent.LocalPosition = new Vector2(12, 12);
            //创建系统监视器
            SystemUIFactroy.GetSystemInspector(new Vector2(28, 13));
            //创建迷宫地图
            var maze = MazeGeneration.GetMaze(5, 5, new RenderPoint("■", Color.White, Config.DefaultBackColor, (int)Layer.Environment));
            maze.Position = new Vector2(5, 5);
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Destroy;

namespace HealerSimulator
{
    public enum Layer
    {
        Label = -1,
        Box = 11,
        SelectBox = 10,
    }


    public class MainScene : Scene
    {

        public override void OnStart()
        {
            //系统监视窗,之后也要出editor版本,更详细,信息量更大
            var inspector = SystemUIFactroy.GetSystemInspector(new Vector2(50,0));

            //团队框架,盯着这个加血
            GameObject.CreateWith<TeamGridPanel>("TeamGridHuge","GameState",localPosition:new Vector2(0,20));

            //角色面板,玩家控制的角色的技能状态
            GameObject.CreateWith<PlayerPanel>("PlayerPanel", "GameState", localPosition: new Vector2(1, 12));

            //焦点面板,上面是玩家信息,下面是焦点信息
            GameObject.CreateWith<FocusPanel>("FocusPanel", "GameState", localPosition: new Vector2(1 , 1));

            //Boss面板
            GameObject.CreateWith<BossHUD>("BossUI", "GameState", localPosition: new Vector2(1, 36));

            //控制器也是创建出来,然后控制器自己去取数据
            GameObject.CreateWith<PlayerController>("PlayerController", "Controller");
        }
    }






}

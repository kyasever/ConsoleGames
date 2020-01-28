using System;
using System.Collections.Generic;
using System.Linq;
using Destroy;
using Destroy.Standard;
using Destroy.Winform;

namespace DiceBattle
{
    static class Program
    {
        private static void Main() {
            Config.TickPerSecond = 60;
            Config.ScreenWidth = 50;
            Config.ScreenHeight = 30;
            //用编辑器模式开始游戏
            WinformEngine.OpenWithEditor(StartGame, "HealerSimulator");
        }

        private static void StartGame() {

            UIFactroy.CreateLabel(new Vector2(2, 2), "123asd");

            GameObject.CreateWith<Hero>();

            //开启游戏进入场景
            //StartScene scene = new StartScene();
            //SceneManager.Load(scene, LoadSceneMode.Single);
            //游戏的主场景本质上不控制任何逻辑,只是与数据进行绑定- 更改 - 显示等.
            //游戏的数据都由gameMode说的算
        }
    }

    /*
    //做一个最简单的游戏. ai只有前进和攻击
    //攻击通常只能攻击自己的前方.和斜方
    //每一个点上的前位置不能空置.
    //如果选择移动,那么一定会加入队尾
    
    每个单位每tick获得固定的1行动力. 根据自身的移动速度和攻速来判断消耗行动力的值
    允许一个点上站双方的兵. 通常不允许穿过有敌人的位置.
    近战射程为0 远程射程>1(通常不能贴脸攻击)

    将ai和技能还有属性都组件化. 然后每一个rouge单位都是组件拼起来的.

    当中点的某方人数是敌人的二倍,那么则可以前进(暂定)
    近战的射程本质上是差值的绝对值. 比如5Vs3 
    左队0号位射程1,那么它可以攻击对方的0,1,3(0-1)全部三个位置

    */

    //TODO: 之后可以搞一个manager类来代替script.
    public class GameMode : Script
    {
        private static GameMode instance;
        public static GameMode Instance {
            get {
                if (instance == null) {
                    instance = GameObject.CreateWith<GameMode>("GameMode");
                }
                return instance;
            }
        }


        public List<Hero> leftTeam;
        public List<Hero> rightTeam;

        public int battleWidth = 5;
        public int battleLength = 10;

        public override void Awake() {

        }
    }

    public class TeamGround
    {
        public int battleWidth = 4;
        public int battleLength = 9;

        private List<Ground> BattleGound = new List<Ground>();

        //只留个这个. 和movein
        public bool CanMoveIn(int pos) {
            return true;
        }

        //d对于控制器来说 其实只有Forward和Back两种.
        //还可以增加一个Attack判定. 控制器里增加一个dir参数.乘一下就好了




        public bool CanForward(Hero hero) {
            return (hero.dir && hero.pos < battleLength && BattleGound[hero.pos+1].CanAddTo(hero))
                || (!hero.dir && hero.pos > 0 && BattleGound[hero.pos - 1].CanAddTo(hero));
        }

        public void Forward(Hero hero) {
            if (CanForward(hero)) {

            }
        }


        public class Ground
        {
            public List<Hero> leftTeam = new List<Hero>();
            public List<Hero> rightTeam = new List<Hero>();
            public int maxCount;

            public bool CanAddTo(Hero hero) {
                return (hero.dir && leftTeam.Count < maxCount) ||
                   (!hero.dir && rightTeam.Count < maxCount);
            }

        }
    }


    public class Hero : Script
    {
        public bool dir = true;
        public int pos;

        public string pic = "狂";

        public override void Awake() {
            var o1 = UIUtils.CreatePointObj(pic);
            o1.SetParent(GameObject);
            var o2 = ProgressBar.Create(5);
            o2.SetParent(GameObject);
            o2.LocalPosition = Vector2.Right;

            Position = new Vector2(10, 10);
        }
    }
}

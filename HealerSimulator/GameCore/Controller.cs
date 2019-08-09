using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Destroy;

namespace HealerSimulator
{
    /// <summary>
    /// 控制类基类,具有一些api
    /// </summary>
    public class Controller
    {
        /// <summary>
        /// Characer和其Skill是由纯数据组成的,Controller负责操作它们的数据
        /// </summary>
        protected Character c;
        protected GameMode game;



        /// <summary>
        /// 需要手动调用这个函数来给Controller指明其负责的对象
        /// </summary>
        public Controller(Character c)
        {
            this.c = c;
            game = GameMode.Instance;
            //将自己的Update托管给gamemode,然后场景从gameMode取数据进行更新
            game.UpdateEvent += DefaultUpdate;
        }

        protected float tickTime = 0;

        public virtual void TickPerSecond()
        {
        }

        protected void DefaultUpdate()
        {
            if (c == null)
            {
                return;
            }

            //驱动每秒事件
            tickTime -= Time.DeltaTime;
            if (tickTime < 0)
            {
                tickTime = 1f;
                TickPerSecond();
            }
            //驱动每帧事件
            Update();
        }

        public virtual void Update()
        {

        }

    }

    /// <summary>
    /// 玩家角色的控制类,操控都由这里做出.PlayerHUD只负责显示玩家状态
    /// 会有AIController 具有完整的操作权限,还有RobotController 只负责结算
    /// </summary>
    public class PlayerController : Controller
    {
        public PlayerController(Character c) : base(c)
        {

        }

        /// <summary>
        /// 每秒触发一次,结算每秒的操作
        /// </summary>
        public override void TickPerSecond()
        {
            c.MP += 20;
        }

        /// <summary>
        /// 每帧结算一次,结算CD和施法操作等
        /// </summary>
        public override void Update()
        {
            //减少CD
            foreach (Skill s in c.SkillList)
            {
                if (s.CDRelease > 0)
                {
                    s.CDRelease -= Time.DeltaTime;
                }
            }

            //推动施法进度条,当技能施法时间结束时,释放这个技能
            if (c.IsCasting)
            {
                c.CastingSkill.CastingRelease -= Time.DeltaTime;
                if (c.CastingSkill.CastingRelease < 0)
                {
                    //技能出手,没有公cd
                    game.CastSkill(c.CastingSkill, game.FocusCharacter);
                    c.CastingSkill = null;
                }
                //空格键打断当前施法
                if (Input.GetKeyDown(ConsoleKey.Spacebar))
                {
                    c.CastingSkill = null;
                }
            }

            //如果公cd> 0 那么处理公cd且不能释放别的技能
            if (c.CommonTime > 0f)
            {
                c.CommonTime -= Time.DeltaTime;
                return;
            }

            //按下对应的按键后执行对应的操作
            foreach (Skill s in c.SkillList)
            {
                //按下对应按键且可以释放(蓝耗,技能,目标等没问题)
                if (Input.GetKeyDown(s.Key) && s.CanCast)
                {
                    //瞬发技能,直接释放,并进入公cd
                    if (s.IsInstant)
                    {
                        //打断当前读条技能
                        if (c.IsCasting)
                        {
                            c.CastingSkill = null;
                        }
                        game.CastSkill(s, game.FocusCharacter);
                        c.CommonTime = c.CommonInterval;
                    }
                    //读条技能,开始读条
                    else
                    {
                        //读条技能不能打断读条技能
                        if (c.IsCasting)
                            return;
                        //进入公cd
                        c.CommonTime = c.CommonInterval;
                        //开始读条
                        c.CastingSkill = s;
                        s.CastingRelease = s.CastingInterval;
                    }
                }
            }
        }
    }

    public static class BossSkillCaster
    {
        public static void CastAOESkill(Skill s, GameMode game)
        {
            foreach (var v in game.TeamCharaters)
            {
                game.CastSkill(s, v);
            }
        }


    }

    /// <summary>
    /// 这是一个boss 控制器,其难度由初始化参数设定
    /// </summary>
    public class BossController : Controller
    {
        public BossController(Character c , int diffcultyLevel) : base(c)
        {
            //难度等级每增加10 boss的输出增加1倍
            float miuti = (float)Math.Sqrt( 1 + diffcultyLevel * 0.1);
            c.Speed = miuti;

            c.SkillList = new List<Skill>();
            var skill = Skill.CreateNormalHitSkill(c, "地震", (int)(30 * miuti) , 2f);
            skill.CDRelease = 1f;
            skill.OnCastEvent += BossSkillCaster.CastAOESkill;
            c.SkillList.Add(skill);

            skill = Skill.CreateNormalHitSkill(c, "重击", (int)(1200 * miuti), 20f);
            skill.OnCastEvent += CastSkill2;
            c.SkillList.Add(skill);

            skill = Skill.CreateNormalHitSkill(c, "流火", (int)(450 * miuti), 20f);
            skill.CDRelease = 10f;
            skill.OnCastEvent += CastSkill3;
            c.SkillList.Add(skill);

        }

        // 2技能 打坦克
        public void CastSkill2(Skill s, GameMode game)
        {
            game.CastSkill(s, GetTank());
        }

        /// <summary>
        /// 获得一个坦克单位,如果没有,那么打第一个人
        /// </summary>
        private Character GetTank()
        {
            foreach(var v in game.TeamCharaters)
            {
                if(v.Duty == TeamDuty.Tank)
                {
                    return v;
                }
            }
            return game.TeamCharaters[0];
        }

        // 3技能 打dps 取决于角色的控制力
        public static void CastSkill3(Skill s, GameMode game)
        {
            foreach (var v in game.TeamCharaters)
            {
                game.CastSkill(s, v);
            }
        }

        //卡cd释放技能
        public override void Update()
        {
            foreach(var s in c.SkillList)
            {
                if(s.CDRelease < 0)
                {
                    s.OnCastEvent.Invoke(s,game);
                    s.CDRelease = s.CD;
                }
            }

        }


    }
}

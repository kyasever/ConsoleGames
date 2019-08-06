using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Destroy;

namespace HealerSimulator
{
    /// <summary>
    /// 玩家角色的控制类,操控都由这里做出.PlayerHUD只负责显示玩家状态
    /// 会有AIController 具有完整的操作权限,还有RobotController 只负责结算
    /// </summary>
    public class PlayerController : Script
    {
        /// <summary>
        /// Controller没有操作数据的权限,Characer是纯数据组成的. SKill也是纯数据组成的
        /// </summary>
        private Character c;

        private GameMode game;

        /// <summary>
        /// 每秒触发一次,结算每秒的操作
        /// </summary>
        public void TickPerSecond()
        {
            c.MP += 20;
        }


        public override void Start()
        {
            game = GameMode.Instance;
            c = game.Player;
            c.HP = 100;
        }

        private float tickTime = 0;

        /// <summary>
        /// 每帧结算一次,结算CD和施法操作等
        /// </summary>
        public override void Update()
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
                c.CurSkill.CastingRelease -= Time.DeltaTime;
                if (c.CurSkill.CastingRelease < 0)
                {
                    //技能出手,没有公cd
                    game.CastSkill(c.CurSkill, game.FocusCharacter);
                    c.IsCasting = false;
                    c.CurSkill = null;
                }
            }

            //当施法中按下空格,中止释放这个技能
            if (c.IsCasting && Input.GetKeyDown(ConsoleKey.Spacebar))
            {
                c.IsCasting = false;
                c.CurSkill = null;
            }

            //减少公CD时间
            if (!c.CanCast)
            {
                c.CommonTime -= Time.DeltaTime;
                if (c.CommonTime < 0f)
                {
                    c.CanCast = true;
                }
                else
                {
                    return;
                }
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
                            c.IsCasting = false;
                            c.CurSkill = null;
                        }
                        game.CastSkill(s, game.FocusCharacter);
                        c.CommonTime = c.CommonInterval;
                        c.CanCast = false;
                    }
                    //读条技能,开始读条
                    else
                    {
                        //读条技能不能打断读条技能
                        if (c.IsCasting)
                            return;
                        //进入公cd
                        c.CommonTime = c.CommonInterval;
                        c.CanCast = false;
                        //开始读条
                        c.IsCasting = true;
                        c.CurSkill = s;
                        s.CastingRelease = s.CastingInterval;
                    }
                }
            }
        }
    }

}

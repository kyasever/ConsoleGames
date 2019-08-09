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

            //如果检测到控制的人挂了,那么解除关系
            if(!c.IsAlive)
            {
                game.TeamCharacters.Remove(c);
                game.DeadCharacters.Add(c);
                c.controller = null;
                c = null;
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

    public static class SkillCaster
    {
        /// <summary>
        /// 一个单位受到一个技能的攻击,返回伤害,日后可以改成返回结果
        /// </summary>
        /// <param name="s">攻击方的技能,日后可以改成一个新的结构体,包含攻击放技能的数据</param>
        /// <param name="target">防守方的角色目标</param>
        /// <returns>伤害,日后改成结果结构体</returns>
        public static int HitCharacter(Skill s, Character target)
        {
            target.HP -= s.Atk;
            return s.Atk;
        }

        //加血
        public static int HealCharacter(Skill s, Character target)
        {
            target.HP += s.Atk;
            return s.Atk;
        }

        //向目标群体发动攻击
        public static void CastAOESkill(Skill s, List<Character> target)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} 释放了 {1} ", s.Caster.CharacterName, s.skillName);

            //消耗蓝
            s.Caster.MP -= s.MPCost;

            //掉血
            foreach (var t in target)
            {
                int damage = HitCharacter(s, t);
                sb.AppendFormat("对{0} | 造成了:{1}伤害", t.CharacterName, damage.ToString());
            }

            //进入CD
            if (s.CDDefault > 0)
            {
                s.CDRelease = s.CD;
            }
            //输出结果
            Debug.Log(sb.ToString());
        }

        //向目标发动单体攻击
        public static void CastSingleSkill(Skill s, Character target)
        {
            if(target == null)
            {
                return;
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} 释放了 {1} ", s.Caster.CharacterName, s.skillName);

            //消耗蓝
            s.Caster.MP -= s.MPCost;

            //掉血
            int damage = HitCharacter(s, target);
            sb.AppendFormat("对{0} | 造成了:{1}伤害", target.CharacterName, damage.ToString());

            //进入CD
            if (s.CDDefault > 0)
            {
                s.CDRelease = s.CD;
            }
            //输出结果
            Debug.Log(sb.ToString());
        }

    }

    public class NPCController : Controller
    {
        public NPCController(Character c) : base(c)
        { 
        }

        public override void TickPerSecond()
        {
            GameMode.Instance.Boss.HP -= GameMode.RandomInstance.Next(64, 144);
        }
    }

    /// <summary>
    /// 这是一个boss 控制器,其难度由初始化参数设定
    /// </summary>
    public class BossController : Controller
    {
        /// <summary>
        /// 难度等级每增加10 boss的输出增加1倍
        /// </summary>
        /// <param name="c"></param>
        /// <param name="diffcultyLevel"></param>
        public BossController(Character c, int diffcultyLevel) : base(c)
        {
            float miuti = (float)Math.Sqrt(1 + diffcultyLevel * 0.1);
            c.Speed = miuti;

            c.SkillList = new List<Skill>();
            var skill = Skill.CreateNormalHitSkill(c, "地震", (int)(30 * miuti), 2f);
            skill.CDRelease = 1f;
            skill.OnCastEvent += CastSkill1;
            c.SkillList.Add(skill);

            skill = Skill.CreateNormalHitSkill(c, "重击", (int)(1200 * miuti), 20f);
            skill.CDRelease = 10f;
            skill.OnCastEvent += CastSkill2;
            c.SkillList.Add(skill);

            skill = Skill.CreateNormalHitSkill(c, "流火", (int)(450 * miuti), 20f);
            skill.CDRelease = 20f;
            skill.OnCastEvent += CastSkill3;
            c.SkillList.Add(skill);

        }

        private void CastSkill1(Skill s)
        {
            SkillCaster.CastAOESkill(s, game.TeamCharacters);
        }

        private void CastSkill2(Skill s)
        {
            SkillCaster.CastSingleSkill(s, GetTank());
        }

        private void CastSkill3(Skill s)
        {
            List<Character> list = new List<Character>();
            foreach (var c in game.TeamCharacters)
            {
                if (c.CanHit(0f))
                {
                    list.Add(c);
                }
            }
            SkillCaster.CastAOESkill(s, list);
        }

        /// <summary>
        /// 获得一个坦克单位,如果没有,那么打第一个人
        /// </summary>
        private Character GetTank()
        {
            foreach (var v in game.TeamCharacters)
            {
                if (v.Duty == TeamDuty.Tank)
                {
                    return v;
                }
            }
            if (game.TeamCharacters.Count > 0)
                return game.TeamCharacters[0];
            else
                return null;
        }


        //卡cd释放技能
        public override void Update()
        {
            if(game.TeamCharacters.Count == 0)
            {
                return;
            }
            foreach (var s in c.SkillList)
            {
                s.CDRelease -= Time.DeltaTime;
                if (s.CDRelease < 0)
                {
                    s.OnCastEvent.Invoke(s);
                    s.CDRelease = s.CD;
                }

            }

        }
    }

    /// <summary>
    /// 游戏控制器,判断游戏是否结束了
    /// </summary>
    public class GameContrtoller
    {
        private GameMode game;

        public GameContrtoller()
        {
            game = GameMode.Instance;
            GameMode.Instance.UpdateEvent += Update;
        }

        private bool Enable = true;

        public void Update()
        {
            if (!Enable)
                return;
            //全死光了
            if(game.TeamCharacters.Count == 0)
            {
                Debug.Log("游戏结束");
                ReturnScene();
                Enable = false;
            }
            else if(!game.Boss.IsAlive)
            {
                Debug.Log("游戏胜利");
                ReturnScene();
                Enable = true;
            }
        }

        private void ReturnScene()
        {
            game.Clear();
            SceneManager.Load(new StartScene(), LoadSceneMode.Single);
        }
    }
}

namespace Destroy
{
    using System;

    /// <summary>
    /// 按键控制器,按一下动一格
    /// </summary>
    public class KeyDownController : Script
    {
        /// <summary>
        /// Update
        /// </summary>
        public override void Update()
        {
            if (Input.GetKeyDown(ConsoleKey.A))
            {
                Position += new Vector2(-1, 0);
            }
            else if (Input.GetKeyDown(ConsoleKey.D))
            {
                Position += new Vector2(1, 0);
            }
            else if (Input.GetKeyDown(ConsoleKey.W))
            {
                Position += new Vector2(0, 1);
            }
            else if (Input.GetKeyDown(ConsoleKey.S))
            {
                Position += new Vector2(0, -1);
            }
        }
    }

    /// <summary>
    /// 简单控制器(每帧进行移动)
    /// </summary>
    public class SimpleController : Script
    {
        /// <summary>
        /// 
        /// </summary>
        public override void Update()
        {
            int x = Input.GetDirectInput(ConsoleKey.A, ConsoleKey.D);
            int y = Input.GetDirectInput(ConsoleKey.S, ConsoleKey.W);

            if (x > 0)
                Position += new Vector2(1, 0);
            if (x < 0)
                Position += new Vector2(-1, 0);
            if (y > 0)
                Position += new Vector2(0, 1);
            if (y < 0)
                Position += new Vector2(0, -1);
        }
    }

    /// <summary>
    /// 角色控制器 有比较完整的运动控制
    /// 可以通过Speed调整速度
    /// 可以通过CanMoveInCollider调整是否可以进入碰撞体
    /// </summary>
    public class CharacterController : Script
    {
        private Vector2Float input = Vector2Float.Zero;

        private Vector2Float lastInput = Vector2Float.Zero;

        /// <summary>
        /// 
        /// </summary>
        public Vector2 Direction = Vector2.Zero;
        /// <summary>
        /// 要控制的角色的速度
        /// </summary>
        public int Speed;

        /// <summary>
        /// 是否可以穿过碰撞体.
        /// </summary>
        public bool CanMoveInCollider = false;

        /// <summary>
        /// 当前处于的浮点数位置
        /// </summary>
        public Vector2Float FPosition;

        /// <summary>
        /// 构造 不要new
        /// </summary>
        public CharacterController()
        {
            FPosition = Vector2Float.Zero;
            Speed = 10;
        }

        /// <summary>
        /// 碰撞检测 测试用
        /// </summary>
        /// <param name="collision"></param>
        public override void OnCollision(Collision collision)
        {
            //可以像这样来检测碰撞
            Debug.Log(collision.HitPos.ToString());
        }

        /// <summary>
        /// Update
        /// </summary>
        public override void Update()
        {
            int x = Input.GetDirectInput(ConsoleKey.A, ConsoleKey.D);
            int y = Input.GetDirectInput(ConsoleKey.S, ConsoleKey.W);
            input = new Vector2Float(x, y);

            if (input == Vector2Float.Zero)
            {
                FPosition = Vector2Float.Zero;
            }
            //启动的时候会送半程的瞬移加速,使反应更灵敏
            //理论上来讲间或的按键可能会比连续按住移动更快...
            else if (input != Vector2Float.Zero && lastInput == Vector2Float.Zero)
            {
                FPosition = input * 0.5f;
                FPosition += input.Normalized * Speed * Time.DeltaTime;
            }
            else
            {
                FPosition += input.Normalized * Speed * Time.DeltaTime;
            }

            //浮点溢出之后进行移动 相当于原来的Rigid
            int dx = 0, dy = 0;
            if (FPosition.X > 1)
            {
                FPosition.X -= 1;
                dx += 1;
            }
            else if (FPosition.X < -1)
            {
                FPosition.X += 1;
                dx -= 1;
            }

            if (FPosition.Y > 1)
            {
                FPosition.Y -= 1;
                dy += 1;
            }
            else if (FPosition.Y < -1)
            {
                FPosition.Y += 1;
                dy -= 1;
            }

            Vector2 tomove = new Vector2(dx, dy);
            //如果开启了连续碰撞模式,或者真的产生了位移,才会发送位移请求
            if (tomove != Vector2.Zero)
            {
                if (!CanMoveInCollider)
                {
                    //先去物理系统检测是否可以移动,检测的过程中不会移动物体,但是会产生碰撞回调
                    if (!RuntimeEngine.GetSystem<CollisionSystem>().CanMoveInPos(GetComponent<Collider>(), Position, Position + tomove))
                    {
                        FPosition = Vector2Float.Zero;
                    }
                    //接下来进行移动
                    else
                    {
                        Position += tomove;
                    }
                }
                else
                {
                    Position += tomove;
                }
            }
            lastInput = input;
        }
    }
}
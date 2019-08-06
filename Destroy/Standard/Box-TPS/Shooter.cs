using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destroy.Standard
{

    /// <summary>
    /// 枪手 可以发射子弹
    /// </summary>
    public class Shooter : Script
    {
        private CharacterController characterController;

        /// <summary>
        /// 射击模式 两种枪目前射击的子弹时一样的.都可以一次打掉一个砖块.暂定把那个迷宫拿来打吧 那个看起来很合适 很多
        /// </summary>
        public enum ShootMode
        {
            /// <summary>
            /// 左轮 按一下射一下 默认装弹6(左轮)
            /// </summary>
            Revolver,
            /// <summary>
            /// 步枪 按住了一直射 默认装弹16(UI只准备了16个格子)
            /// </summary>
            Rifle 
        }
        public ShootMode shootMode =  ShootMode.Rifle;

        /// <summary>
        /// 
        /// </summary>
        public override void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        private float shootTime = 0;
        private float rifilInterval = 0.3f;
        private float revolverInterval = 1f;

        /// <summary>
        /// 
        /// </summary>
        public override void Update()
        {
            if(shootMode == ShootMode.Revolver)
            {
                shootTime += Time.DeltaTime;
                if (Input.GetKeyDown(ConsoleKey.Spacebar))
                {
                    if (shootTime > rifilInterval)
                    {
                        Shoot();
                        shootTime = 0f;
                    }
                }
            }
            else if(shootMode == ShootMode.Rifle)
            {
                shootTime += Time.DeltaTime;
                if(Input.GetKey( ConsoleKey.Spacebar))
                {
                    if(shootTime > rifilInterval)
                    {
                        Shoot();
                        shootTime = 0f;
                    }
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void Shoot()
        {
            if(GunBox.Instance.CurrertAM > 0)
            {
                var bullet = GameObject.CreateWith<Bullet>("Bullet","Bullet");
                bullet.createrCollider = GetComponent<Collider>();
                bullet.Position = Position;
                bullet.Speed = (Vector2Float)(characterController.Direction * 20);
                GunBox.Instance.CurrertAM--;
            }

        }
    }

    /// <summary>
    /// 一个会自动飞行的子弹对象
    /// </summary>
    public class Bullet : Script
    {
        internal Collider createrCollider;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="collision"></param>
        public void OnCollision(Collision collision)
        {
            if(collision.OtherCollier == createrCollider)
            {
                return;
            }
            else
            {
                GameObject other = collision.OtherCollier.GameObject;
                Debug.Log("Bullet" + collision.HitPos);
                //将对方接触点的Mesh移除
                //这里有严重的问题 就是Mesh和Collider绑定的问题
                other.PosList.Remove(collision.HitPos - other.Position);
                //这是一个补丁.之后需要重写
                RuntimeEngine.GetSystem<CollisionSystem>().Colliders[collision.HitPos].Remove(collision.OtherCollier);
                other.GetComponent<Renderer>().RendererPoints.Remove(collision.HitPos - other.Position);
                //如果对方被打没了,则销毁对方的对象
                if (other.PosList.Count == 0)
                {
                    GameObject.Destroy(other);
                }
                GameObject.Destroy(GameObject);
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Awake()
        {
            var collider = AddComponent<Collider>();
            collider.OnCollisionEvent += OnCollision;
            var renderer =  AddComponent<Renderer>();
            renderer.Rendering(new RenderPoint("◉", (int)Layer.Agent));
        }

        private float releaseTime = 5f;

        public Vector2Float FPosition = Vector2Float.Zero;
        public Vector2Float Speed = Vector2Float.Zero;

        /// <summary>
        /// 
        /// </summary>
        public override void Update()
        {
            releaseTime -= Time.DeltaTime;
            if(releaseTime < 0)
            {
                GameObject.Destroy(GameObject);
            }
            FPosition += Speed * Time.DeltaTime;
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
            Position += new Vector2(dx, dy);
        }
    }
}

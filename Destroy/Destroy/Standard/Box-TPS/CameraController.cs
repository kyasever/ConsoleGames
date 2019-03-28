using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destroy.Standard
{
    /// <summary>
    /// 相机跟随组件
    /// </summary>
    public class CameraController : Script
    {
        /// <summary>
        /// 跟随的目标
        /// </summary>
        public GameObject followTrans;

        /// <summary>
        /// 
        /// </summary>
        public override void Update()
        {
            if (followTrans == null)
                return;
            Camera.Main.Position = followTrans.Position - new Vector2(Config.ScreenWidth, Config.ScreenHeight)/2;
        }

    }
}

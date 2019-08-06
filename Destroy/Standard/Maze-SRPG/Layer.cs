using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destroy.Standard
{
    /// <summary>
    /// Layer划分,越上面的显示的越靠前,数字可以随意改动,但是最好使用枚举来进行数字排列避免出错
    /// </summary>
    public enum Layer
    {
        /// <summary>
        /// 
        /// </summary>
        Cursor = 8,
        /// <summary>
        /// 
        /// </summary>
        Environment = 9,
        /// <summary>
        /// 
        /// </summary>
        Agent = 10,
        /// <summary>
        /// 
        /// </summary>
        MoveRoute = 11,
        /// <summary>
        /// 
        /// </summary>
        MoveAera = 12,
    }
}

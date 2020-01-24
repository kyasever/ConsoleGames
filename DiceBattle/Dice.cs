using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceBattle
{
    /// <summary>
    /// 骰子可以投出的面的种类
    /// 目前对应的贴图和spritesheet的顺序是一样的.
    /// </summary>
    public enum DiceSide
    {
        紫能量I,
        紫能量II,
        紫空,

        红力量,
        红技巧,
        红空,

        蓝智慧,
        蓝观察,
        蓝空,

        黄空,
        黄稳定,
        黄迅速,

        灰空,
        灰工程路障,
        灰火炮,
        灰重力锤,

        绿空,
        绿炮弹,
        绿建材,
    }

}

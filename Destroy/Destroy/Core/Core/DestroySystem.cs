namespace Destroy
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 所有系统的基类，继承这个类就可以AddSystem到RuntimeEngine上。
    /// </summary>
    public abstract class DestroySystem
    {
        /*
         * 系统的执行顺序逻辑
        这个也有一部分生命周期管理的目的,执行顺序绝对是必要的,请保留这部分内容.可以不重写但不能没有
        构造函数 - 初始化一些变量等等.在这个system被addsystem之前执行
        */

        /// <summary>
        /// 初步的初始化,在这个system被addsystem之后立即执行
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// 在外部初始化完毕之后按照顺序统一执行
        /// </summary>
        public virtual void Start() { }

        /// <summary>
        /// 这个系统是否需要执行Update
        /// </summary>
        public bool needUpdate = true;

        /// <summary>
        /// 每帧执行.可以通过在初始化的时候改变needUpdate关闭.
        /// </summary>
        public virtual void Update() { }
    }
}
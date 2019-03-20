namespace Destroy
{
    /*
     * 最后更新:2018.12.15
     * 修改:by Charlie
     */

    /// <summary>
    /// 所有脚本的基类,通常开发者继承这个类来自定义组件
    /// </summary>
    public abstract class Script : Component
    {
        /// <summary>
        /// 相当于Initialize 在添加的时候进行调用
        /// </summary>
        public virtual void Awake() { } 

        /// <summary>
        /// 在生命周期的开始调用. 如果在Update中创建,那么就在下一次生命周期的时候调用
        /// </summary>
        public virtual void Start() { }

        /// <summary>
        /// 每帧调用一次
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// 延迟调用一个方法(该方法必须为实例无参public方法)
        /// </summary>
        public void Invoke(string methodName, float delayTime)
        {
            RuntimeEngine.GetSystem<InvokeSystem>().AddInvokeRequest(this, methodName, delayTime);
        }

        /// <summary>
        /// 取消一个延迟调用的方法
        /// </summary>
        public void CancleInvoke(string methodName)
        {
            RuntimeEngine.GetSystem<InvokeSystem>().CancleInvokeRequest(this, methodName);
        }

        /// <summary>
        /// 该方法是否在延迟调用
        /// </summary>
        public bool IsInvoking(string methodName)
        {
            return RuntimeEngine.GetSystem<InvokeSystem>().IsInvoking(this, methodName);
        }

        //在这里执行Awake
        internal override void Initialize()
        {
            Awake();
        }

        internal override void OnAdd()
        {
            GameObject.OnCollisionEvent += OnCollisionReceive;
            RuntimeEngine.GetSystem<StartSystem>().StartScriptCollection.Add(this);
        }

        internal override void OnRemove()
        {
            GameObject.OnCollisionEvent -= OnCollisionReceive;
            bool suc = RuntimeEngine.GetSystem<UpdateSystem>().UpdateScriptCollection.Remove(this);
            if (!suc)
                RuntimeEngine.GetSystem<StartSystem>().StartScriptCollection.Remove(this);
        }

        //用这个来判断是否活跃,不活跃就不接收碰撞回调了
        private void OnCollisionReceive(Collision collision)
        {
            if(Enable)
            {
                OnCollision(collision);
            }
        }

        /// <summary>
        /// 重载来接收碰撞回调消息
        /// </summary>
        public virtual void OnCollision(Collision collision)
        {

        }

    }
}
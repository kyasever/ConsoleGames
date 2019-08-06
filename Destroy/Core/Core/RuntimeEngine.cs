namespace Destroy
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// 运行时引擎, 接管整个游戏的生命周期 <see langword="static"/>
    /// </summary>
    public static class RuntimeEngine
    {
        /// <summary>
        /// 通过外部调用来终止引擎
        /// </summary>
        public static bool Enabled { get; set; }

        /// <summary>
        /// 初始化操作，系统按顺序执行Init操作
        /// </summary>
        internal static List<Action> StartActions { get; private set; }

        /// <summary>
        /// 更新操作，系统按顺序执行Update操作
        /// </summary>
        internal static List<Action> UpdateActions { get; private set; }

        /// <summary>
        /// 保存着系统的引用
        /// </summary>
        private static Dictionary<Type, DestroySystem> systemDict;

        /// <summary>
        /// 引擎的初始化和引擎内部系统的添加
        /// </summary>
        public static void Init()
        {
            if (Config.TickPerSecond <= 0)
                throw new Exception("帧率设置错误");

            Enabled = true;
            StartActions = new List<Action>();
            UpdateActions = new List<Action>();
            systemDict = new Dictionary<Type, DestroySystem>();

            //初始化根类型
            TypeRootConverter.Init();



            //Start系统
            AddSystem<StartSystem>();

            //物理系统
            AddSystem<CollisionSystem>();

            //Update系统(必须放在Input之前才能正常使用Input.GetXXXUp)
            AddSystem<UpdateSystem>();

            //按键输入系统
            AddSystem<InputSystem>();

            //鼠标检测系统
            AddSystem<EventHandlerSystem>();

            //调度系统
            AddSystem<InvokeSystem>();

            //渲染系统
            AddSystem<RendererSystem>();

            //删除系统
            AddSystem<DeleteSystem>();

            //初始化Scene 创建游戏默认的两个Scene
            SceneManager.Init();
        }

        /// <summary>
        /// 设置系统的执行顺序和优先级,这部分操作考虑交给中间层来做
        /// </summary>
        public static void SetSystemUpdate()
        {
            foreach (DestroySystem system in systemDict.Values)
            {
                //在这里按照顺序调用start,start可为空,并按顺序将系统加入update,默认是加入的,如果不要则在初始化的时候将needupdate改成false
                StartActions.Add(system.Start);
                if (system.needUpdate)
                    UpdateActions.Add(system.Update);
            }
        }

        /// <summary>
        /// 开始运行游戏. 执行系统的Start循环执行系统的Update
        /// </summary>
        public static void Run()
        {
            foreach (Action action in StartActions)
            {
                action.Invoke();
            }
            //每帧应该使用的时间(单位:秒)
            float tickTime = (float)1 / Config.TickPerSecond;
            //这一帧距离上一帧的时间
            float deltaTime = 0;

            while (Enabled)
            {
                //设置Time类属性
                Time.DeltaTime = deltaTime;
                Time.TotalTime += Time.DeltaTime;

                long frequence = 0;
                long startPoint = 0;
                long stopPoint = 0;
                long systemStartPoint = 0;

                //获取系统时钟频率
                Windows.Windows.QueryPerformanceFrequency(ref frequence);
                //初始计数
                Windows.Windows.QueryPerformanceCounter(ref startPoint);
                Windows.Windows.QueryPerformanceCounter(ref systemStartPoint);

                //更新所有系统
                foreach (Action action in UpdateActions)
                {
                    //在这里打断点可以查看最终排版的Update生命周期
                    action.Invoke();
                    //统计每个系统的操作用时
                    Windows.Windows.QueryPerformanceCounter(ref stopPoint);
                    //单个系统运行时间
                    float singleSystemSpendTime = (stopPoint - systemStartPoint) / (float)(frequence);
                    systemStartPoint = stopPoint;

                    string targetName = action.Target?.GetType().Name;
                    string methodName = action.Method?.Name;
                    if (methodName == "Update")
                        methodName = "";

                    string str = targetName + methodName;
                    if (Time.SystemsSpendTimeDict.ContainsKey(str))
                        Time.SystemsSpendTimeDict[str] = singleSystemSpendTime;
                    else
                        Time.SystemsSpendTimeDict.Add(str, singleSystemSpendTime);
                }

                Windows.Windows.QueryPerformanceCounter(ref stopPoint);
                float allSystemsSpendTime = (stopPoint - startPoint) / (float)(frequence);
                Time.AllSystemsSpendTime = allSystemsSpendTime;

                while (allSystemsSpendTime < tickTime) //Wait
                {
                    Thread.Sleep(0); //短暂让出线程防止死循环
                    Windows.Windows.QueryPerformanceCounter(ref stopPoint);
                    allSystemsSpendTime = (stopPoint - startPoint) / (float)(frequence);
                }

                deltaTime = allSystemsSpendTime;
            }
        }

        /// <summary>
        /// 将系统加入引擎总生命周期 完 全 一 致
        /// </summary>
        public static T AddSystem<T>() where T : DestroySystem, new()
        {
            Type type = typeof(T);
            Type root = TypeRootConverter.GetSystemRoot(type);

            if (root == null)
                throw new Exception("未知错误");

            if (systemDict.ContainsKey(root))
            {
                string rootName = nameof(root);

                throw new Exception($"你已经添加了继承了{rootName}类型的系统{systemDict[root]}, " +
                    $"无法继续添加继承{rootName}类型的系统.");
            }

            T instance = new T();
            systemDict.Add(root, instance);

            //初始化组件
            instance.Initialize();

            return instance;
        }

        /// <summary>
        /// 获取系统,理论上来讲这个方法是不应该被滥用的
        /// </summary>
        public static T GetSystem<T>() where T : DestroySystem
        {
            Type type = typeof(T);
            Type root = TypeRootConverter.GetSystemRoot(type);

            if (root == null)
                throw new Exception("未知错误");

            if (systemDict == null)
                return null;
            if (!systemDict.ContainsKey(root))
                return null;
            else
                return systemDict[root] as T;
        }
    }
}
namespace Destroy.Example
{
    /// <summary>
    /// 继承该类获得单例
    /// </summary>
    public class Singleton<T> where T : new()
    {
        private static T instance;

        /// <summary>
        /// 单例
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance == null)
                    instance = new T();
                return instance;
            }
        }
    }
}
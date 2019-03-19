namespace Destroy
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 对象池
    /// </summary>
    public class ObjectPool
    {
        private Instantiate instantiate;

        private readonly List<GameObject> pool;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instantiate"></param>
        public ObjectPool(Instantiate instantiate)
        {
            this.instantiate = instantiate;
            pool = new List<GameObject>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        public void PreAllocate(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject instance = instantiate();
                ReturnInstance(instance);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public GameObject GetInstance()
        {
            if (pool.Any())
            {
                GameObject instance = pool.First();
                pool.Remove(instance);
                instance.SetActive(true);
                return instance;
            }
            return instantiate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        public void ReturnInstance(GameObject instance)
        {
            instance.SetActive(false);
            pool.Add(instance);
        }
    }
}
namespace Destroy.Network
{
    using Destroy.Example;
    using System.Collections.Generic;

    /// <summary>
    /// 网络管理器
    /// </summary>
    public class NetworkManager : Singleton<NetworkManager>
    {
        public void Register(Dictionary<int, Instantiate> prefabs) => NetworkSystem.Register(prefabs);

        public void Instantiate_RPC(int typeId, Vector2Int position)
        {
            NetworkSystem.Client?.Instantiate_RPC(typeId, position);
        }

        public void Destroy(GameObject instance)
        {
            NetworkSystem.Client?.Destroy(instance);
        }
    }
}
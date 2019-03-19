namespace Destroy.Network
{
    using System.Collections.Generic;

    internal class GameClient : NetworkClient
    {
        private bool join;
        private int id;
        private int frame;
        private Dictionary<int, Instantiate> prefabs;
        private Dictionary<int, Dictionary<int, GameObject>> instances; //管理该局游戏场景中的对象

        private Dictionary<int, GameObject> selfIntances;               //物品Id, 游戏物体
        private Dictionary<int, GameObject> otherInstances;

        public GameClient(string serverIp, int serverPort, Dictionary<int, Instantiate> prefabs) : base(serverIp, serverPort)
        {
            join = false;
            id = -1;
            frame = -1;
            this.prefabs = prefabs;
            instances = new Dictionary<int, Dictionary<int, GameObject>>();
            foreach (var prefab in prefabs)
                instances.Add(prefab.Key, new Dictionary<int, GameObject>());
            //负责管理移动
            selfIntances = new Dictionary<int, GameObject>();
            otherInstances = new Dictionary<int, GameObject>();

            Register((ushort)NetworkRole.Server, (ushort)GameCmd.Join, JoinCallback);
            Register((ushort)NetworkRole.Server, (ushort)GameCmd.Move, MoveCallback);
            Register((ushort)NetworkRole.Server, (ushort)GameCmd.Destroy, DestroyCallback);
            Register((ushort)NetworkRole.Server, (ushort)GameCmd.Instantiate, OnInstantiated);
        }

        internal void Instantiate_RPC(int typeId, Vector2Int position)
        {
            if (!prefabs.ContainsKey(typeId))
                return;
            C2S_Instantiate cmd = new C2S_Instantiate();
            cmd.Frame = frame;
            cmd.TypeId = typeId;
            //修改坐标
            cmd.X = position.X;
            cmd.Y = position.Y;
            //发送指令
            Send((ushort)NetworkRole.Client, (ushort)GameCmd.Instantiate, cmd);
        }

        internal void Destroy(GameObject instance)
        {
            if (!instance) //如果没有游戏物体
                return;
            NetworkIdentity identity = instance.GetComponent<NetworkIdentity>();
            if (!identity.Enable) //如果没有该组件或者不活跃
                return;
            //只允许删除自己的游戏物体
            if (!selfIntances.ContainsKey(identity.Id))
                return;

            //从列表中删除
            instances[identity.TypeId].Remove(identity.Id);
            //从场景中删除
            GameObject.Destroy(instance);
            //从自己物体中删除
            selfIntances.Remove(identity.Id);

            C2S_Destroy cmd = new C2S_Destroy();
            cmd.Frame = frame;
            cmd.TypeId = identity.TypeId;
            cmd.Id = identity.Id;
            //发送指令
            Send((ushort)NetworkRole.Client, (ushort)GameCmd.Destroy, cmd);
        }

        internal void Move()
        {
            if (!join) //等待加入游戏成功
                return;

            C2S_Move move = new C2S_Move();
            move.Frame = frame;
            move.Entities = new List<Entity>();
            foreach (var instance in selfIntances)
            {
                Vector2Int pos = instance.Value.Position;
                move.Entities.Add(new Entity(instance.Key, pos.X, pos.Y));
            }
            Send((ushort)NetworkRole.Client, (ushort)GameCmd.Move, move);
        }

        private void JoinCallback(byte[] data)
        {
            S2C_Join join = NetworkSerializer.Deserialize<S2C_Join>(data);
            this.join = true;
            frame = join.Frame;
            id = join.YourId;
            if (join.Instances == null) //表示当前没有游戏物体
                return;
            //生成所有游戏物体
            foreach (var instance in join.Instances)
                CreatInstance(instance);
        }

        private void MoveCallback(byte[] data)
        {
            S2C_Move move = NetworkSerializer.Deserialize<S2C_Move>(data);
            frame = move.Frame;
            if (move.Entities == null) //表示当前没有游戏物体
                return;
            //实现其他物体同步移动
            foreach (var entity in move.Entities)
            {
                //只同步他人
                Vector2Int pos = new Vector2Int(entity.X, entity.Y);
                if (otherInstances.ContainsKey(entity.Id))
                    otherInstances[entity.Id].Position = pos;
            }
        }

        private void DestroyCallback(byte[] data)
        {
            S2C_Destroy cmd = NetworkSerializer.Deserialize<S2C_Destroy>(data);
            //获取场景实例
            GameObject instance = instances[cmd.TypeId][cmd.Id];
            //从列表中删除
            instances[cmd.TypeId].Remove(cmd.Id);
            //从场景中删除
            global::Destroy.GameObject.Destroy(instance);
            //从他人物体中移除
            otherInstances.Remove(cmd.Id);
        }

        private void OnInstantiated(byte[] data)
        {
            S2C_Instantiate cmd = NetworkSerializer.Deserialize<S2C_Instantiate>(data);
            CreatInstance(cmd.Instance);
        }

        private void CreatInstance(Instance instance)
        {
            //创建场景实例
            GameObject gameObject = prefabs[instance.TypeId]();
            //加进对象列表
            instances[instance.TypeId].Add(instance.Id, gameObject);
            //加进本地物体
            if (instance.IsLocal)
                selfIntances.Add(instance.Id, gameObject);
            //加进他人物体
            else
                otherInstances.Add(instance.Id, gameObject);

            //修改坐标
            gameObject.Position = new Vector2Int(instance.X, instance.Y);
            //添加Id组件
            NetworkIdentity identity = gameObject.AddComponent<NetworkIdentity>();
            identity.TypeId = instance.TypeId;
            identity.Id = instance.Id;
            //组件赋值
            List<NetworkScript> netScripts = gameObject.GetComponents<NetworkScript>();
            foreach (NetworkScript netScript in netScripts)
                netScript.IsLocal = instance.IsLocal;
        }
    }
}
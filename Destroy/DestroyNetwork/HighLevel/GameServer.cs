namespace Destroy.Network
{
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;

    internal class GameServer : NetworkServer
    {
        private int clientId;
        private int frame;
        private Dictionary<Socket, int> clients;                    //客户端, Id
        private Dictionary<int, Instantiate> prefabs;               //物体Id, Prefab
        private int instanceId;
        private Dictionary<int, Dictionary<int, Entity>> instances; //管理该局中的对象的位置

        public GameServer(int port, Dictionary<int, Instantiate> prefabs) : base(port)
        {
            clientId = 0;
            frame = 0;
            clients = new Dictionary<Socket, int>();
            this.prefabs = prefabs;
            instanceId = 0;
            instances = new Dictionary<int, Dictionary<int, Entity>>();
            foreach (var prefab in this.prefabs)
                instances.Add(prefab.Key, new Dictionary<int, Entity>());

            Console.WriteLine("服务器开启");

            base.OnConnected += OnConnected;
            base.OnDisconnected += OnDisconnected;
            Register((ushort)NetworkRole.Client, (ushort)GameCmd.Move, OnMove);
            Register((ushort)NetworkRole.Client, (ushort)GameCmd.Instantiate, OnInstantiate);
            Register((ushort)NetworkRole.Client, (ushort)GameCmd.Destroy, OnDestroy);
        }

        public void Broadcast()
        {
            S2C_Move move = new S2C_Move();
            move.Frame = frame;
            move.Entities = new List<Entity>();
            frame++;
            foreach (var dict in instances.Values)
            {
                foreach (var entity in dict.Values)
                {
                    move.Entities.Add(entity);
                }
            }

            //广播所有人
            foreach (var client in clients.Keys)
                Send(client, (ushort)NetworkRole.Server, (ushort)GameCmd.Move, move);
        }

        private new void OnConnected(Socket socket)
        {
            clients.Add(socket, clientId);
            Console.WriteLine("客户端连接");

            //发送断线重连包
            S2C_Join join = new S2C_Join();
            join.Frame = frame;
            join.YourId = clientId;
            join.Instances = new List<Instance>();
            foreach (var instance in instances)
            {
                foreach (var item in instance.Value) //该类型下的物体
                {
                    Instance instantiate = new Instance();
                    instantiate.IsLocal = clients[socket] == item.Key;
                    instantiate.TypeId = instance.Key;
                    instantiate.Id = item.Key;      //物品Id
                    instantiate.X = item.Value.X;
                    instantiate.Y = item.Value.Y;
                    //添加
                    join.Instances.Add(instantiate);
                }
            }
            //自增
            clientId++;
            //发送给登陆者
            Send(socket, (ushort)NetworkRole.Server, (ushort)GameCmd.Join, join);
        }

        private new void OnDisconnected(string msg, Socket socket)
        {
            clients.Remove(socket);
            Console.WriteLine($"客户端断开连接:{msg}");
        }

        private void OnMove(Socket socket, byte[] data)
        {
            C2S_Move move = NetworkSerializer.Deserialize<C2S_Move>(data);

            if (move.Entities == null) //表示该玩家没有创建移动的游戏物体
                return;
            //玩家的实体
            foreach (var entity in move.Entities)
            {
                //获得场景中所有实例
                foreach (var dict in instances.Values)
                {
                    dict[entity.Id] = entity; //赋值
                }
            }
        }

        private void OnInstantiate(Socket socket, byte[] data)
        {
            C2S_Instantiate clientCmd = NetworkSerializer.Deserialize<C2S_Instantiate>(data);

            //新增对象数据
            instances[clientCmd.TypeId].Add(instanceId, new Entity(clients[socket], clientCmd.X, clientCmd.Y));

            S2C_Instantiate cmd = new S2C_Instantiate();
            cmd.Frame = clientCmd.Frame;
            cmd.Instance.TypeId = clientCmd.TypeId;
            cmd.Instance.Id = instanceId;
            cmd.Instance.X = clientCmd.X;
            cmd.Instance.Y = clientCmd.Y;
            //自增
            instanceId++;
            //广播所有人
            foreach (var client in clients.Keys)
            {
                if (client == socket) //调用者拥有IsLocal属性
                    cmd.Instance.IsLocal = true;
                else
                    cmd.Instance.IsLocal = false;
                Send(client, (ushort)NetworkRole.Server, (ushort)GameCmd.Instantiate, cmd);
            }
        }

        private void OnDestroy(Socket socket, byte[] data)
        {
            C2S_Destroy clientCmd = NetworkSerializer.Deserialize<C2S_Destroy>(data);

            //移除对象数据
            instances[clientCmd.TypeId].Remove(clientCmd.Id);

            S2C_Destroy cmd = new S2C_Destroy();
            cmd.Frame = clientCmd.Frame;
            cmd.TypeId = clientCmd.TypeId;
            cmd.Id = clientCmd.Id;
            //广播除了发送者之外的人
            foreach (var client in clients.Keys)
            {
                if (client == socket)
                    continue;
                Send(client, (ushort)NetworkRole.Server, (ushort)GameCmd.Destroy, cmd);
            }
        }
    }
}
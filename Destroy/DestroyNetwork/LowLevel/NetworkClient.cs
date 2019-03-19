namespace Destroy.Network
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;

    internal class NetworkClient
    {
        public delegate void CallbackEvent(byte[] data);

        private readonly string serverIp;
        private readonly int serverPort;
        private Dictionary<int, CallbackEvent> events;
        private Socket client;
        private Queue<byte[]> messages;

        public NetworkClient(string serverIp, int serverPort)
        {
            this.serverIp = serverIp;
            this.serverPort = serverPort;
            events = new Dictionary<int, CallbackEvent>();
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            messages = new Queue<byte[]>();
        }

        /// <summary>
        /// 连接完成
        /// </summary>
        public event Action<Socket> OnConnected;

        public void Register(ushort cmd1, ushort cmd2, CallbackEvent _event)
        {
            int key = NetworkSerializer.EnumToKey(cmd1, cmd2);
            if (events.ContainsKey(key))
                return;
            events.Add(key, _event);
        }

        public void Send(ushort cmd1, ushort cmd2, byte[] data)
        {
            byte[] packData = NetworkSerializer.PackSimpleTCPMessage(cmd1, cmd2, data);
            messages.Enqueue(packData);
        }

        public void Send<T>(ushort cmd1, ushort cmd2, T message)
        {
            byte[] data = NetworkSerializer.PackTCPMessage(cmd1, cmd2, message);
            messages.Enqueue(data);
        }

        public void Start()
        {
            client.Connect(new IPEndPoint(IPAddress.Parse(serverIp), serverPort));
            OnConnected?.Invoke(client);
        }

        public void Update()
        {
            //接受消息
            while (client.Available > 0)
            {
                NetworkSerializer.UnpackTCPMessage(client, out ushort cmd1, out ushort cmd2, out byte[] data);
                int key = NetworkSerializer.EnumToKey(cmd1, cmd2);

                if (events.ContainsKey(key))
                    events[key](data);
            }

            //发送消息
            while (messages.Count > 0)
            {
                byte[] data = messages.Dequeue();
                client.Send(data);
            }
        }
    }
}
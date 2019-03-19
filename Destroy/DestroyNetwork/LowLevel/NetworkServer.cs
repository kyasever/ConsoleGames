namespace Destroy.Network
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;

    internal class NetworkServer
    {
        private sealed class Message
        {
            public Socket Socket;
            private byte[] data;

            public Message(Socket socket, byte[] data)
            {
                Socket = socket;
                this.data = data;
            }

            public void Send() => Socket.Send(data);
        }

        private sealed class Client
        {
            public bool Connected;
            public Socket Socket;

            public Client(bool connected, Socket socket)
            {
                Connected = connected;
                Socket = socket;
            }
        }

        public delegate void CallbackEvent(Socket socket, byte[] data);

        public int ClientCount => clients.Count;

        private Dictionary<int, CallbackEvent> events;
        private Queue<Message> messages;
        private List<Client> clients;
        private Socket server;
        private bool accept;
        private IAsyncResult acceptAsync;

        public NetworkServer(int port)
        {
            events = new Dictionary<int, CallbackEvent>();
            messages = new Queue<Message>();
            clients = new List<Client>();
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(new IPEndPoint(NetworkUtils.LocalIPv4, port));
            accept = true;
            acceptAsync = null;
        }

        /// <summary>
        /// 收到客户端连接
        /// </summary>
        public event Action<Socket> OnConnected;

        /// <summary>
        /// 客户端断开连接
        /// </summary>
        public event Action<string, Socket> OnDisconnected;

        public void Register(ushort cmd1, ushort cmd2, CallbackEvent _event)
        {
            int key = NetworkSerializer.EnumToKey(cmd1, cmd2);
            if (events.ContainsKey(key))
                return;
            events.Add(key, _event);
        }

        public void Send(Socket client, ushort cmd1, ushort cmd2, byte[] data)
        {
            byte[] packData = NetworkSerializer.PackSimpleTCPMessage(cmd1, cmd2, data);
            messages.Enqueue(new Message(client, packData));
        }

        public void Send<T>(Socket client, ushort cmd1, ushort cmd2, T message)
        {
            byte[] data = NetworkSerializer.PackTCPMessage(cmd1, cmd2, message);
            messages.Enqueue(new Message(client, data));
        }

        public void Start() => server.Listen(10);

        public void Update()
        {
            //异步接收客户端
            if (accept)
            {
                try
                {
                    acceptAsync = server.BeginAccept(null, null);
                    accept = false;
                }
                catch (Exception) { }
            }
            if (acceptAsync.IsCompleted)
            {
                try
                {
                    Socket socket = server.EndAccept(acceptAsync);
                    clients.Add(new Client(true, socket));
                    OnConnected?.Invoke(socket);
                }
                catch (Exception) { }
                finally { accept = true; }
            }

            //异步接收消息
            foreach (Client client in clients)
            {
                if (!client.Connected) //不接受断开连接的消息
                    continue;
                Socket socket = client.Socket;

                while (socket.Available > 0)
                {
                    try
                    {
                        NetworkSerializer.UnpackTCPMessage(socket, out ushort cmd1, out ushort cmd2, out byte[] data);
                        int key = NetworkSerializer.EnumToKey(cmd1, cmd2);

                        if (events.ContainsKey(key))
                            events[key](socket, data);
                    }
                    catch (Exception ex)
                    {
                        socket.Close();
                        client.Connected = false;
                        OnDisconnected?.Invoke(ex.Message, socket);
                    }
                }
            }

            //异步发送消息
            while (messages.Count > 0)
            {
                Message message = messages.Dequeue();
                Socket socket = message.Socket;

                bool pass = false;
                foreach (Client client in clients)
                {
                    if (client.Socket == socket && client.Connected) //存在该客户端并且该客户端激活
                    {
                        pass = true;
                        break;
                    }
                }
                if (!pass)
                    continue;

                try
                {
                    message.Send();
                }
                catch (Exception ex)
                {
                    socket.Close();
                    //禁用该Socket
                    foreach (Client client in clients)
                        if (client.Socket == socket)
                            client.Connected = false;

                    OnDisconnected?.Invoke(ex.Message, socket);
                    break;
                }
            }
        }
    }
}
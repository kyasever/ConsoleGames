using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WizardAdvanture
{
    enum GameAction
    {
        Game = 1,
    }

    enum GameCmd
    {
        HeartBeat = 1,
        LoginGame = 2,
        Action = 3,
    }

    class ResponseMessage
    {
        protected void Init()
        {
            if (rawMessage.Length >= 2)
            {
                act = (ushort)((rawMessage[1] << 8) + rawMessage[0]);
            }
            if (rawMessage.Length >= 4)
            {
                cmd = (ushort)((rawMessage[3] << 8) + rawMessage[2]);
            }
            message = "";
            if (rawMessage.Length > 4)
            {
                for (int i = 4; i < rawMessage.Length; i++)
                {
                    message += (char)rawMessage[i];
                }
            }
        }

        public ResponseMessage(byte[] message)
        {
            this.rawMessage = message;
            Init();
        }
        public string message;
        public byte[] rawMessage;

        public ushort act;
        public ushort cmd;
    }

    delegate void ProcessFunc(ResponseMessage message);

    class MyTCPClient
    {
        static MyTCPClient instance = null;
        public static MyTCPClient Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MyTCPClient();
                }
                return instance;
            }
        }

        Socket socketClient;
        IPAddress ip;
        IPEndPoint endPoint;

        public void Init(string _ip, int port)
        {
            socketClient = new Socket(SocketType.Stream, ProtocolType.Tcp);
            ip = IPAddress.Parse(_ip);
            endPoint = new IPEndPoint(ip, port);
        }

        public void Connect()
        {
            socketClient.Connect(endPoint);

            //不停的接收服务器端发送的消息
            Thread thread = new Thread(Receive);
            thread.IsBackground = true;
            thread.Start(null);
        }

        public Queue<ResponseMessage> messageQueue = new Queue<ResponseMessage>();

        public BinaryWriter GetSendStream(GameAction act, GameCmd cmd)
        {
            return GetSendStream((ushort)act, (ushort)cmd);
        }

        public BinaryWriter GetSendStream(ushort act, ushort cmd)
        {
            BinaryWriter writer = new BinaryWriter(new MemoryStream());
            writer.Write((ushort)0);
            writer.Write(act);
            writer.Write(cmd);
            return writer;
        }

        public void Send(BinaryWriter writer)
        {
            MemoryStream stream = writer.BaseStream as MemoryStream;
            byte[] data = stream.ToArray();
            int len = data.Length - 2;
            data[0] = (byte)(len >> 8);
            data[1] = (byte)len;
            socketClient.Send(data);
        }

        public Dictionary<int, ProcessFunc> dictProcessFunc = new Dictionary<int, ProcessFunc>();
        public virtual void AddProcessFunc(GameAction act, GameCmd cmd, ProcessFunc func)
        {
            int key = (((int)act) << 16) + (int)cmd;
            if (dictProcessFunc.ContainsKey(key))
            {
                throw new Exception("重复定义消息处理函数");
            }
            dictProcessFunc[key] = func;
        }

        // 接收消息
        void Receive(object o)
        {
            // 获取发送过来的消息buffer
            byte[] buffer = new byte[0xFFFF + 1];
            try
            {
                // 一个客户端一旦连接，就一直处于收包循环
                while (true)
                {
                    int effective = _ReceivePacket(socketClient, buffer);
                   
                    //Console.WriteLine($"客户端 {socketClient.Handle.ToInt32()} 收包 {effective}字节");
                    byte[] message = new byte[effective];
                    Array.Copy(buffer, message, effective);
                    messageQueue.Enqueue(new ResponseMessage(message));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"捕获到异常：{e.Message}，退出 ");
            }
            finally
            {
                socketClient.Close();
                socketClient.Dispose();
            }
            // 如果运行到这里，就代表网络断开了。看需求重连或者其他操作
            return;
        }

        // 收包专用，考虑了粘包问题
        static int _ReceivePacket(Socket socket, byte[] buffer)
        {
            int n = socket.Receive(buffer, 0, 2, SocketFlags.None);
            int packetLength = (buffer[0] << 8) + buffer[1];

            int received = 0;
            while (received < packetLength)
            {
                received += socket.Receive(buffer, received, packetLength - received, SocketFlags.None);
            }
            return received;
        }
    }
}

namespace Destroy.Network
{
    using Destroy;
    using System;
    using System.Collections.Generic;

    public class NetworkSystem : DestroySystem
    {
        internal static GameClient Client;

        private static Dictionary<int, Instantiate> prefabs;

        private static GameServer server;

        private static int gamePort;

        private static float clientInterval;

        private static float serverInterval;

        private static bool choose;

        private static float serverTimer;

        private static float clientTimer;

        internal static void Register(Dictionary<int, Instantiate> prefabs)
        {
            NetworkSystem.prefabs = prefabs;
        }

        public override void Start()
        {
            gamePort = Config.Net.GamePort;
            clientInterval = (float)1 / Config.Net.ClientSyncRate;
            serverInterval = (float)1 / Config.Net.ServerSyncRate;
        }

        public override void Update()
        {
            if (prefabs == null)
                throw new Exception($"Please Invoke {nameof(Register)} first.");

            if (!choose)
            {
                Console.WriteLine("1.Client 2.Server 3.Host");
                int mode = int.Parse(Console.ReadLine());
                switch (mode)
                {
                    case 1:
                        {
                            Console.WriteLine("Enter Server Ip(Enter # connect local server):");
                            string ip = Console.ReadLine();
                            if (ip == "#")
                                ip = NetworkUtils.LocalIPv4Str;
                            Client = new GameClient(ip, gamePort, prefabs);
                            Client.Start();
                        }
                        break;
                    case 2:
                        {
                            server = new GameServer(gamePort, prefabs);
                            server.Start();
                        }
                        break;
                    case 3:
                        {
                            server = new GameServer(gamePort, prefabs);
                            server.Start();
                            Client = new GameClient(NetworkUtils.LocalIPv4Str, gamePort, prefabs);
                            Client.Start();
                        }
                        break;
                    default:
                        throw new Exception();
                }
                choose = true;
                Console.Clear();
            }

            if (server != null)
            {
                serverTimer += Time.DeltaTime;
                server.Update();
                if (serverTimer >= serverInterval)
                {
                    serverTimer = 0;
                    server.Broadcast();
                }
            }

            if (Client != null)
            {
                clientTimer += Time.DeltaTime;
                Client.Update();
                if (clientTimer >= clientInterval)
                {
                    clientTimer = 0;
                    Client.Move();
                }
            }
        }
    }
}
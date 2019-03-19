namespace Destroy.Network
{
    using System.Net;
    using System.Net.Sockets;

    internal static class NetworkUtils
    {
        public static IPAddress LocalIPv4
        {
            get
            {
                string hostName = Dns.GetHostName();
                IPHostEntry iPEntry = Dns.GetHostEntry(hostName);
                for (int i = 0; i < iPEntry.AddressList.Length; i++)
                {
                    if (iPEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                        return iPEntry.AddressList[i];
                }
                return null;
            }
        }

        public static string LocalIPv4Str
        {
            get
            {
                IPAddress iPAddress = LocalIPv4;
                return iPAddress?.ToString();
            }
        }
    }
}
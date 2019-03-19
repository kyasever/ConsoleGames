namespace Destroy.Network
{
    public class NetworkIdentity : Component
    {
        public int TypeId;
        public int Id;

        public NetworkIdentity()
        {
            TypeId = 0;
            Id = 0;
        }
    }
}
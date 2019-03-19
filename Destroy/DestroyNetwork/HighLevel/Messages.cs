namespace Destroy.Network
{
    using ProtoBuf;
    using System.Collections.Generic;

    internal enum NetworkRole
    {
        Client,
        Server,
    }

    internal enum GameCmd
    {
        Join,
        Move,
        Instantiate,
        Destroy,
    }

    [ProtoContract]
    internal struct C2S_Instantiate
    {
        [ProtoMember(1)]
        public int Frame;
        [ProtoMember(2)]
        public int TypeId;
        [ProtoMember(3)]
        public int X;
        [ProtoMember(4)]
        public int Y;
    }

    [ProtoContract]
    internal struct S2C_Instantiate
    {
        [ProtoMember(1)]
        public int Frame;
        [ProtoMember(2)]
        public Instance Instance;
    }

    [ProtoContract]
    internal struct C2S_Destroy
    {
        [ProtoMember(1)]
        public int Frame;
        [ProtoMember(2)]
        public int TypeId;
        [ProtoMember(3)]
        public int Id;
    }

    [ProtoContract]
    internal struct S2C_Destroy
    {
        [ProtoMember(1)]
        public int Frame;
        [ProtoMember(2)]
        public int TypeId;
        [ProtoMember(3)]
        public int Id;
    }

    [ProtoContract]
    internal struct S2C_Join
    {
        [ProtoMember(1)]
        public int Frame;
        [ProtoMember(2)]
        public int YourId;
        [ProtoMember(3)]
        public List<Instance> Instances;
    }

    [ProtoContract]
    internal struct Instance
    {
        [ProtoMember(1)]
        public int TypeId;
        [ProtoMember(2)]
        public int Id;
        [ProtoMember(3)]
        public bool IsLocal;
        [ProtoMember(4)]
        public int X;
        [ProtoMember(5)]
        public int Y;
    }

    [ProtoContract]
    internal struct Entity
    {
        [ProtoMember(1)]
        public int Id;
        [ProtoMember(2)]
        public int X;
        [ProtoMember(3)]
        public int Y;

        public Entity(int id, int x, int y)
        {
            Id = id;
            X = x;
            Y = y;
        }
    }

    [ProtoContract]
    internal struct C2S_Move
    {
        [ProtoMember(1)]
        public int Frame;
        [ProtoMember(2)]
        public List<Entity> Entities; //Self Instances's Postions
    }

    [ProtoContract]
    internal struct S2C_Move
    {
        [ProtoMember(1)]
        public int Frame;
        [ProtoMember(2)]
        public List<Entity> Entities;
    }
}
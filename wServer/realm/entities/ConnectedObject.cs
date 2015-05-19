#region

using System;
using System.Collections.Generic;

#endregion

namespace wServer.realm.entities
{
    public enum ConnectionType
    {
        Dot = 0,
        ushortLine = 1,
        L = 2,
        Line = 3,
        T = 4,
        Cross = 5
    }

    public class ConnectionInfo
    {
        public static readonly Dictionary<uint, ConnectionInfo> Infos = new Dictionary<uint, ConnectionInfo>();

        public static readonly Dictionary<Tuple<ConnectionType, int>, ConnectionInfo> Infos2 =
            new Dictionary<Tuple<ConnectionType, int>, ConnectionInfo>();

        static ConnectionInfo()
        {
            Build(0x02020202, ConnectionType.Dot); //1111
            Build(0x01020202, ConnectionType.ushortLine); //0111
            Build(0x01010202, ConnectionType.L); //0011
            Build(0x01020102, ConnectionType.Line); //0101
            Build(0x01010201, ConnectionType.T); //0010
            Build(0x01010101, ConnectionType.Cross); //0000
        }

        private ConnectionInfo(uint bits, ConnectionType type, int rotation)
        {
            Bits = bits;
            Type = type;
            Rotation = rotation;
        }


        public ConnectionType Type { get; private set; }
        public int Rotation { get; private set; }
        public uint Bits { get; private set; }

        private static void Build(uint bits, ConnectionType type)
        {
            for (int i = 0; i < 4; i++)
                if (!Infos.ContainsKey(bits))
                {
                    Infos[bits] = Infos2[Tuple.Create(type, i*90)] = new ConnectionInfo(bits, type, i*90);
                    bits = (bits >> 8) | (bits << 24);
                }
        }
    }

    public class ConnectionComputer
    {
        public static ConnectionInfo Compute(Func<int, int, bool> offset)
        {
            bool[,] z = new bool[3, 3];
            for (int y = -1; y <= 1; y++)
                for (int x = -1; x <= 1; x++)
                    z[x + 1, y + 1] = offset(x, y);


            if (z[1, 0] && z[1, 2] && z[0, 1] && z[2, 1])
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.Cross, 0)];

            if (z[0, 1] && z[1, 1] && z[2, 1] && z[1, 0])
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.T, 0)];
            if (z[1, 0] && z[1, 1] && z[1, 2] && z[2, 1])
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.T, 90)];
            if (z[0, 1] && z[1, 1] && z[2, 1] && z[1, 2])
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.T, 180)];
            if (z[1, 0] && z[1, 1] && z[1, 2] && z[0, 1])
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.T, 270)];

            if (z[1, 0] && z[1, 1] && z[1, 2]) 
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.Line, 0)];
            if (z[0, 1] && z[1, 1] && z[2, 1])
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.Line, 90)];

            if (z[1, 0] && z[2, 1])
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.L, 0)];
            if (z[2, 1] && z[1, 2])
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.L, 90)];
            if (z[1, 2] && z[0, 1])
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.L, 180)];
            if (z[0, 1] && z[1, 0])
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.L, 270)];

            if (z[1, 0])
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.ushortLine, 0)];
            if (z[2, 1])
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.ushortLine, 90)];
            if (z[1, 2])
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.ushortLine, 180)];
            if (z[0, 1])
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.ushortLine, 270)];

            return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.Dot, 0)];
        }

        public static string GetConnString(Func<int, int, bool> offset)
        {
            return "conn:" + Compute(offset).Bits;
        }
    }

    public class ConnectedObject : StaticObject
    {
        public ConnectedObject(RealmManager manager, ushort objType)
            : base(manager, objType, null, true, false, true)
        {
        }

        public ConnectionInfo Connection { get; set; }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            stats[StatsType.ObjectConnection] = (int)ConnectionComputer.Compute((_x, _y) => false).Bits;
            base.ExportStats(stats);
        }


        public override bool HitByProjectile(Projectile projectile, RealmTime time)
        {
            return true;
        }
    }
}
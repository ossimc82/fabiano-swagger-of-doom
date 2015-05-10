#region

using System;
using System.Collections.Generic;
using wServer.networking;
using wServer.realm;

#endregion

namespace wServer
{
    public struct BitmapData
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public byte[] Bytes { get; set; }

        public static BitmapData Read(Client psr, NReader rdr)
        {
            BitmapData ret = new BitmapData();
            ret.Width = rdr.ReadInt32();
            ret.Height = rdr.ReadInt32();
            ret.Bytes = new byte[ret.Width*ret.Height*4];
            ret.Bytes = rdr.ReadBytes(ret.Bytes.Length);
            return ret;
        }

        public void Write(Client psr, NWriter wtr)
        {
            wtr.Write(Width);
            wtr.Write(Height);
            wtr.Write(Bytes);
        }
    }

    public struct IntPointComparer : IEqualityComparer<IntPoint>
    {
        public bool Equals(IntPoint x, IntPoint y)
        {
            return x.X == y.X && x.Y == y.Y;
        }

        public int GetHashCode(IntPoint obj)
        {
            return obj.X*23 << 16 + obj.Y*17;
        }
    }

    public struct IntPoint
    {
        public int X;
        public int Y;

        public IntPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public struct TradeItem
    {
        public bool Included;
        public int Item;
        public int SlotType;
        public bool Tradeable;

        public static TradeItem Read(Client psr, NReader rdr)
        {
            TradeItem ret = new TradeItem();
            ret.Item = rdr.ReadInt32();
            ret.SlotType = rdr.ReadInt32();
            ret.Tradeable = rdr.ReadBoolean();
            ret.Included = rdr.ReadBoolean();
            return ret;
        }

        public void Write(Client psr, NWriter wtr)
        {
            wtr.Write(Item);
            wtr.Write(SlotType);
            wtr.Write(Tradeable);
            wtr.Write(Included);
        }
    }

    public enum EffectType
    {
        Potion = 1,
        Teleport = 2,
        Stream = 3,
        Throw = 4,
        AreaBlast = 5, //radius=pos1.x
        Dead = 6,
        Trail = 7,
        Diffuse = 8, //radius=dist(pos1,pos2)
        Flow = 9,
        Trap = 10, //radius=pos1.x
        Lightning = 11, //particleSize=pos2.x
        Concentrate = 12, //radius=dist(pos1,pos2)
        BlastWave = 13, //origin=pos1, radius = pos2.x
        Earthquake = 14,
        Flashing = 15, //period=pos1.x, numCycles=pos1.y
        BeachBall = 16,
        ElectricBolts = 17, //If a pet paralyzes a monster
        ElectricFlashing = 18, //If a monster got paralyzed from a electric pet
        SavageEffect = 19 //If a pet is standing still (this white particles)
    }

    public struct ARGB
    {
        public byte A;
        public byte B;
        public byte G;
        public byte R;

        public ARGB(uint argb)
        {
            A = (byte) ((argb & 0xff000000) >> 24);
            R = (byte) ((argb & 0x00ff0000) >> 16);
            G = (byte) ((argb & 0x0000ff00) >> 8);
            B = (byte) ((argb & 0x000000ff) >> 0);
        }

        public static ARGB Read(Client psr, NReader rdr)
        {
            ARGB ret = new ARGB();
            ret.A = rdr.ReadByte();
            ret.R = rdr.ReadByte();
            ret.G = rdr.ReadByte();
            ret.B = rdr.ReadByte();
            return ret;
        }

        public void Write(Client psr, NWriter wtr)
        {
            wtr.Write(A);
            wtr.Write(R);
            wtr.Write(G);
            wtr.Write(B);
        }
    }

    public struct ObjectSlot
    {
        public int ObjectId;
        public ushort ObjectType;
        public byte SlotId;

        public static ObjectSlot Read(Client psr, NReader rdr)
        {
            ObjectSlot ret = new ObjectSlot();
            ret.ObjectId = rdr.ReadInt32();
            ret.SlotId = rdr.ReadByte();
            ret.ObjectType = (ushort)rdr.ReadInt16();
            return ret;
        }

        public void Write(Client psr, NWriter wtr)
        {
            wtr.Write(ObjectId);
            wtr.Write(SlotId);
            wtr.Write(ObjectType);
        }

        public override string ToString()
        {
            return string.Format("{{ObjectId: {0}, SlotId: {1}, ObjectType: {2}}}", ObjectId, SlotId, ObjectType);
        }
    }

    public struct TimedPosition
    {
        public Position Position;
        public int Time;

        public static TimedPosition Read(Client psr, NReader rdr)
        {
            TimedPosition ret = new TimedPosition();
            ret.Time = rdr.ReadInt32();
            ret.Position = Position.Read(psr, rdr);
            return ret;
        }

        public void Write(Client psr, NWriter wtr)
        {
            wtr.Write(Time);
            Position.Write(psr, wtr);
        }

        public override string ToString()
        {
            return string.Format("{{Time: {0}, Position: {1}}}", Time, Position);
        }
    }

    public struct Position
    {
        public float X;
        public float Y;

        public Position(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Position Read(Client psr, NReader rdr)
        {
            Position ret = new Position();
            ret.X = rdr.ReadSingle();
            ret.Y = rdr.ReadSingle();
            return ret;
        }

        public void Write(Client psr, NWriter wtr)
        {
            wtr.Write(X);
            wtr.Write(Y);
        }

        public override string ToString()
        {
            return string.Format("{{X: {0}, Y: {1}}}", X, Y);
        }
    }

    public struct ObjectDef
    {
        public ushort ObjectType;
        public ObjectStats Stats;

        public static ObjectDef Read(Client psr, NReader rdr)
        {
            ObjectDef ret = new ObjectDef();
            ret.ObjectType = (ushort)rdr.ReadInt16();
            ret.Stats = ObjectStats.Read(psr, rdr);
            return ret;
        }

        public void Write(Client psr, NWriter wtr)
        {
            wtr.Write(ObjectType);
            Stats.Write(psr, wtr);
        }
    }

    public struct ObjectStats
    {
        public int Id;
        public Position Position;
        public KeyValuePair<StatsType, object>[] Stats;

        public static ObjectStats Read(Client psr, NReader rdr)
        {
            ObjectStats ret = new ObjectStats();
            ret.Id = rdr.ReadInt32();
            ret.Position = Position.Read(psr, rdr);
            ret.Stats = new KeyValuePair<StatsType, object>[rdr.ReadInt16()];
            for (int i = 0; i < ret.Stats.Length; i++)
            {
                StatsType type = (StatsType) rdr.ReadByte();
                if (type == StatsType.Guild || type == StatsType.Name)
                    ret.Stats[i] = new KeyValuePair<StatsType, object>(type, rdr.ReadUTF());
                else
                    ret.Stats[i] = new KeyValuePair<StatsType, object>(type, rdr.ReadInt32());
            }
            return ret;
        }

        public void Write(Client psr, NWriter wtr)
        {
            try
            {
                wtr.Write(Id);
                Position.Write(psr, wtr);
                wtr.Write((ushort)Stats.Length);
                foreach (KeyValuePair<StatsType, object> i in Stats)
                {
                    wtr.Write((byte)i.Key);
                    if (i.Key.IsUTF() && i.Value != null) wtr.WriteUTF(i.Value.ToString());
                    else wtr.Write((int)i.Value);
                }
            }
            catch(Exception) { }
        }
    }
}
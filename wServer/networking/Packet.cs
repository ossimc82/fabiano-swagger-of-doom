#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using wServer.networking.svrPackets;

#endregion

namespace wServer.networking
{
    public abstract class Packet
    {
        public static Dictionary<PacketID, Packet> Packets = new Dictionary<PacketID, Packet>();

        static Packet()
        {
            foreach (Type i in typeof (Packet).Assembly.GetTypes())
                if (typeof (Packet).IsAssignableFrom(i) && !i.IsAbstract)
                {
                    Packet pkt = (Packet) Activator.CreateInstance(i);
                    if (!(pkt is ServerPacket))
                        if (!Packets.ContainsKey(pkt.ID))
                            Packets.Add(pkt.ID, pkt);
                }
        }

        public abstract PacketID ID { get; }
        public abstract Packet CreateInstance();

        public abstract void Crypt(Client client, byte[] dat, int offset, int len);

        public void Read(Client client, byte[] body, int offset, int len)
        {
            Crypt(client, body, offset, len);
            Read(client, new NReader(new MemoryStream(body)));
        }

        public int Write(Client client, byte[] buff, int offset)
        {
            MemoryStream s = new MemoryStream(buff, offset + 5, buff.Length - offset - 5);
            Write(client, new NWriter(s));

            int len = (int) s.Position;
            Crypt(client, buff, offset + 5, len);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(len + 5)), 0, buff, offset, 4);
            buff[offset + 4] = (byte) ID;
            return len + 5;
        }

        protected abstract void Read(Client client, NReader rdr);
        protected abstract void Write(Client client, NWriter wtr);

        public override string ToString()
        {
            StringBuilder ret = new StringBuilder("{");
            PropertyInfo[] arr = GetType().GetProperties();
            for (int i = 0; i < arr.Length; i++)
            {
                if (i != 0) ret.Append(", ");
                ret.AppendFormat("{0}: {1}", arr[i].Name, arr[i].GetValue(this, null));
            }
            ret.Append("}");
            return ret.ToString();
        }
    }

    public class NopPacket : Packet
    {
        public override PacketID ID
        {
            get { return (PacketID) 255; }
        }

        public override Packet CreateInstance()
        {
            return new NopPacket();
        }

        public override void Crypt(Client client, byte[] dat, int offset, int len)
        {
        }

        protected override void Read(Client client, NReader rdr)
        {
        }

        protected override void Write(Client client, NWriter wtr)
        {
        }
    }
}
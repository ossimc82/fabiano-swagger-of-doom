using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wServer.networking.svrPackets
{
    public class UnlockedSkinPacket : ServerPacket
    {
        public int SkinID { get; set; }

        public override PacketID ID
        {
            get { return PacketID.RESKIN2; }
        }

        public override Packet CreateInstance()
        {
            return new UnlockedSkinPacket();
        }

        protected override void Read(Client client, NReader rdr)
        {
            SkinID = rdr.ReadInt32();
        }

        protected override void Write(Client client, NWriter wtr)
        {
            wtr.Write(SkinID);
        }
    }
}

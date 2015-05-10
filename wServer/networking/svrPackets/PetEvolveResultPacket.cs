using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wServer.networking.svrPackets
{
    public class PetEvolveResultPacket : ServerPacket
    {
        public int PetId1 { get; set; }
        public int SkinId1 { get; set; }
        public int SkinId2 { get; set; }

        public override PacketID ID
        {
            get { return PacketID.EVOLVEPET; }
        }

        public override Packet CreateInstance()
        {
            return new PetEvolveResultPacket();
        }

        protected override void Read(Client client, NReader rdr)
        {
            PetId1 = rdr.ReadInt32();
            SkinId1 = rdr.ReadInt32();
            SkinId2 = rdr.ReadInt32();
        }

        protected override void Write(Client client, NWriter wtr)
        {
            wtr.Write(PetId1);
            wtr.Write(SkinId1);
            wtr.Write(SkinId2);
        }
    }
}

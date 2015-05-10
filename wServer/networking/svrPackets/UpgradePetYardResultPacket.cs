using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wServer.networking.svrPackets
{
    public class UpgradePetYardResultPacket : ServerPacket
    {
        public int Type { get; set; }

        public override PacketID ID
        {
            get { return PacketID.UPGRADEPETYARDRESULT; }
        }

        public override Packet CreateInstance()
        {
            return new UpgradePetYardResultPacket();
        }

        protected override void Read(Client client, NReader rdr)
        {
            Type = rdr.ReadInt32();
        }

        protected override void Write(Client client, NWriter wtr)
        {
            wtr.Write(Type);
        }
    }
}

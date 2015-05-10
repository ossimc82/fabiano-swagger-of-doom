using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wServer.networking.cliPackets
{
    public class TinkerQuestPacket : ClientPacket
    {
        public ObjectSlot Object { get; set; }

        public override PacketID ID
        {
            get { return PacketID.TINKERQUEST; }
        }

        public override Packet CreateInstance()
        {
            return new TinkerQuestPacket();
        }

        protected override void Read(Client client, NReader rdr)
        {
            Object = ObjectSlot.Read(client, rdr);
        }

        protected override void Write(Client client, NWriter wtr)
        {
            Object.Write(client, wtr);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wServer.networking.svrPackets
{
    public class QuestRedeemResponsePacket : ServerPacket
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public override PacketID ID
        {
            get { return PacketID.QUESTREDEEMRESPONSE; }
        }

        public override Packet CreateInstance()
        {
            return new QuestRedeemResponsePacket();
        }

        protected override void Read(Client client, NReader rdr)
        {
            Success = rdr.ReadBoolean();
            Message = rdr.ReadUTF();
        }

        protected override void Write(Client client, NWriter wtr)
        {
            wtr.Write(Success);
            wtr.WriteUTF(Message);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wServer.networking.svrPackets
{
    public class QuestFetchResponsePacket : ServerPacket
    {
        public int Tier { get; set; }
        public string Goal { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }


        public override PacketID ID
        {
        	get { return PacketID.QUESTFETCHRESPONSE; }
        }
        
        public override Packet CreateInstance()
        {
            return new QuestFetchResponsePacket();
        }
        
        protected override void Read(Client client, NReader rdr)
        {
            Tier = rdr.ReadInt32();
            Goal = rdr.ReadUTF();
            Description = rdr.ReadUTF();
            Image = rdr.ReadUTF();
        }
        
        protected override void Write(Client client, NWriter wtr)
        {
            wtr.Write(Tier);
            wtr.WriteUTF(Goal);
            wtr.WriteUTF(Description);
            wtr.WriteUTF(Image);
        }
    }
}

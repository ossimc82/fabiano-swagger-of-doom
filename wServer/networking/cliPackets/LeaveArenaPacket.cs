using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wServer.networking.cliPackets
{
    public class LeaveArenaPacket : ClientPacket
    {
        public int _li { get; set; }

        public override PacketID ID
        {
            get { return PacketID.LEAVEARENA; }
        }

        public override Packet CreateInstance()
        {
            return new LeaveArenaPacket();
        }

        protected override void Read(Client client, NReader rdr)
        {
            _li = rdr.ReadInt32();
        }

        protected override void Write(Client client, NWriter wtr)
        {
            wtr.Write(_li);
        }
    }
}

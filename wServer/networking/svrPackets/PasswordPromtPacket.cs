using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wServer.networking.svrPackets
{
    public class PasswordPromtPacket : ServerPacket
    {
        public const int SIGN_IN = 2;
        public const int SEND_EMAIL_AND_SIGN_IN = 3;
        public const int REGISTER = 4;

        public int CleanPasswordStatus { get; set; }

        public override PacketID ID
        {
            get { return PacketID.PASSWORDPROMPT; }
        }

        public override Packet CreateInstance()
        {
            return new PasswordPromtPacket();
        }

        protected override void Read(Client client, NReader rdr)
        {
            CleanPasswordStatus = rdr.ReadInt32();
        }

        protected override void Write(Client client, NWriter wtr)
        {
            wtr.Write(CleanPasswordStatus);
        }
    }
}

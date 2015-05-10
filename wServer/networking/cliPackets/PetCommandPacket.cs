using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wServer.networking.cliPackets
{
    public class PetCommandPacket : ClientPacket
    {
        public const int FOLLOW_PET = 1;
        public const int UNFOLLOW_PET = 2;
        public const int RELEASE_PET = 3;

        public int CommandId { get; set; }
        public uint PetId { get; set; }

        public override PacketID ID
        {
            get { return PacketID.PETCOMMAND; }
        }

        public override Packet CreateInstance()
        {
            return new PetCommandPacket();
        }

        protected override void Read(Client client, NReader rdr)
        {
            CommandId = (int)rdr.ReadByte();
            PetId = (uint)rdr.ReadInt32();
        }

        protected override void Write(Client client, NWriter wtr)
        {
            wtr.Write((byte)CommandId);
            wtr.Write((int)PetId);
        }
    }
}

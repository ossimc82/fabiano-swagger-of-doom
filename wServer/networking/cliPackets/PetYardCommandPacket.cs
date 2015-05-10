using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.realm;

namespace wServer.networking.cliPackets
{
    public class PetYardCommandPacket : ClientPacket
    {
        public const int UPGRADE_PET_YARD = 1;
        public const int FEED_PET = 2;
        public const int FUSE_PET = 3;

        public byte CommandId { get; set; }
        public int PetId1 { get; set; }
        public int PetId2 { get; set; }
        public int ObjectId { get; set; }
        public ObjectSlot ObjectSlot { get; set; }
        public CurrencyType Currency { get; set; }

        public override PacketID ID
        {
            get { return PacketID.PETYARDCOMMAND; }
        }

        public override Packet CreateInstance()
        {
            return new PetYardCommandPacket();
        }

        protected override void Read(Client client, NReader rdr)
        {
            CommandId = rdr.ReadByte();
            PetId1 = rdr.ReadInt32();
            PetId2 = rdr.ReadInt32();
            ObjectId = rdr.ReadInt32();
            ObjectSlot = ObjectSlot.Read(client, rdr);
            Currency = (CurrencyType)rdr.ReadByte();
        }

        protected override void Write(Client client, NWriter wtr)
        {
            wtr.Write(CommandId);
            wtr.Write(PetId1);
            wtr.Write(PetId2);
            wtr.Write(ObjectId);
            ObjectSlot.Write(client, wtr);
            wtr.Write((byte)Currency);
        }
    }
}

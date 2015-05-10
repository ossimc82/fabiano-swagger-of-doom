namespace wServer.networking.svrPackets
{
    public class HatchEggPacket : ServerPacket
    {
        public string PetName { get; set; }
        public int PetSkinId { get; set; }

        public override PacketID ID
        {
            get { return PacketID.HATCHEGG; }
        }

        public override Packet CreateInstance()
        {
            return new HatchEggPacket();
        }

        protected override void Read(Client client, NReader rdr)
        {
            PetName = rdr.ReadUTF();
            PetSkinId = rdr.ReadInt32();
        }

        protected override void Write(Client client, NWriter wtr)
        {
            wtr.WriteUTF(PetName);
            wtr.Write(PetSkinId);
        }
    }
}

namespace wServer.networking.cliPackets
{
    public class CreatePacket : ClientPacket
    {
        public int ClassType { get; set; }
        public int SkinType { get; set; }

        public override PacketID ID
        {
            get { return PacketID.CREATE; }
        }

        public override Packet CreateInstance()
        {
            return new CreatePacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            ClassType = rdr.ReadInt16();
            SkinType = rdr.ReadInt16();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.Write((ushort) ClassType);
            wtr.Write((ushort) SkinType);
        }
    }
}
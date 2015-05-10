namespace wServer.networking.cliPackets
{
    public class UsePortalPacket : ClientPacket
    {
        public int ObjectId { get; set; }

        public override PacketID ID
        {
            get { return PacketID.USEPORTAL; }
        }

        public override Packet CreateInstance()
        {
            return new UsePortalPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            ObjectId = rdr.ReadInt32();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.Write(ObjectId);
        }
    }
}
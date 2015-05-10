namespace wServer.networking.cliPackets
{
    public class ReskinPacket : ClientPacket
    {
        public int SkinId { get; set; }

        public override PacketID ID
        {
            get { return PacketID.RESKIN; }
        }

        public override Packet CreateInstance()
        {
            return new ReskinPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            SkinId = rdr.ReadInt32();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.Write(SkinId);
        }
    }
}
namespace wServer.networking.svrPackets
{
    public class GotoPacket : ServerPacket
    {
        public int ObjectId { get; set; }
        public Position Position { get; set; }

        public override PacketID ID
        {
            get { return PacketID.GOTO; }
        }

        public override Packet CreateInstance()
        {
            return new GotoPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            ObjectId = rdr.ReadInt32();
            Position = Position.Read(psr, rdr);
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.Write(ObjectId);
            Position.Write(psr, wtr);
        }
    }
}
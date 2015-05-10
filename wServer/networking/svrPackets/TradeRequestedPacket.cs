namespace wServer.networking.svrPackets
{
    public class TradeRequestedPacket : ServerPacket
    {
        public string Name { get; set; }

        public override PacketID ID
        {
            get { return PacketID.TRADEREQUESTED; }
        }

        public override Packet CreateInstance()
        {
            return new TradeRequestedPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            Name = rdr.ReadUTF();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.WriteUTF(Name);
        }
    }
}
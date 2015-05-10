namespace wServer.networking.cliPackets
{
    public class RequestTradePacket : ClientPacket
    {
        public string Name { get; set; }

        public override PacketID ID
        {
            get { return PacketID.REQUESTTRADE; }
        }

        public override Packet CreateInstance()
        {
            return new RequestTradePacket();
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
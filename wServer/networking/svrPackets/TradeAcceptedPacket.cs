namespace wServer.networking.svrPackets
{
    public class TradeAcceptedPacket : ServerPacket
    {
        public bool[] MyOffers { get; set; }
        public bool[] YourOffers { get; set; }

        public override PacketID ID
        {
            get { return PacketID.TRADEACCEPTED; }
        }

        public override Packet CreateInstance()
        {
            return new TradeAcceptedPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            MyOffers = new bool[rdr.ReadInt16()];
            for (int i = 0; i < MyOffers.Length; i++)
                MyOffers[i] = rdr.ReadBoolean();

            YourOffers = new bool[rdr.ReadInt16()];
            for (int i = 0; i < YourOffers.Length; i++)
                YourOffers[i] = rdr.ReadBoolean();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.Write((ushort) MyOffers.Length);
            foreach (bool i in MyOffers)
                wtr.Write(i);
            wtr.Write((ushort) YourOffers.Length);
            foreach (bool i in YourOffers)
                wtr.Write(i);
        }
    }
}
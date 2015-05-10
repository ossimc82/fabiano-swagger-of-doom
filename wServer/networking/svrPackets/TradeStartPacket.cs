namespace wServer.networking.svrPackets
{
    public class TradeStartPacket : ServerPacket
    {
        public TradeItem[] MyItems { get; set; }
        public string YourName { get; set; }
        public TradeItem[] YourItems { get; set; }

        public override PacketID ID
        {
            get { return PacketID.TRADESTART; }
        }

        public override Packet CreateInstance()
        {
            return new TradeStartPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            MyItems = new TradeItem[rdr.ReadInt16()];
            for (int i = 0; i < MyItems.Length; i++)
                MyItems[i] = TradeItem.Read(psr, rdr);

            YourName = rdr.ReadUTF();
            YourItems = new TradeItem[rdr.ReadInt16()];
            for (int i = 0; i < YourItems.Length; i++)
                YourItems[i] = TradeItem.Read(psr, rdr);
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.Write((ushort) MyItems.Length);
            foreach (TradeItem i in MyItems)
                i.Write(psr, wtr);

            wtr.WriteUTF(YourName);
            wtr.Write((ushort) YourItems.Length);
            foreach (TradeItem i in YourItems)
                i.Write(psr, wtr);
        }
    }
}
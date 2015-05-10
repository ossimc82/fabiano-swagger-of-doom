namespace wServer.networking.cliPackets
{
    public class InvSwapPacket : ClientPacket
    {
        public int Time { get; set; }
        public Position Position { get; set; }
        public ObjectSlot SlotObject1 { get; set; }
        public ObjectSlot SlotObject2 { get; set; }

        public override PacketID ID
        {
            get { return PacketID.INVSWAP; }
        }

        public override Packet CreateInstance()
        {
            return new InvSwapPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            Time = rdr.ReadInt32();
            Position = Position.Read(psr, rdr);
            SlotObject1 = ObjectSlot.Read(psr, rdr);
            SlotObject2 = ObjectSlot.Read(psr, rdr);
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.Write(Time);
            Position.Write(psr, wtr);
            SlotObject1.Write(psr, wtr);
            SlotObject2.Write(psr, wtr);
        }
    }
}
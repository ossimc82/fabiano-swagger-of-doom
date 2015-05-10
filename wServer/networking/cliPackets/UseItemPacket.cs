namespace wServer.networking.cliPackets
{
    public class UseItemPacket : ClientPacket
    {
        public int Time { get; set; }
        public ObjectSlot SlotObject { get; set; }
        public Position ItemUsePos { get; set; }
        public byte UseType { get; set; }

        public override PacketID ID
        {
            get { return PacketID.USEITEM; }
        }

        public override Packet CreateInstance()
        {
            return new UseItemPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            Time = rdr.ReadInt32();
            SlotObject = ObjectSlot.Read(psr, rdr);
            ItemUsePos = Position.Read(psr, rdr);
            UseType = rdr.ReadByte();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.Write(Time);
            SlotObject.Write(psr, wtr);
            ItemUsePos.Write(psr, wtr);
            wtr.Write(UseType);
        }
    }
}
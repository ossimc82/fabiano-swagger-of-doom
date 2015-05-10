namespace wServer.networking.cliPackets
{
    public class InvDropPacket : ClientPacket
    {
        public ObjectSlot SlotObject { get; set; }

        public override PacketID ID
        {
            get { return PacketID.INVDROP; }
        }

        public override Packet CreateInstance()
        {
            return new InvDropPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            SlotObject = ObjectSlot.Read(psr, rdr);
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            SlotObject.Write(psr, wtr);
        }
    }
}
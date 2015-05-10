namespace wServer.networking.cliPackets
{
    public class GotoAckPacket : ClientPacket
    {
        public int Time { get; set; }

        public override PacketID ID
        {
            get { return PacketID.GOTOACK; }
        }

        public override Packet CreateInstance()
        {
            return new GotoAckPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            Time = rdr.ReadInt32();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.Write(Time);
        }
    }
}
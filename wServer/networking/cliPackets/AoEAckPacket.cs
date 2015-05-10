namespace wServer.networking.cliPackets
{
    public class AOEAckPacket : ClientPacket
    {
        public int Time { get; set; }
        public Position Position { get; set; }

        public override PacketID ID
        {
            get { return PacketID.AOEACK; }
        }

        public override Packet CreateInstance()
        {
            return new AOEAckPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            Time = rdr.ReadInt32();
            Position = Position.Read(psr, rdr);
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.Write(Time);
            Position.Write(psr, wtr);
        }
    }
}
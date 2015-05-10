namespace wServer.networking.cliPackets
{
    public class GroundDamagePacket : ClientPacket
    {
        public int Time { get; set; }
        public Position Position { get; set; }

        public override PacketID ID
        {
            get { return PacketID.GROUNDDAMAGE; }
        }

        public override Packet CreateInstance()
        {
            return new GroundDamagePacket();
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
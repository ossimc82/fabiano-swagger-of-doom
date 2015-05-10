namespace wServer.networking.cliPackets
{
    public class PongPacket : ClientPacket
    {
        public int Serial { get; set; }
        public int Time { get; set; }

        public override PacketID ID
        {
            get { return PacketID.PONG; }
        }

        public override Packet CreateInstance()
        {
            return new PongPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            Serial = rdr.ReadInt32();
            Time = rdr.ReadInt32();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.Write(Serial);
            wtr.Write(Time);
        }
    }
}
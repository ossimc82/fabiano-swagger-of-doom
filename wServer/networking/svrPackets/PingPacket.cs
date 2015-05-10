namespace wServer.networking.svrPackets
{
    public class PingPacket : ServerPacket
    {
        public int Serial { get; set; }

        public override PacketID ID
        {
            get { return PacketID.PING; }
        }

        public override Packet CreateInstance()
        {
            return new PingPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            Serial = rdr.ReadInt32();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.Write(Serial);
        }
    }
}
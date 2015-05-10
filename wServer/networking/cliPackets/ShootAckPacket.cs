namespace wServer.networking.cliPackets
{
    public class ShootAckPacket : ClientPacket
    {
        public int Time { get; set; }

        public override PacketID ID
        {
            get { return PacketID.SHOOTACK; }
        }

        public override Packet CreateInstance()
        {
            return new ShootAckPacket();
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
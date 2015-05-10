namespace wServer.networking.svrPackets
{
    public class ArenaNextWavePacket : ServerPacket
    {
        public int Type { get; set; } //Not sure for what the type is, but u need it

        public override PacketID ID
        {
            get { return PacketID.ARENANEXTWAVE; }
        }

        public override Packet CreateInstance()
        {
            return new ArenaNextWavePacket();
        }

        protected override void Read(Client client, NReader rdr)
        {
            Type = rdr.ReadInt32();
        }

        protected override void Write(Client client, NWriter wtr)
        {
            wtr.Write(Type);
        }
    }
}
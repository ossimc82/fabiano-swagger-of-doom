namespace wServer.networking.svrPackets
{
    public class ArenaDeathPacket : ServerPacket
    {
        public int RestartPrice { get; set; }

        public override PacketID ID
        {
            get { return PacketID.ARENADEATH; }
        }

        public override Packet CreateInstance()
        {
            return new ArenaDeathPacket();
        }

        protected override void Read(Client client, NReader rdr)
        {
            RestartPrice = rdr.ReadInt32();
        }

        protected override void Write(Client client, NWriter wtr)
        {
            wtr.Write(RestartPrice);
        }
    }
}
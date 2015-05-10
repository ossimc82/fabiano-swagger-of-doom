namespace wServer.networking.svrPackets
{
    public class PicPacket : ServerPacket
    {
        public BitmapData BitmapData { get; set; }

        public override PacketID ID
        {
            get { return PacketID.PIC; }
        }

        public override Packet CreateInstance()
        {
            return new PicPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            BitmapData = BitmapData.Read(psr, rdr);
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            BitmapData.Write(psr, wtr);
        }
    }
}
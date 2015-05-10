namespace wServer.networking.cliPackets
{
    public class SquareHitPacket : ClientPacket
    {
        public int Time { get; set; }
        public byte BulletId { get; set; }
        public int ObjectId { get; set; }

        public override PacketID ID
        {
            get { return PacketID.SQUAREHIT; }
        }

        public override Packet CreateInstance()
        {
            return new SquareHitPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            Time = rdr.ReadInt32();
            BulletId = rdr.ReadByte();
            ObjectId = rdr.ReadInt32();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.Write(Time);
            wtr.Write(BulletId);
            wtr.Write(ObjectId);
        }
    }
}
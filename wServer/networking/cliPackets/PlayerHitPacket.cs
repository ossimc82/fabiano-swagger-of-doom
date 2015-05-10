namespace wServer.networking.cliPackets
{
    public class PlayerHitPacket : ClientPacket
    {
        public byte BulletId { get; set; }
        public int ObjectId { get; set; }

        public override PacketID ID
        {
            get { return PacketID.PLAYERHIT; }
        }

        public override Packet CreateInstance()
        {
            return new PlayerHitPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            BulletId = rdr.ReadByte();
            ObjectId = rdr.ReadInt32();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.Write(BulletId);
            wtr.Write(ObjectId);
        }
    }
}
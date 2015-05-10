namespace wServer.networking.cliPackets
{
    public class PlayerShootPacket : ClientPacket
    {
        public int Time { get; set; }
        public byte BulletId { get; set; }
        public short ContainerType { get; set; }
        public Position Position { get; set; }
        public float Angle { get; set; }

        public override PacketID ID
        {
            get { return PacketID.PLAYERSHOOT; }
        }

        public override Packet CreateInstance()
        {
            return new PlayerShootPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            Time = rdr.ReadInt32();
            BulletId = rdr.ReadByte();
            ContainerType = rdr.ReadInt16();
            Position = Position.Read(psr, rdr);
            Angle = rdr.ReadSingle();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.Write(Time);
            wtr.Write(BulletId);
            wtr.Write(ContainerType);
            Position.Write(psr, wtr);
            wtr.Write(Angle);
        }
    }
}
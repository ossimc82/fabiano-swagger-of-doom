namespace wServer.networking.svrPackets
{
    public class ShootPacket : ServerPacket
    {
        public byte BulletId { get; set; }
        public int OwnerId { get; set; }
        public byte BulletType { get; set; }
        public Position Position { get; set; }
        public float Angle { get; set; }
        public short Damage { get; set; }
        public byte NumShots { get; set; }
        public float AngleInc { get; set; }

        public override PacketID ID
        {
            get { return PacketID.SHOOT; }
        }

        public override Packet CreateInstance()
        {
            return new ShootPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            BulletId = rdr.ReadByte();
            OwnerId = rdr.ReadInt32();
            BulletType = rdr.ReadByte();
            Position = Position.Read(psr, rdr);
            Angle = rdr.ReadSingle();
            Damage = rdr.ReadInt16();
            if (rdr.BaseStream.Length - rdr.BaseStream.Position <= 0) return;
            NumShots = rdr.ReadByte();
            AngleInc = rdr.ReadSingle();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.Write(BulletId);
            wtr.Write(OwnerId);
            wtr.Write(BulletType);
            Position.Write(psr, wtr);
            wtr.Write(Angle);
            wtr.Write(Damage);
            if (NumShots == 1 || AngleInc == 0) return;
            wtr.Write(NumShots);
            wtr.Write(AngleInc);
        }
    }
}
namespace wServer.networking.svrPackets
{
    public class Shoot2Packet : ServerPacket
    {
        public byte BulletId { get; set; }
        public int OwnerId { get; set; }
        public int ContainerType { get; set; }
        public Position StartingPos { get; set; }
        public float Angle { get; set; }
        public short Damage { get; set; }

        public override PacketID ID
        {
            get { return PacketID.SHOOT2; }
        }

        public override Packet CreateInstance()
        {
            return new Shoot2Packet();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            BulletId = rdr.ReadByte();
            OwnerId = rdr.ReadInt32();
            ContainerType = rdr.ReadInt32();
            StartingPos = Position.Read(psr, rdr);
            Angle = rdr.ReadSingle();
            Damage = rdr.ReadInt16();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.Write(BulletId);
            wtr.Write(OwnerId);
            wtr.Write(ContainerType);
            StartingPos.Write(psr, wtr);
            wtr.Write(Angle);
            wtr.Write(Damage);
        }
    }
}
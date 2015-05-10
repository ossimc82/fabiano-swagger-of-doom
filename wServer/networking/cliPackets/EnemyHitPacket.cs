namespace wServer.networking.cliPackets
{
    public class EnemyHitPacket : ClientPacket
    {
        public int Time { get; set; }
        public byte BulletId { get; set; }
        public int TargetId { get; set; }
        public bool Killed { get; set; }

        public override PacketID ID
        {
            get { return PacketID.ENEMYHIT; }
        }

        public override Packet CreateInstance()
        {
            return new EnemyHitPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            Time = rdr.ReadInt32();
            BulletId = rdr.ReadByte();
            TargetId = rdr.ReadInt32();
            Killed = rdr.ReadBoolean();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.Write(Time);
            wtr.Write(BulletId);
            wtr.Write(TargetId);
            wtr.Write(Killed);
        }
    }
}
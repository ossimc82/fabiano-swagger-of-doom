namespace wServer.networking.svrPackets
{
    public class ShowEffectPacket : ServerPacket
    {
        public EffectType EffectType { get; set; }
        public int TargetId { get; set; }
        public Position PosA { get; set; }
        public Position PosB { get; set; }
        public ARGB Color { get; set; }

        public override PacketID ID
        {
            get { return PacketID.SHOW_EFFECT; }
        }

        public override Packet CreateInstance()
        {
            return new ShowEffectPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            EffectType = (EffectType) rdr.ReadByte();
            TargetId = rdr.ReadInt32();
            PosA = Position.Read(psr, rdr);
            PosB = Position.Read(psr, rdr);
            Color = ARGB.Read(psr, rdr);
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.Write((byte) EffectType);
            wtr.Write(TargetId);
            PosA.Write(psr, wtr);
            PosB.Write(psr, wtr);
            Color.Write(psr, wtr);
        }
    }
}
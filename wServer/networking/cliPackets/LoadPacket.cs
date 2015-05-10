namespace wServer.networking.cliPackets
{
    public class LoadPacket : ClientPacket
    {
        public int CharacterId { get; set; }
        public bool IsFromArena { get; set; }

        public override PacketID ID
        {
            get { return PacketID.LOAD; }
        }

        public override Packet CreateInstance()
        {
            return new LoadPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            CharacterId = rdr.ReadInt32();
            IsFromArena = rdr.ReadBoolean();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.Write(CharacterId);
            wtr.Write(IsFromArena);
        }
    }
}
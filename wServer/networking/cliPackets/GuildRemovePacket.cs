namespace wServer.networking.cliPackets
{
    public class GuildRemovePacket : ClientPacket
    {
        public string Name { get; set; }

        public override PacketID ID
        {
            get { return PacketID.GUILDREMOVE; }
        }

        public override Packet CreateInstance()
        {
            return new GuildRemovePacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            Name = rdr.ReadUTF();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.WriteUTF(Name);
        }
    }
}
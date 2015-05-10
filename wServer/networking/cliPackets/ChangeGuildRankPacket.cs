namespace wServer.networking.cliPackets
{
    public class ChangeGuildRankPacket : ClientPacket
    {
        public string Name { get; set; }
        public int GuildRank { get; set; }

        public override PacketID ID
        {
            get { return PacketID.CHANGEGUILDRANK; }
        }

        public override Packet CreateInstance()
        {
            return new ChangeGuildRankPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            Name = rdr.ReadUTF();
            GuildRank = rdr.ReadInt32();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.WriteUTF(Name);
            wtr.Write(GuildRank);
        }
    }
}
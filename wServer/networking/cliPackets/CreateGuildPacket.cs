namespace wServer.networking.cliPackets
{
    public class CreateGuildPacket : ClientPacket
    {
        public string Name { get; set; }

        public override PacketID ID
        {
            get { return PacketID.CREATEGUILD; }
        }

        public override Packet CreateInstance()
        {
            return new CreateGuildPacket();
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
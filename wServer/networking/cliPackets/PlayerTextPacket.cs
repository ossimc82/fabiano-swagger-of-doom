namespace wServer.networking.cliPackets
{
    public class PlayerTextPacket : ClientPacket
    {
        public string Text { get; set; }

        public override PacketID ID
        {
            get { return PacketID.PLAYERTEXT; }
        }

        public override Packet CreateInstance()
        {
            return new PlayerTextPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            Text = rdr.ReadUTF();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.WriteUTF(Text);
        }
    }
}
namespace wServer.networking.cliPackets
{
    public class CheckCreditsPacket : ClientPacket
    {
        public override PacketID ID
        {
            get { return PacketID.CHECKCREDITS; }
        }

        public override Packet CreateInstance()
        {
            return new CheckCreditsPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
        }

        protected override void Write(Client psr, NWriter wtr)
        {
        }
    }
}
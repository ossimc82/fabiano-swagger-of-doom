namespace wServer.networking.svrPackets
{
    public class NameResultPacket : ServerPacket
    {
        public bool Success { get; set; }
        public string ErrorText { get; set; }

        public override PacketID ID
        {
            get { return PacketID.NAMERESULT; }
        }

        public override Packet CreateInstance()
        {
            return new NameResultPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            Success = rdr.ReadBoolean();
            ErrorText = rdr.ReadUTF();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.Write(Success);
            wtr.WriteUTF(ErrorText);
        }
    }
}
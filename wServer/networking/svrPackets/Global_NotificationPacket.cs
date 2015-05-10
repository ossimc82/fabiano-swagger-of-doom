namespace wServer.networking.svrPackets
{
    public class Global_NotificationPacket : ServerPacket
    {
        public int Type { get; set; }
        public string Text { get; set; }

        public override PacketID ID
        {
            get { return PacketID.GLOBAL_NOTIFICATION; }
        }

        public override Packet CreateInstance()
        {
            return new Global_NotificationPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            Type = rdr.ReadInt32();
            Text = rdr.ReadUTF();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.Write(Type);
            wtr.WriteUTF(Text);
        }
    }
}
namespace wServer.networking.svrPackets
{
    public class TextPacket : ServerPacket
    {
        public string Name { get; set; }
        public int ObjectId { get; set; }
        public int Stars { get; set; }
        public byte BubbleTime { get; set; }
        public string Recipient { get; set; }
        public string Text { get; set; }
        public string CleanText { get; set; }

        public override PacketID ID
        {
            get { return PacketID.TEXT; }
        }

        public override Packet CreateInstance()
        {
            return new TextPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            Name = rdr.ReadUTF();
            ObjectId = rdr.ReadInt32();
            Stars = rdr.ReadInt32();
            BubbleTime = rdr.ReadByte();
            Recipient = rdr.ReadUTF();
            Text = rdr.ReadUTF();
            CleanText = rdr.ReadUTF();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.WriteUTF(Name);
            wtr.Write(ObjectId);
            wtr.Write(Stars);
            wtr.Write(BubbleTime);
            wtr.WriteUTF(Recipient);
            wtr.WriteUTF(Text);
            wtr.WriteUTF(CleanText);
        }
    }
}
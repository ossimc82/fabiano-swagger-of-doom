namespace wServer.networking.svrPackets
{
    public class ClientStatPacket : ServerPacket
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public override PacketID ID
        {
            get { return PacketID.CLIENTSTAT; }
        }

        public override Packet CreateInstance()
        {
            return new ClientStatPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            Name = rdr.ReadUTF();
            Value = rdr.ReadInt32();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.WriteUTF(Name);
            wtr.Write(Value);
        }
    }
}
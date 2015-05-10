namespace wServer.networking.svrPackets
{
    public class DeathPacket : ServerPacket
    {
        public string AccountId { get; set; }
        public int CharId { get; set; }
        public string Killer { get; set; }
        public int obf0 { get; set; }
        public int obf1 { get; set; }

        public override PacketID ID
        {
            get { return PacketID.DEATH; }
        }

        public override Packet CreateInstance()
        {
            return new DeathPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            AccountId = rdr.ReadUTF();
            CharId = rdr.ReadInt32();
            Killer = rdr.ReadUTF();
            obf0 = rdr.ReadInt32();
            obf1 = rdr.ReadInt32();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.WriteUTF(AccountId);
            wtr.Write(CharId);
            wtr.WriteUTF(Killer);
            wtr.Write(obf0);
            wtr.Write(obf1);
        }
    }
}
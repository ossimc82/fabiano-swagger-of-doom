namespace wServer.networking.svrPackets
{
    public class AccountListPacket : ServerPacket
    {
        public const int LOCKED_LIST_ID = 0;
        public const int IGNORED_LIST_ID = 1;

        public int AccountListId { get; set; }
        public string[] AccountIds { get; set; }
        public int LockAction { get; set; }

        public override PacketID ID
        {
            get { return PacketID.ACCOUNTLIST; }
        }

        public override Packet CreateInstance()
        {
            return new AccountListPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            AccountListId = rdr.ReadInt32();
            AccountIds = new string[rdr.ReadInt16()];
            for (int i = 0; i < AccountIds.Length; i++)
                AccountIds[i] = rdr.ReadUTF();
            LockAction = rdr.ReadInt32();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.Write(AccountListId);
            wtr.Write((ushort) AccountIds.Length);
            foreach (string i in AccountIds)
                wtr.WriteUTF(i);
            wtr.Write(LockAction);
        }
    }
}
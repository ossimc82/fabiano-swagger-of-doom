namespace wServer.networking.cliPackets
{
    public class HelloPacket : ClientPacket
    {
        public string BuildVersion { get; set; }
        public int GameId { get; set; }
        public string GUID { get; set; }
        public string Password { get; set; }
        public string Secret { get; set; }
        public int randomint1 { get; set; }
        public int KeyTime { get; set; }
        public byte[] Key { get; set; }
        public byte[] MapInfo { get; set; }
        public string obf1 { get; set; }
        public string obf2 { get; set; }
        public string obf3 { get; set; }
        public string obf4 { get; set; }
        public string obf5 { get; set; }

        public override PacketID ID
        {
            get { return PacketID.HELLO; }
        }

        public override Packet CreateInstance()
        {
            return new HelloPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            BuildVersion = rdr.ReadUTF();
            GameId = rdr.ReadInt32();
            GUID = RSA.Instance.Decrypt(rdr.ReadUTF());
            rdr.ReadInt32();
            Password = RSA.Instance.Decrypt(rdr.ReadUTF());
            randomint1 = rdr.ReadInt32();
            Secret = rdr.ReadUTF();
            KeyTime = rdr.ReadInt32();
            Key = rdr.ReadBytes(rdr.ReadInt16());
            MapInfo = rdr.ReadBytes(rdr.ReadInt32());
            obf1 = rdr.ReadUTF();
            obf2 = rdr.ReadUTF();
            obf3 = rdr.ReadUTF();
            obf4 = rdr.ReadUTF();
            obf5 = rdr.ReadUTF();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.WriteUTF(BuildVersion);
            wtr.Write(GameId);
            wtr.Write(0);
            wtr.WriteUTF(RSA.Instance.Encrypt(GUID));
            wtr.Write(randomint1);
            wtr.WriteUTF(RSA.Instance.Encrypt(Password));
            wtr.WriteUTF(Secret);
            wtr.Write(KeyTime);
            wtr.Write((ushort)Key.Length);
            wtr.Write(Key);
            wtr.Write(MapInfo.Length);
            wtr.Write(MapInfo);
            wtr.WriteUTF(obf1);
            wtr.WriteUTF(obf2);
            wtr.WriteUTF(obf3);
            wtr.WriteUTF(obf4);
            wtr.WriteUTF(obf5);
        }
    }
}
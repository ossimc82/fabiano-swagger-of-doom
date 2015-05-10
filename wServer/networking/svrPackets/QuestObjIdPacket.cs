namespace wServer.networking.svrPackets
{
    public class QuestObjIdPacket : ServerPacket
    {
        public int ObjectId { get; set; }

        public override PacketID ID
        {
            get { return PacketID.QUESTOBJID; }
        }

        public override Packet CreateInstance()
        {
            return new QuestObjIdPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            ObjectId = rdr.ReadInt32();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.Write(ObjectId);
        }
    }
}
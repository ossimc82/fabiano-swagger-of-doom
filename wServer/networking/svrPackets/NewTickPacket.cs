namespace wServer.networking.svrPackets
{
    public class NewTickPacket : ServerPacket
    {
        public int TickId { get; set; }
        public int TickTime { get; set; }
        public ObjectStats[] UpdateStatuses { get; set; }

        public override PacketID ID
        {
            get { return PacketID.NEW_TICK; }
        }

        public override Packet CreateInstance()
        {
            return new NewTickPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            TickId = rdr.ReadInt32();
            TickTime = rdr.ReadInt32();

            UpdateStatuses = new ObjectStats[rdr.ReadInt16()];
            for (int i = 0; i < UpdateStatuses.Length; i++)
                UpdateStatuses[i] = ObjectStats.Read(psr, rdr);
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.Write(TickId);
            wtr.Write(TickTime);

            wtr.Write((ushort) UpdateStatuses.Length);
            foreach (ObjectStats i in UpdateStatuses)
                i.Write(psr, wtr);
        }
    }
}
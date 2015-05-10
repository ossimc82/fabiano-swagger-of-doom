namespace wServer.networking.cliPackets
{
    public class MovePacket : ClientPacket
    {
        public int TickId { get; set; }
        public int Time { get; set; }
        public Position Position { get; set; }
        public TimedPosition[] Records { get; set; }

        public override PacketID ID
        {
            get { return PacketID.MOVE; }
        }

        public override Packet CreateInstance()
        {
            return new MovePacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            TickId = rdr.ReadInt32();
            Time = rdr.ReadInt32();
            Position = Position.Read(psr, rdr);
            Records = new TimedPosition[rdr.ReadInt16()];
            for (int i = 0; i < Records.Length; i++)
                Records[i] = TimedPosition.Read(psr, rdr);
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.Write(TickId);
            wtr.Write(Time);
            Position.Write(psr, wtr);
            if (Records == null)
            {
                wtr.Write((ushort) 0);
                return;
            }
            wtr.Write((ushort) Records.Length);
            foreach (TimedPosition i in Records)
                i.Write(psr, wtr);
        }
    }
}
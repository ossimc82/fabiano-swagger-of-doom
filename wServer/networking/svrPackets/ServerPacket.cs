namespace wServer.networking.svrPackets
{
    public abstract class ServerPacket : Packet
    {
        public override void Crypt(Client client, byte[] dat, int offset, int len)
        {
            client.SendKey.Crypt(dat, offset, len);
        }
    }
}
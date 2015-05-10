namespace wServer.networking.cliPackets
{
    public abstract class ClientPacket : Packet
    {
        public override void Crypt(Client client, byte[] dat, int offset, int len)
        {
            client.ReceiveKey.Crypt(dat, offset, len);
        }
    }
}
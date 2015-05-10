#region

using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;

#endregion

namespace wServer
{
    public class RC4
    {
        private readonly RC4Engine rc4;

        public RC4(byte[] key)
        {
            rc4 = new RC4Engine();
            rc4.Init(true, new KeyParameter(key));
        }

        public void Crypt(byte[] buf, int offset, int len) => rc4.ProcessBytes(buf, offset, len, buf, offset);
    }
}
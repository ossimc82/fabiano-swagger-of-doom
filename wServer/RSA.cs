#region

using System;
using System.IO;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.OpenSsl;

#endregion

namespace wServer
{
    public class RSA
    {
        public static readonly RSA Instance = new RSA(@"
-----BEGIN RSA PRIVATE KEY-----
MIICXAIBAAKBgQCbqweYUxzW0IiCwuBAzx6HtskrhWW+B0iX4LMu2xqRh4gh52HU
Vu9nNiXso7utTKCv/HNK19v5xoWp3Cne23sicp2oVGgKMFSowBFbtr+fhsq0yHv+
JxixkL3WLnXcY3xREz7LOzVMoybUCmJzzhnzIsLPiIPdpI1PxFDcnFbdRQIDAQAB
AoGAbjGLltB+wbGscKPyiu4S9o71qNEtTG9re9eb/7cp/4qpWxanseA4aB90iSb+
W5a6yNkz4+8Z0J4vUCaBnThQ2Nyoj4B6HUJpih6f9NbcaqTj/8zibr1YyeEzo4rw
dO1ptPb9y5Pv8DOAInEb3NhqitLBRm1jguxpK9Ybbnob2QECQQDjpAmsqxk2w0Q0
IgMlx5Cn9uE/iTXaEuqoYRRig2TH7zhzsoll3XfLyuBdkm0tSyUrA4+V7wYXjCoU
dEEHhzhJAkEArw+gGzbAWHzLwgvBru/WtceSdaT6XPYyp+xssSD0BYIL8xmkIsyS
0x6Oh99Ec9Ov1M4qGliJlxdZ3vgVyiuVHQJBAJWXh5ADg/c7zIchzsW15jaqgw0Y
ot3iznfGC/pM9B568rL9IVNifUXb1SNIhRxdpFgm5+WUhIFW55Q3bUCAOJkCQBah
VnkuIr9Noql7C5apun/VRMGgihzqVrIOhh5/vAvaO+E5N1aoS3KvSI2X9ylh/CDu
ZdLyDxdRFXUVbPutlqECQE+4PbsqiekYX4BWRTAnOy5Ly+/ivTWOWNJxHicuNu8i
0zLn+R6ZamUkKcQI5N/91TGJvkIKXRJTYcII+w5gSdw=
-----END RSA PRIVATE KEY-----");

/*        
-----BEGIN PUBLIC KEY-----
MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCbqweYUxzW0IiCwuBAzx6Htskr
hWW+B0iX4LMu2xqRh4gh52HUVu9nNiXso7utTKCv/HNK19v5xoWp3Cne23sicp2o
VGgKMFSowBFbtr+fhsq0yHv+JxixkL3WLnXcY3xREz7LOzVMoybUCmJzzhnzIsLP
iIPdpI1PxFDcnFbdRQIDAQAB
-----END PUBLIC KEY-----
*/

        private readonly RsaEngine engine;
        private readonly AsymmetricKeyParameter key;

        private RSA(string privPem)
        {
            key = (new PemReader(new StringReader(privPem.Trim())).ReadObject() as AsymmetricCipherKeyPair).Private;
            engine = new RsaEngine();
            engine.Init(true, key);
        }

        public string Decrypt(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            byte[] dat = Convert.FromBase64String(str);
            Pkcs1Encoding encoding = new Pkcs1Encoding(engine);
            encoding.Init(false, key);
            return Encoding.UTF8.GetString(encoding.ProcessBlock(dat, 0, dat.Length));
        }

        public string Encrypt(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            byte[] dat = Encoding.UTF8.GetBytes(str);
            Pkcs1Encoding encoding = new Pkcs1Encoding(engine);
            encoding.Init(true, key);
            return Convert.ToBase64String(encoding.ProcessBlock(dat, 0, dat.Length));
        }
    }
}
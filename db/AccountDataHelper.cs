using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using db.data;

namespace db
{
    public class AccountDataHelper
    {
        public static AccountGiftCodeData GetAccountGiftCodeData(string httpData)
        {
            return new AccountGiftCodeData(Convert.FromBase64String(httpData));
        }

        public static AccountGiftCodeData GenerateAccountGiftCodeData(string accId, string giftCode)
        {
            return new AccountGiftCodeData(accId, giftCode);
        }

        public class AccountGiftCodeData
        {
            private readonly string accId;
            private readonly string giftCode;

            public AccountGiftCodeData(byte[] data)
            {
                var rdr = new NReader(new MemoryStream(data));
                accId = rdr.ReadUTF();
                giftCode = rdr.ReadUTF();
            }

            internal AccountGiftCodeData(string accId, string giftCode)
            {
                this.accId = accId;
                this.giftCode = giftCode;
            }

            public Account GetAccount(XmlData data)
            {
                using (var db = new Database())
                    return db.GetAccount(accId, data);
            }

            public AccountGiftCodeData GetGiftCode(out string giftCode)
            {
                giftCode = this.giftCode;
                return this;
            }

            public string Write()
            {
                var wtr = new NWriter(new MemoryStream());
                wtr.WriteUTF(accId);
                wtr.WriteUTF(giftCode);
                return Convert.ToBase64String(((MemoryStream)wtr.BaseStream).ToArray());
            }
        }
    }
}

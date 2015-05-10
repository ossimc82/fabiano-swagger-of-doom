#region

using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using db;
using MySql.Data.MySqlClient;

#endregion

namespace server.account
{
    internal class purchaseCharSlot : RequestHandler
    {
        protected override void HandleRequest()
        {
            using (Database db = new Database())
            {
                Account acc = db.Verify(Query["guid"], Query["password"], Program.GameData);
                byte[] status = new byte[0];
                if (CheckAccount(acc, db))
                {
                    MySqlCommand cmd = db.CreateQuery();
                    cmd.CommandText = "SELECT credits FROM stats WHERE accId=@accId;";
                    cmd.Parameters.AddWithValue("@accId", acc.AccountId);
                    if ((int) cmd.ExecuteScalar() < acc.NextCharSlotPrice)
                        status = Encoding.UTF8.GetBytes("<Error>Not enough Gold</Error>");
                    else
                    {
                        cmd = db.CreateQuery();
                        cmd.CommandText = "UPDATE stats SET credits = credits - @charSlotPrice WHERE accId=@accId";
                        cmd.Parameters.AddWithValue("@accId", acc.AccountId);
                        cmd.Parameters.AddWithValue("@charSlotPrice", acc.NextCharSlotPrice);
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            cmd = db.CreateQuery();
                            cmd.CommandText = "UPDATE accounts SET maxCharSlot = maxCharSlot + 1 WHERE id=@accId";
                            cmd.Parameters.AddWithValue("@accId", acc.AccountId);
                            if (cmd.ExecuteNonQuery() > 0)
                                status = Encoding.UTF8.GetBytes("<Success/>");
                            else
                                status = Encoding.UTF8.GetBytes("<Error>Internal Error</Error>");
                        }
                        else
                            status = Encoding.UTF8.GetBytes("<Error>Internal Error</Error>");
                    }
                }
                Context.Response.OutputStream.Write(status, 0, status.Length);
            }
        }
    }
}
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
    internal class changePassword : RequestHandler
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
                    cmd.CommandText = "UPDATE accounts SET password=SHA1(@password) WHERE id=@accId;";
                    cmd.Parameters.AddWithValue("@accId", acc.AccountId);
                    cmd.Parameters.AddWithValue("@password", Query["newPassword"]);
                    if (cmd.ExecuteNonQuery() > 0)
                        status = Encoding.UTF8.GetBytes("<Success />");
                    else
                        status = Encoding.UTF8.GetBytes("<Error>Internal error</Error>");
                }
                Context.Response.OutputStream.Write(status, 0, status.Length);
            }
        }
    }
}
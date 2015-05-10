#region

using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using db;
using MySql.Data.MySqlClient;

#endregion

namespace server.@char
{
    internal class delete : RequestHandler
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
                    cmd.CommandText = @"DELETE FROM characters WHERE accId = @accId AND charId = @charId;";
                    cmd.Parameters.AddWithValue("@accId", acc.AccountId);
                    cmd.Parameters.AddWithValue("@charId", Query["charId"]);
                    if (cmd.ExecuteNonQuery() > 0)
                        status = Encoding.UTF8.GetBytes("<Success />");
                    else
                        status = Encoding.UTF8.GetBytes("<Error>Internal Error</Error>");
                }
                Context.Response.OutputStream.Write(status, 0, status.Length);
            }
        }
    }
}
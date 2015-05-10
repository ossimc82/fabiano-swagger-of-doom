#region

using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Web;
using db;
using MySql.Data.MySqlClient;

#endregion

namespace server.account
{
    internal class verifyage : RequestHandler
    {
        protected override void HandleRequest()
        {
            using (Database db = new Database())
            {
                Account acc = db.Verify(Query["guid"], Query["password"], Program.GameData);
                if (CheckAccount(acc, db))
                {
                    MySqlCommand cmd = db.CreateQuery();
                    cmd.CommandText = "UPDATE accounts SET isAgeVerified=@newIsAgeVerified WHERE uuid=@uuid;";
                    cmd.Parameters.AddWithValue("@newIsAgeVerified", Query["isAgeVerified"]);
                    cmd.Parameters.AddWithValue("@uuid", Query["guid"]);
                    if (cmd.ExecuteNonQuery() == 1)
                        using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                            wtr.Write("<Success/>");
                    else
                        using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                            wtr.Write("<Error>Error.accountNotFound</Error>");
                }
                else
                    using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                        wtr.Write("<Error>Error.accountNotFound</Error>");
            }
        }
    }
}
using db;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace server.account
{
    internal class acceptTOS : RequestHandler
    {
        protected override void HandleRequest()
        {
            using (Database db = new Database())
            {
                Account acc;
                if (CheckAccount(acc = db.Verify(Query["guid"], Query["password"], Program.GameData), db))
                {
                    if (acc.NotAcceptedNewTos == null)
                        using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                            wtr.Write("<Error>TOS Already Accepted</Error>");
                    else
                    {
                        var cmd = db.CreateQuery();
                        cmd.CommandText = "Update accounts SET acceptedNewTos=1 WHERE uuid=@uuid;";
                        cmd.Parameters.AddWithValue("@uuid", Query["guid"]);
                        using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                            wtr.Write(cmd.ExecuteNonQuery() > 0 ? "<Success/>" : "<Error>Internal Server Error</Error>");
                    }
                }
                else
                    using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                        wtr.Write("<Error>Account not found</Error>");
            }
        }
    }
}

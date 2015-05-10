using db;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Web;

namespace server.account
{
    internal class validateEmail : RequestHandler
    {
        protected override void HandleRequest()
        {
            using (Database db = new Database())
            {
                var cmd = db.CreateQuery();
                cmd.CommandText = "UPDATE accounts SET verified=1 WHERE authToken=@authToken";
                cmd.Parameters.AddWithValue("@authToken", Query["authToken"]);
                using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                {
                    if (cmd.ExecuteNonQuery() == 1)
                        Program.SendFile("game/verifySuccess.html", Context);
                    else
                        Program.SendFile("game/verifyFail.html", Context);
                }
            }
        }
    }
}

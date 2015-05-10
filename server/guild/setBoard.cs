#region

using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using db;

#endregion

namespace server.guild
{
    internal class setBoard : RequestHandler
    {
        protected override void HandleRequest()
        {
            using (Database db = new Database())
            {
                Account acc = db.Verify(Query["guid"], Query["password"], Program.GameData);
                byte[] status = new byte[0];
                if (CheckAccount(acc, db, false))
                {
                    status = Encoding.UTF8.GetBytes(db.SetGuildBoard(Query["board"], acc));
                }
                Context.Response.OutputStream.Write(status, 0, status.Length);
            }
        }
    }
}
#region

using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using db;

#endregion

namespace server.guild
{
    internal class getBoard : RequestHandler
    {
        protected override void HandleRequest()
        {
            using (Database db = new Database())
            {
                Account acc = db.Verify(Query["guid"], Query["password"], Program.GameData);
                byte[] status = new byte[0];
                if (CheckAccount(acc, db, false))
                {
                    try
                    {
                        status = Encoding.UTF8.GetBytes(db.GetGuildBoard(acc));
                    }
                    catch (Exception e)
                    {
                        status = Encoding.UTF8.GetBytes("<Error>" + e.Message + "</Error>");
                    }
                }
                Context.Response.OutputStream.Write(status, 0, status.Length);
            }
        }
    }
}
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
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Serialization;

namespace server.account
{
    internal class getProdAccount : RequestHandler
    {
        public const string TRANSFERENGINEVERSION = "v1.0 (beta)";

        protected override void HandleRequest()
        {
            string status = "<Error>Internal server error</Error>";
            using(Database db = new Database())
            {
                Account acc;
                if (CheckAccount(acc = db.Verify(Query["guid"], Query["password"], Program.GameData), db))
                {
                    if (acc.Rank < 1)
                    {
                        status = "<Error>Only donators can port prod accounts to the private server.</Error>";
                    }
                    else if (acc.IsProdAccount && acc.Rank < 2)
                    {
                        status = "<Error>You account is already transfered.</Error>";
                    }
                    else if (!acc.Banned)
                    {
                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(String.Format("http://www.realmofthemadgodhrd.appspot.com/char/list?guid={0}&password={1}", Query["prodGuid"], Query["prodPassword"]));
                        var resp = req.GetResponse();

                        Chars chrs = new Chars();
                        chrs.Characters = new List<Char>();

                        string s;
                        using (StreamReader rdr = new StreamReader(resp.GetResponseStream()))
                            s = rdr.ReadToEnd();

                        s = s.Replace("False", "false").Replace("True", "true");

                        try
                        {
                            XmlSerializer serializer = new XmlSerializer(chrs.GetType(),
                                new XmlRootAttribute("Chars") { Namespace = "" });
                            chrs = (Chars)serializer.Deserialize(new StringReader(s));

                            if (db.SaveChars(acc.AccountId, new server.@char.list().GetChars(Query["guid"], Query["password"], Program.GameData), chrs, Program.GameData))
                                status = "<Success />";
                        }
                        catch (Exception e)
                        {
                            Program.Logger.Error(e);
                        }
                    }
                    else
                        status = "<Error>Account under Maintenance</Error>";
                }
                else
                    status = "<Error>Account credentials not valid</Error>";
            }

            using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                wtr.Write(status);
        }

        private const string body = @"";
    }
}

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
using System.Xml.Serialization;

namespace server.playerMuledump
{
    internal class view : RequestHandler
    {
        protected override void HandleRequest()
        {
            using (Database db = new Database())
            {
                Account a;
                if (Query["guid"].Contains("@") && !String.IsNullOrWhiteSpace(Query["password"]))
                {
                    a = db.Verify(Query["guid"], Query["password"], Program.GameData);
                    if (a == null)
                    {
                        using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                            wtr.WriteLine("<Error>Account credentials not valid</Error>");
                        Context.Response.Close();
                        return;
                    }
                }
                else
                {
                    a = db.GetAccountByName(Query["guid"], Program.GameData);
                    if (a != null)
                    {
                        if (!a.VisibleMuledump)
                        {
                            using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                                wtr.WriteLine("<Error>This player has a private muledump.</Error>");
                            Context.Response.Close();
                            return;
                        }
                    }
                    else
                    {
                        using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                            wtr.WriteLine("<Error>User not found</Error>");
                        Context.Response.Close();
                        return;
                    }
                }

                if (a.Banned)
                {
                    using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                        wtr.WriteLine("<Error>Account under maintenance</Error>");
                    Context.Response.Close();
                    return;
                }

                Chars chrs = new Chars
                {
                    Characters = new List<Char>(),
                    NextCharId = 2,
                    MaxNumChars = 1,
                    Account = a
                };

                if (chrs.Account != null)
                {
                    db.GetCharData(chrs.Account, chrs);
                    db.LoadCharacters(chrs.Account, chrs);
                    chrs.News = db.GetNews(Program.GameData, chrs.Account);
                    chrs.OwnedSkins = Utils.GetCommaSepString(chrs.Account.OwnedSkins.ToArray());
                }
                else
                {
                    using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                        wtr.WriteLine("<Error>User not found</Error>");
                    Context.Response.Close();
                    return;
                }

                XmlSerializer serializer = new XmlSerializer(chrs.GetType(),
                    new XmlRootAttribute(chrs.GetType().Name) { Namespace = "" });

                XmlWriterSettings xws = new XmlWriterSettings();
                xws.OmitXmlDeclaration = true;
                xws.Encoding = Encoding.UTF8;
                xws.Indent = true;
                XmlWriter xtw = XmlWriter.Create(Context.Response.OutputStream, xws);
                serializer.Serialize(xtw, chrs, chrs.Namespaces);
            }
        }
    }
}

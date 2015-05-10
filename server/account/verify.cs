#region

using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using db;

#endregion

namespace server.account
{
    internal class verify : RequestHandler
    {
        protected override void HandleRequest()
        {
            using (Database db = new Database())
            {
                Account acc = db.Verify(Query["guid"], Query["password"], Program.GameData);
                if (CheckAccount(acc, db))
                {
                    XmlSerializer serializer = new XmlSerializer(acc.GetType(),
                        new XmlRootAttribute(acc.GetType().Name) {Namespace = ""});

                    XmlWriterSettings xws = new XmlWriterSettings();
                    xws.Indent = true;
                    xws.OmitXmlDeclaration = true;
                    xws.Encoding = Encoding.UTF8;
                    XmlWriter xtw = XmlWriter.Create(Context.Response.OutputStream, xws);
                    serializer.Serialize(xtw, acc, acc.Namespaces);
                }
            }
        }
    }
}
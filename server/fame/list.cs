#region

using db;
using MySql.Data.MySqlClient;
using System;
using System.Text;
using System.Xml;

#endregion

namespace server.fame
{
    internal class list : RequestHandler
    {
        protected override void HandleRequest()
        {
            byte[] status = null;

            string span = "";
            switch (Query["timespan"])
            {
                case "week":
                    span = "(time >= DATE_SUB(NOW(), INTERVAL 1 WEEK))";
                    break;
                case "month":
                    span = "(time >= DATE_SUB(NOW(), INTERVAL 1 MONTH))";
                    break;
                case "all":
                    span = "TRUE";
                    break;
                default:
                    status = Encoding.UTF8.GetBytes("<Error>Invalid fame list</Error>");
                    break;
            }
            string ac = "FALSE";
            if (Query["accountId"] != null)
                ac = "(accId=@accId AND chrId=@charId)";

            if (status == null)
            {
                XmlDocument doc = new XmlDocument();
                XmlElement root = doc.CreateElement("FameList");

                XmlAttribute spanAttr = doc.CreateAttribute("timespan");
                spanAttr.Value = Query["timespan"];
                root.Attributes.Append(spanAttr);

                doc.AppendChild(root);

                using (Database db = new Database())
                {
                    MySqlCommand cmd = db.CreateQuery();
                    cmd.CommandText = @"SELECT * FROM death WHERE " + span + @" OR " + ac +
                                      @" ORDER BY totalFame DESC LIMIT 20;";
                    if (Query["accountId"] != null)
                    {
                        cmd.Parameters.AddWithValue("@accId", Query["accountId"]);
                        cmd.Parameters.AddWithValue("@charId", Query["charId"]);
                    }
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            XmlElement elem = doc.CreateElement("FameListElem");

                            int accId = rdr.GetInt32("accId");
                            XmlAttribute accIdAttr = doc.CreateAttribute("accountId");
                            accIdAttr.Value = accId.ToString();
                            elem.Attributes.Append(accIdAttr);
                            XmlAttribute chrIdAttr = doc.CreateAttribute("charId");
                            chrIdAttr.Value = rdr.GetInt32("chrId").ToString();
                            elem.Attributes.Append(chrIdAttr);

                            root.AppendChild(elem);

                            XmlElement nameElem = doc.CreateElement("Name");
                            nameElem.InnerText = String.Empty;
                            elem.AppendChild(nameElem);
                            XmlElement objTypeElem = doc.CreateElement("ObjectType");
                            objTypeElem.InnerText = rdr.GetString("charType");
                            elem.AppendChild(objTypeElem);
                            XmlElement tex1Elem = doc.CreateElement("Tex1");
                            tex1Elem.InnerText = rdr.GetString("tex1");
                            elem.AppendChild(tex1Elem);
                            XmlElement tex2Elem = doc.CreateElement("Tex2");
                            tex2Elem.InnerText = rdr.GetString("tex2");
                            elem.AppendChild(tex2Elem);
                            XmlElement skinElem = doc.CreateElement("Texture");
                            skinElem.InnerText = rdr.GetString("skin");
                            elem.AppendChild(skinElem);
                            XmlElement equElem = doc.CreateElement("Equipment");
                            equElem.InnerText = rdr.GetString("items");
                            elem.AppendChild(equElem);
                            XmlElement fameElem = doc.CreateElement("TotalFame");
                            fameElem.InnerText = rdr.GetString("totalFame");
                            elem.AppendChild(fameElem);
                        }
                    }

                    XmlNodeList list = doc.SelectNodes("/FameList/FameListElem");

                    foreach (XmlNode node in list)
                    {
                        foreach (XmlNode xnode in node.ChildNodes)
                        {
                            if (xnode.Name == "Name")
                                xnode.InnerText = db.GetAccount(node.Attributes["accountId"].Value, Program.GameData).Name;
                        }
                    }
                }

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                using (XmlWriter wtr = XmlWriter.Create(Context.Response.OutputStream))
                    doc.Save(wtr);
            }
        }
    }
}

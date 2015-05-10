#region

using db;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Web;
using System.Xml;

#endregion

namespace server.package
{
    internal class getPackages : RequestHandler
    {
        internal static Dictionary<string, Package> CurrentPackages { get; set; }

        protected override void HandleRequest()
        {
            string s = Serialize();

            using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                wtr.Write(s);
        }

        internal static string Serialize()
        {
            XmlDocument doc = new XmlDocument();
            XmlNode packageResponse = doc.CreateElement("PackageResponse");
            doc.AppendChild(packageResponse);

            XmlNode packages = doc.CreateElement("Packages");
            packageResponse.AppendChild(packages);

            using (Database db = new Database())
            {
                var cmd = db.CreateQuery();
                cmd.CommandText = "SELECT * FROM packages WHERE endDate >= @now;";
                cmd.Parameters.AddWithValue("@now", DateTime.UtcNow);

                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        XmlNode packageElem = doc.CreateElement("Package");
                        XmlAttribute packageElemId = doc.CreateAttribute("id");
                        packageElemId.Value = rdr.GetString("id");
                        packageElem.Attributes.Append(packageElemId);

                        XmlNode name = doc.CreateElement("Name");
                        name.InnerText = rdr.GetString("name");
                        packageElem.AppendChild(name);

                        XmlNode price = doc.CreateElement("Price");
                        price.InnerText = rdr.GetString("price");
                        packageElem.AppendChild(price);

                        XmlNode quantity = doc.CreateElement("Quantity");
                        quantity.InnerText = rdr.GetString("quantity");
                        packageElem.AppendChild(quantity);

                        XmlNode maxPurchase = doc.CreateElement("MaxPurchase");
                        maxPurchase.InnerText = rdr.GetString("maxPurchase");
                        packageElem.AppendChild(maxPurchase);

                        XmlNode weight = doc.CreateElement("Weight");
                        weight.InnerText = rdr.GetString("weight");
                        packageElem.AppendChild(weight);

                        XmlNode bgUrl = doc.CreateElement("BgURL");
                        bgUrl.InnerText = rdr.GetString("bgUrl");
                        packageElem.AppendChild(bgUrl);

                        XmlNode endDate = doc.CreateElement("EndDate");
                        DateTime dt = rdr.GetDateTime("endDate").Kind != DateTimeKind.Utc ?
                            rdr.GetDateTime("endDate").ToUniversalTime() : rdr.GetDateTime("endDate");
                        endDate.InnerText = String.Format("{0}/{1}/{2} {3} GMT-0000", dt.Day, dt.Month, dt.Year, dt.ToLongTimeString());
                        packageElem.AppendChild(endDate);
                        packages.AppendChild(packageElem);
                    }
                }
            }

            StringWriter wtr = new StringWriter();
            doc.Save(wtr);
            return wtr.ToString();
        }
    }

    public class Package
    {
        public int PackageId { get; set; }

        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int MaxPurchase { get; set; }
        public int Weight { get; set; }
        public string BgURL { get; set; }
        public DateTime EndDate { get; set; }
        public string Contents { get; set; }

        internal static Package GetPackage(int id)
        {
            using (Database db = new Database())
            {
                var cmd = db.CreateQuery();
                cmd.CommandText = "SELECT * FROM packages WHERE id=@id AND endDate >= now();";
                cmd.Parameters.AddWithValue("@id", id);

                using (var rdr = cmd.ExecuteReader())
                {
                    if (!rdr.HasRows) return null;
                    rdr.Read();

                    return new Package
                    {
                        BgURL = rdr.GetString("bgUrl"),
                        EndDate = rdr.GetDateTime("endDate"),
                        Weight = rdr.GetInt32("weight"),
                        MaxPurchase = rdr.GetInt32("maxPurchase"),
                        Name = rdr.GetString("name"),
                        PackageId = rdr.GetInt32("id"),
                        Price = rdr.GetInt32("price"),
                        Quantity = rdr.GetInt32("quantity"),
                        Contents = rdr.GetString("contents")
                    };
                }
            }
        }
    }
}
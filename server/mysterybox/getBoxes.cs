#region

using db;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Web;
using System.Xml;

#endregion

namespace server.mysterybox
{
    internal class getBoxes : RequestHandler
    {
        protected override void HandleRequest()
        {
            string s = MysteryBox.Serialize();

            //<FortuneGame id = "-3" title = "Armor of the Mad God #1" ><Description></Description><Contents>2835,2833,3105,3176,8812,3290,3279,3278,8851,8781,9017,9015,3239,3133,4103,2873,2872,3105,2762,2761,2766,2764,2759,2760,2765,9015,3276,3264,3275,3133,3177,3178,3270,1803,3138,3269,3293,3180,3274,3272</Contents><Price firstInGold="100" firstInToken="1" secondInGold="250"/><Image>http://rotmg.kabamcdn.com/MadGodArmorAlchemistRewards.png</Image><Icon></Icon><StartTime>2014-09-18 13:25:20</StartTime><EndTime>2014-09-23 13:33:00</EndTime></FortuneGame>

            using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                wtr.Write(s);
        }
    }

    internal class MysteryBox
    {
        internal int BoxId { get; set; }
        internal string Title { get; set; }
        internal int Weight { get; set; }
        internal string Description { get; set; }
        internal string Contents { get; set; }
        internal string Image { get; set; }
        internal string Icon { get; set; }
        internal Price Price { get; set; }
        internal DateTime StartTime { get; set; }
        internal Sale Sale { get; set; }

        internal static MysteryBox GetBox(int id)
        {
            using (Database db = new Database())
            {
                var cmd = db.CreateQuery();
                cmd.CommandText = "SELECT * FROM mysteryboxes WHERE id=@id AND boxEnd >= now();";
                cmd.Parameters.AddWithValue("@id", id);
                using(var rdr = cmd.ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        rdr.Read();

                        return new MysteryBox
                        {
                            BoxId = id,
                            Contents = rdr.GetString("contents"),
                            Weight = rdr.GetInt32("weight"),
                            Title = rdr.GetString("title"),
                            Description = rdr.GetString("description"),
                            Icon = rdr.GetString("icon"),
                            Image = rdr.GetString("image"),
                            StartTime = rdr.GetDateTime("startTime"),
                            Price = new Price
                            {
                                Amount = rdr.GetInt32("priceAmount"),
                                Currency = rdr.GetInt32("priceCurrency")
                            },
                            Sale = rdr.GetDateTime("saleEnd") == DateTime.MinValue ? null :
                            new Sale
                            {
                                SaleEnd = rdr.GetDateTime("saleEnd"),
                                Currency = rdr.GetInt32("saleCurrency"),
                                Price = rdr.GetInt32("salePrice")
                            }
                        };
                    }
                    else
                        return null;
                }
            }
        }

        internal static string Serialize()
        {
            XmlDocument doc = new XmlDocument();
            XmlNode minigames = doc.CreateElement("MiniGames");
            XmlAttribute minigamesVersion = doc.CreateAttribute("version");
            minigamesVersion.Value = "1402333568.446112";
            minigames.Attributes.Append(minigamesVersion);
            doc.AppendChild(minigames);

            using (Database db = new Database())
            {
                var cmd = db.CreateQuery();
                cmd.CommandText = "SELECT * FROM mysteryboxes WHERE boxEnd >= now();";
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        XmlNode boxElem = doc.CreateElement("MysteryBox");
                        XmlAttribute boxId = doc.CreateAttribute("id");
                        boxId.Value = rdr.GetString("id");
                        XmlAttribute boxTitle = doc.CreateAttribute("title");
                        boxTitle.Value = rdr.GetString("title");
                        XmlAttribute boxWeight = doc.CreateAttribute("weight");
                        boxWeight.Value = rdr.GetInt32("weight").ToString();
                        boxElem.Attributes.Append(boxId);
                        boxElem.Attributes.Append(boxTitle);
                        boxElem.Attributes.Append(boxWeight);

                        XmlNode desc = doc.CreateElement("Description");
                        desc.InnerText = rdr.GetString("description");
                        boxElem.AppendChild(desc);

                        XmlNode contents = doc.CreateElement("Contents");
                        contents.InnerText = rdr.GetString("contents");
                        boxElem.AppendChild(contents);

                        XmlNode price = doc.CreateElement("Price");
                        XmlAttribute priceAmount = doc.CreateAttribute("amount");
                        priceAmount.Value = rdr.GetInt32("priceAmount").ToString();
                        XmlAttribute priceCurrency = doc.CreateAttribute("currency");
                        priceCurrency.Value = rdr.GetInt32("priceCurrency").ToString();
                        price.Attributes.Append(priceAmount);
                        price.Attributes.Append(priceCurrency);
                        boxElem.AppendChild(price);

                        XmlNode image = doc.CreateElement("Image");
                        image.InnerText = rdr.GetString("image");
                        boxElem.AppendChild(image);

                        XmlNode icon = doc.CreateElement("Icon");
                        icon.InnerText = rdr.GetString("icon");
                        boxElem.AppendChild(icon);

                        XmlNode startTime = doc.CreateElement("StartTime");
                        startTime.InnerText = rdr.GetDateTime("startTime").ToString("yyyy-MM-dd HH:mm:ss");
                        boxElem.AppendChild(startTime);

                        if (rdr.GetDateTime("saleEnd") != DateTime.MinValue)
                        {
                            XmlNode salePrice = doc.CreateElement("Sale");
                            XmlNode saleEnd = doc.CreateElement("End");
                            saleEnd.InnerText = rdr.GetDateTime("saleEnd").ToString("yyyy-MM-dd HH:mm:ss");
                            XmlAttribute saleAmount = doc.CreateAttribute("price");
                            saleAmount.Value = rdr.GetInt32("salePrice").ToString();
                            XmlAttribute saleCurrency = doc.CreateAttribute("currency");
                            saleCurrency.Value = rdr.GetInt32("saleCurrency").ToString();
                            salePrice.Attributes.Append(saleAmount);
                            salePrice.Attributes.Append(saleCurrency);
                            salePrice.AppendChild(saleEnd);
                            boxElem.AppendChild(salePrice);
                        }
                        minigames.AppendChild(boxElem);
                    }
                }

                cmd = db.CreateQuery();
                cmd.CommandText = "SELECT * FROM theAlchemist WHERE endTime >= now();";
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        XmlNode boxElem = doc.CreateElement("FortuneGame");
                        XmlAttribute boxId = doc.CreateAttribute("id");
                        boxId.Value = rdr.GetString("id");
                        XmlAttribute boxTitle = doc.CreateAttribute("title");
                        boxTitle.Value = rdr.GetString("title");
                        boxElem.Attributes.Append(boxId);
                        boxElem.Attributes.Append(boxTitle);

                        XmlNode desc = doc.CreateElement("Description");
                        desc.InnerText = rdr.GetString("description");
                        boxElem.AppendChild(desc);

                        XmlNode contents = doc.CreateElement("Contents");
                        contents.InnerText = rdr.GetString("contents");
                        boxElem.AppendChild(contents);

                        XmlNode price = doc.CreateElement("Price");
                        XmlAttribute firstInGold = doc.CreateAttribute("firstInGold");
                        firstInGold.Value = rdr.GetInt32("priceFirstInGold").ToString();
                        XmlAttribute firstInToken = doc.CreateAttribute("firstInToken");
                        firstInToken.Value = rdr.GetInt32("priceFirstInToken").ToString();
                        XmlAttribute secondInGold = doc.CreateAttribute("secondInGold");
                        secondInGold.Value = rdr.GetInt32("priceSecondInGold").ToString();
                        price.Attributes.Append(firstInGold);
                        price.Attributes.Append(firstInToken);
                        price.Attributes.Append(secondInGold);
                        boxElem.AppendChild(price);

                        XmlNode image = doc.CreateElement("Image");
                        image.InnerText = rdr.GetString("image");
                        boxElem.AppendChild(image);

                        XmlNode icon = doc.CreateElement("Icon");
                        icon.InnerText = rdr.GetString("icon");
                        boxElem.AppendChild(icon);

                        XmlNode startTime = doc.CreateElement("StartTime");
                        startTime.InnerText = rdr.GetDateTime("startTime").ToString("yyyy-MM-dd HH:mm:ss");
                        boxElem.AppendChild(startTime);

                        XmlNode endTime = doc.CreateElement("EndTime");
                        endTime.InnerText = rdr.GetDateTime("endTime").ToString("yyyy-MM-dd HH:mm:ss");
                        boxElem.AppendChild(endTime);
                        minigames.AppendChild(boxElem);
                    }
                }
            }
            StringWriter wtr = new StringWriter();
            doc.Save(wtr);
            return wtr.ToString();
        }
    }

    internal class Price
    {
        internal int Amount { get; set; }
        internal int Currency { get; set; }
    }

    internal class Sale
    {
        internal int Price { get; set; }
        internal int Currency { get; set; }
        internal DateTime SaleEnd { get; set; }
    }
}
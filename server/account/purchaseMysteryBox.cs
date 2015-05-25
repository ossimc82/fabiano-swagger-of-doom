#region

using System;
using System.IO;
using System.Linq;
using db;
using MySql.Data.MySqlClient;
using server.mysterybox;
using System.Xml;

#endregion

namespace server.account
{
    internal class purchaseMysteryBox : RequestHandler
    {
        //Thanks to Liinkii for purchasing me a MysteryBox
        //<Success><Awards>ITEM ID</Awards><Gold>GOLD LEFT</Gold></Success>
        private Random rand;

        protected override void HandleRequest()
        {
            rand = Query["ignore"] != null ? new Random(int.Parse(Query["ignore"])) : new Random();

            using (Database db = new Database())
            {
                Account acc = db.Verify(Query["guid"], Query["password"], Program.GameData);
                if (CheckAccount(acc, db, false))
                {
                    if (Query["boxId"] == null)
                    {
                        using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                            wtr.WriteLine("<Error>Box not found</Error>");
                        return;
                    }
                    MysteryBox box = MysteryBox.GetBox(int.Parse(Query["boxId"]));
                    if (box == null)
                    {
                        using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                            wtr.WriteLine("<Error>Box not found</Error>");
                        return;
                    }
                    if (box.Sale != null && DateTime.UtcNow <= box.Sale.SaleEnd)
                    {
                        switch (box.Sale.Currency)
                        {
                            case 0:
                                if (acc.Credits < box.Sale.Price)
                                {
                                    using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                                        wtr.WriteLine("<Error>Not Enough Gold</Error>");
                                    return;
                                }
                                break;
                            case 1:
                                if (acc.Stats.Fame < box.Sale.Price)
                                {
                                    using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                                        wtr.WriteLine("<Error>Not Enough Fame</Error>");
                                    return;
                                }
                                break;
                        }
                    }
                    else
                    {
                        switch (box.Price.Currency)
                        {
                            case 0:
                                if (acc.Credits < box.Price.Amount)
                                {
                                    using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                                        wtr.WriteLine("<Error>Not Enough Gold</Error>");
                                    return;
                                }
                                break;
                            case 1:
                                if (acc.Stats.Fame < box.Price.Amount)
                                {
                                    using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                                        wtr.WriteLine("<Error>Not Enough Fame</Error>");
                                    return;
                                }
                                break;
                        }
                    }

                    MysteryBoxResult res = new MysteryBoxResult
                    {
                        Awards = Utils.GetCommaSepString(GetAwards(box.Contents))
                    };
                    if (box.Sale != null && DateTime.UtcNow <= box.Sale.SaleEnd)
                        res.GoldLeft = box.Sale.Currency == 0
                            ? db.UpdateCredit(acc, -box.Sale.Price)
                            : db.UpdateFame(acc, -box.Sale.Price);
                    else
                        res.GoldLeft = box.Price.Currency == 0
                            ? db.UpdateCredit(acc, -box.Price.Amount)
                            : db.UpdateFame(acc, -box.Price.Amount);

                    if (box.Sale != null && DateTime.UtcNow <= box.Sale.SaleEnd)
                        res.Currency = box.Sale.Currency;
                    else
                        res.Currency = box.Price.Currency;

                    sendMysteryBoxResult(Context.Response.OutputStream, res);

                    int[] gifts = Utils.FromCommaSepString32(res.Awards);
                    foreach (int item in gifts)
                        acc.Gifts.Add(item);

                    MySqlCommand cmd = db.CreateQuery();
                    cmd.CommandText =
                        "UPDATE accounts SET gifts=@gifts WHERE uuid=@uuid AND password=SHA1(@password);";
                    cmd.Parameters.AddWithValue("@uuid", Query["guid"]);
                    cmd.Parameters.AddWithValue("@password", Query["password"]);
                    cmd.Parameters.AddWithValue("@gifts", Utils.GetCommaSepString(acc.Gifts.ToArray()));
                    cmd.ExecuteNonQuery();
                }
                else
                    using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                        wtr.WriteLine("<Error>Account not found</Error>");
            }
        }

        private int[] GetAwards(string items)
        {
            int[] ret = new int[items.Split(';').Length];
            for (int i = 0; i < ret.Length; i++)
                ret[i] = Utils.FromString(items.Split(';')[0].Split(',')[rand.Next(items.Split(';')[0].Split(',').Length)]);
            return ret.ToArray();
        }

        private void sendMysteryBoxResult(Stream stream, MysteryBoxResult res)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode success = doc.CreateElement("Success");
            doc.AppendChild(success);
            
            XmlNode awards = doc.CreateElement("Awards");
            awards.InnerText = res.Awards.Replace(" ", String.Empty);
            success.AppendChild(awards);


            XmlNode goldLeft = doc.CreateElement(res.Currency == 0 ? "Gold" : "Fame");
            goldLeft.InnerText = res.GoldLeft.ToString();
            success.AppendChild(goldLeft);

            StringWriter wtr = new StringWriter();
            doc.Save(wtr);
            using (StreamWriter output = new StreamWriter(stream))
                output.WriteLine(wtr.ToString());
        }

        private class MysteryBoxResult
        {
            public string Awards { get; set; }
            public int GoldLeft { get; set; }
            public int Currency { get; set; }
        }
    }
}
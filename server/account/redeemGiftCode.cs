using db;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Web;

namespace server.account
{
    internal class redeemGiftCode : RequestHandler
    {
        protected override void HandleRequest()
        {
            //#Giftcode content format
            //#gold:amount
            //#fame:amount
            //#items:itemId's:amount
            //#charSlot:amount
            //#vaultChest:amount

            using (Database db = new Database())
            {
                Account acc = db.Verify(Query["guid"], Query["password"], Program.GameData);

                if (CheckAccount(acc, db))
                {
                    string contents = null;
                    var cmd = db.CreateQuery();
                    cmd.CommandText = "SELECT * FROM giftCodes WHERE code=@code";
                    cmd.Parameters.AddWithValue("@code", Query["code"]);

                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (!rdr.HasRows)
                        {
                            Context.Response.Redirect("../InvalidGiftCode.html");
                            return;
                        }

                        while(rdr.Read())
                        {
                            contents = rdr.GetString("content");
                        }
                    }

                    if (ParseContents(Query, new StringReader(contents)))
                    {
                        Context.Response.Redirect("../GiftCodeSuccess.html");
                        cmd = db.CreateQuery();
                        cmd.CommandText = "DELETE FROM giftCodes WHERE code=@code";
                        cmd.Parameters.AddWithValue("@code", Query["code"]);
                        cmd.ExecuteNonQuery();
                    }
                    else
                        Context.Response.Redirect("../InvalidGiftCode.html");
                }
            }
        }

        private bool ParseContents(NameValueCollection query, StringReader rdr)
        {
            try
            {
                using (Database db = new Database())
                {
                    List<string> tokens = new List<string>();

                    while (true)
                    {
                        string s = rdr.ReadLine();
                        if (s.IsNullOrWhiteSpace()) break;
                        if (s.StartsWith("#")) continue;
                        tokens.Add(s.Trim());
                    }

                    string[] headers = new string[tokens.Count];

                    for (int i = 0; i < tokens.Count; i++)
                    {
                        if (tokens.Count > 0)
                            headers[i] = tokens[i].Split(':')[0];
                    }
                    Account acc = db.Verify(query["guid"], query["password"], Program.GameData);

                    if (acc != null)
                    {
                        var cmd = db.CreateQuery();

                        for (int i = 0; i < headers.Length; i++)
                        {
                            if (headers[i].StartsWith("items"))
                            {
                                Dictionary<string, int> itemDic = new Dictionary<string, int>();
                                List<int> gifts = acc.Gifts;

                                if (tokens[i].Split(':').Length == 3)
                                    for (int j = 0; j < tokens[i].Split(':')[1].Split(',').Length; j++)
                                        itemDic.Add(tokens[i].Split(':')[1].Split(',')[j],
                                            int.Parse(tokens[i].Split(':')[2].Split(',')[j]));
                                else if (tokens[i].Split(':').Length == 2)
                                    gifts.AddRange(Utils.FromCommaSepString32(tokens[i].Split(':')[1]));
                                else
                                    throw new Exception("Invalid giftCode data.");

                                foreach (KeyValuePair<string, int> item in itemDic)
                                    for (int j = 0; j < item.Value; j++)
                                        gifts.Add(Utils.FromString(item.Key));

                                cmd = db.CreateQuery();
                                cmd.CommandText =
                                    "UPDATE accounts SET gifts=@gifts WHERE uuid=@uuid AND password=SHA1(@password);";
                                cmd.Parameters.AddWithValue("@gifts", Utils.GetCommaSepString<int>(gifts.ToArray()));
                                cmd.Parameters.AddWithValue("@uuid", query["guid"]);
                                cmd.Parameters.AddWithValue("@password", query["password"]);
                                cmd.ExecuteNonQuery();
                            }

                            if (headers[i].StartsWith("charSlot"))
                            {
                                int amount = int.Parse(tokens[i].Split(':')[1]);

                                cmd = db.CreateQuery();
                                cmd.CommandText =
                                    "UPDATE accounts SET maxCharSlot=maxCharSlot + @amount WHERE uuid=@uuid AND password=SHA1(@password);";
                                cmd.Parameters.AddWithValue("@amount", amount);
                                cmd.Parameters.AddWithValue("@uuid", query["guid"]);
                                cmd.Parameters.AddWithValue("@password", query["password"]);
                                cmd.ExecuteNonQuery();
                            }

                            if (headers[i].StartsWith("vaultChest"))
                            {
                                int length = int.Parse(tokens[i].Split(':')[1]);
                                for (int j = 0; j < length; j++)
                                    db.CreateChest(acc);
                            }

                            if (headers[i].StartsWith("gold"))
                                db.UpdateCredit(acc, int.Parse(tokens[i].Split(':')[1]));

                            if (headers[i].StartsWith("fame"))
                                db.UpdateFame(acc, int.Parse(tokens[i].Split(':')[1]));
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}

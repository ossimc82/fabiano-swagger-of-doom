using db;
using db.JsonObjects;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace server.account
{
    internal class redeemGiftCode : RequestHandler
    {
        protected override void HandleRequest()
        {
            using (Database db = new Database())
            {
                string code = Query["code"];
                Account acc = Query["data"] != null ? AccountDataHelper.GetAccountGiftCodeData(HttpUtility.UrlDecode(Query["data"], Encoding.UTF8)).GetGiftCode(out code).GetAccount(Program.GameData) : db.Verify(Query["guid"], Query["password"], Program.GameData);

                if (CheckAccount(acc, db, false))
                {
                    string contents = String.Empty;
                    var cmd = db.CreateQuery();
                    cmd.CommandText = "SELECT * FROM giftCodes WHERE code=@code";
                    cmd.Parameters.AddWithValue("@code", code);

                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (!rdr.HasRows)
                        {
                            Context.Response.Redirect("../InvalidGiftCode.html");
                            return;
                        }

                        while(rdr.Read())
                            contents = rdr.GetString("content");
                    }

                    if (ParseContents(acc, contents))
                    {
                        Context.Response.Redirect("../GiftCodeSuccess.html");
                        cmd = db.CreateQuery();
                        cmd.CommandText = "DELETE FROM giftCodes WHERE code=@code";
                        cmd.Parameters.AddWithValue("@code", code);
                        cmd.ExecuteNonQuery();
                    }
                    else
                        Context.Response.Redirect("../InvalidGiftCode.html");
                }
            }
        }

        private bool ParseContents(Account acc, string json)
        {
            try
            {
                using (var db = new Database())
                {
                    var code = GiftCode.FromJson(json);
                    if (code == null) return false;
                    var cmd = db.CreateQuery();

                    if (code.Gifts.Count > 0)
                    {
                        List<int> gifts = acc.Gifts;
                        foreach (var i in code.Gifts)
                            gifts.Add(i);

                        cmd = db.CreateQuery();
                        cmd.CommandText =
                            "UPDATE accounts SET gifts=@gifts WHERE uuid=@uuid AND password=SHA1(@password);";
                        cmd.Parameters.AddWithValue("@gifts", Utils.GetCommaSepString<int>(gifts.ToArray()));
                        cmd.Parameters.AddWithValue("@uuid", Query["guid"]);
                        cmd.Parameters.AddWithValue("@password", Query["password"]);
                        cmd.ExecuteNonQuery();
                    }

                    if (code.CharSlots > 0)
                    {
                        cmd = db.CreateQuery();
                        cmd.CommandText =
                            "UPDATE accounts SET maxCharSlot=maxCharSlot + @amount WHERE uuid=@uuid AND password=SHA1(@password);";
                        cmd.Parameters.AddWithValue("@amount", code.CharSlots);
                        cmd.Parameters.AddWithValue("@uuid", Query["guid"]);
                        cmd.Parameters.AddWithValue("@password", Query["password"]);
                        cmd.ExecuteNonQuery();
                    }

                    if (code.VaultChests > 0)
                        for (int j = 0; j < code.VaultChests; j++)
                            db.CreateChest(acc);

                    if (code.Gold > 0)
                        db.UpdateCredit(acc, code.Gold);

                    if (code.Fame > 0)
                        db.UpdateFame(acc, code.Fame);
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

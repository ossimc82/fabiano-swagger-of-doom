#region

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Web;
using db;
using MySql.Data.MySqlClient;

#endregion

namespace server.account
{
    internal class purchaseSkin : RequestHandler
    {
        private List<ItemCostItem> Prices
        {
            get
            {
                return new List<ItemCostItem>
                {
                    new ItemCostItem {Type = "900", Puchasable = 0, Expires = 0, Price = 90000},
                    new ItemCostItem {Type = "902", Puchasable = 0, Expires = 0, Price = 90000},
                    new ItemCostItem {Type = "834", Puchasable = 1, Expires = 0, Price = 900},
                    new ItemCostItem {Type = "835", Puchasable = 1, Expires = 0, Price = 600},
                    new ItemCostItem {Type = "836", Puchasable = 1, Expires = 0, Price = 900},
                    new ItemCostItem {Type = "837", Puchasable = 1, Expires = 0, Price = 600},
                    new ItemCostItem {Type = "838", Puchasable = 1, Expires = 0, Price = 900},
                    new ItemCostItem {Type = "839", Puchasable = 1, Expires = 0, Price = 900},
                    new ItemCostItem {Type = "840", Puchasable = 1, Expires = 0, Price = 900},
                    new ItemCostItem {Type = "841", Puchasable = 1, Expires = 0, Price = 900},
                    new ItemCostItem {Type = "842", Puchasable = 1, Expires = 0, Price = 900},
                    new ItemCostItem {Type = "843", Puchasable = 1, Expires = 0, Price = 900},
                    new ItemCostItem {Type = "844", Puchasable = 1, Expires = 0, Price = 900},
                    new ItemCostItem {Type = "845", Puchasable = 1, Expires = 0, Price = 900},
                    new ItemCostItem {Type = "846", Puchasable = 1, Expires = 0, Price = 900},
                    new ItemCostItem {Type = "847", Puchasable = 1, Expires = 0, Price = 900},
                    new ItemCostItem {Type = "848", Puchasable = 1, Expires = 0, Price = 900},
                    new ItemCostItem {Type = "849", Puchasable = 1, Expires = 0, Price = 900},
                    new ItemCostItem {Type = "850", Puchasable = 0, Expires = 1, Price = 900},
                    new ItemCostItem {Type = "851", Puchasable = 0, Expires = 1, Price = 900},
                    new ItemCostItem {Type = "852", Puchasable = 1, Expires = 0, Price = 900},
                    new ItemCostItem {Type = "853", Puchasable = 1, Expires = 0, Price = 900},
                    new ItemCostItem {Type = "854", Puchasable = 1, Expires = 0, Price = 900},
                    new ItemCostItem {Type = "855", Puchasable = 1, Expires = 0, Price = 900},
                    new ItemCostItem {Type = "856", Puchasable = 0, Expires = 0, Price = 90000},
                    new ItemCostItem {Type = "883", Puchasable = 0, Expires = 0, Price = 90000}
                };
            }
        }

        protected override void HandleRequest()
        {
            StreamWriter wtr = new StreamWriter(Context.Response.OutputStream);
            if (Query.AllKeys.Length > 0)
            {
                using (Database db = new Database())
                {
                    Account acc = db.Verify(Query["guid"], Query["password"], Program.GameData);

                    if (CheckAccount(acc, db, false))
                    {
                        foreach (ItemCostItem item in Prices)
                        {
                            if (item.Type == Query["skinType"] && item.Puchasable == 1)
                            {
                                if (!acc.OwnedSkins.Contains(int.Parse(Query["skinType"])))
                                {
                                    acc.OwnedSkins.Add(int.Parse(Query["skinType"]));
                                    db.UpdateCredit(acc, -item.Price);
                                    MySqlCommand cmd = db.CreateQuery();
                                    cmd.CommandText =
                                        "UPDATE accounts SET ownedSkins=@ownedSkins WHERE uuid=@uuid AND password=SHA1(@password)";
                                    cmd.Parameters.AddWithValue("@ownedSkins",
                                        Utils.GetCommaSepString(acc.OwnedSkins.ToArray()));
                                    cmd.Parameters.AddWithValue("@uuid", Query["guid"]);
                                    cmd.Parameters.AddWithValue("@password", Query["password"]);
                                    if (cmd.ExecuteNonQuery() == 0)
                                        wtr.WriteLine("<Error>Unable to purchase</Error>");
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
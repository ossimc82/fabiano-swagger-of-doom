#region

using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Web;
using db;
using MySql.Data.MySqlClient;

#endregion

namespace server.@char
{
    internal class purchaseClassUnlock : RequestHandler
    {
        protected override void HandleRequest()
        {
            using (Database db = new Database())
            {
                try
                {
                    Account acc = db.Verify(Query["guid"], Query["password"], Program.GameData);

                    string classType = Program.GameData.ObjectTypeToId[ushort.Parse(Query["classType"])];

                    if (CheckAccount(acc, db))
                    {
                        int price = Program.GameData.ObjectDescs[ushort.Parse(Query["classType"])].UnlockCost;
                        if (acc.Credits < price) return;
                        db.UpdateCredit(acc, -price);
                        MySqlCommand cmd = db.CreateQuery();
                        cmd.CommandText =
                            "UPDATE unlockedclasses SET available='unrestricted' WHERE accId=@accId AND class=@class;";
                        cmd.Parameters.AddWithValue("@accId", acc.AccountId);
                        cmd.Parameters.AddWithValue("@class", classType);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                    {
                        wtr.WriteLine("<Error>Invalid classType");
                        wtr.Flush();
                        wtr.WriteLine(e);
                    }
                }
            }
        }
    }
}
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

namespace server.account
{
    internal class playFortuneGame : RequestHandler
    {
        private static Dictionary<string, int[]> CurrentGames = new Dictionary<string, int[]>();

        private const int GOLD = 0;
        private const int FORTUNETOKENS = 2;

        protected override void HandleRequest()
        {
            using (var db = new Database())
            {
                int currency = -1;
                int price = -1;
                int.TryParse(Query["currency"], out currency);
                string status = "<Error>Internal Server Error</Error>";
                Account acc;

                if (CheckAccount(acc = db.Verify(Query["guid"], Query["password"], Program.GameData), db, false))
                {
                    var cmd = db.CreateQuery();
                    cmd.CommandText = "SELECT * FROM thealchemist WHERE startTime <= now() AND endTime >= now() AND id=@gameId;";
                    cmd.Parameters.AddWithValue("@gameId", Query["gameId"]);
                    Random rand = new Random();
                    List<int> gifts = new List<int>();

                    using (var rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();
                        if (rdr.HasRows)
                        {
                            List<int> items = Utils.FromCommaSepString32(rdr.GetString("contents")).ToList();
                            List<int> candidates = new List<int>(3);

                            do
                            {
                                int item = items[rand.Next(items.Count)];
                                if (!candidates.Contains(item))
                                    candidates.Add(item);
                            } while (candidates.Count < 3);

                            if (currency == GOLD)
                            {
                                if (Query["status"] == "0")
                                {
                                    if (CurrentGames.ContainsKey(acc.AccountId))
                                        CurrentGames.Remove(acc.AccountId);
                                    CurrentGames.Add(acc.AccountId, candidates.ToArray());
                                    price = rdr.GetInt32("priceFirstInGold");
                                    status = "<Success><Candidates>" +
                                        Utils.GetCommaSepString(candidates.ToArray()) +
                                        "</Candidates><Gold>" +
                                        (acc.Credits - price) +
                                        "</Gold></Success>";
                                }
                                else if (Query["status"] == "1")
                                {
                                    if (CurrentGames.ContainsKey(acc.AccountId))
                                    {
                                        candidates = CurrentGames[acc.AccountId].ToList();
                                        candidates.Shuffle();
                                        status = "<Success><Awards>" + candidates[int.Parse(Query["choice"])] + "</Awards></Success>";
                                        gifts.Add(candidates[int.Parse(Query["choice"])]);
                                        candidates.Remove(candidates[int.Parse(Query["choice"])]);
                                        CurrentGames[acc.AccountId] = candidates.ToArray();
                                    }
                                }
                                else if (Query["status"] == "2")
                                {
                                    if (CurrentGames.ContainsKey(acc.AccountId))
                                    {
                                        candidates = CurrentGames[acc.AccountId].ToList();
                                        candidates.Shuffle();
                                        price = rdr.GetInt32("priceSecondInGold");
                                        status = "<Success><Awards>" + candidates[int.Parse(Query["choice"])] + "</Awards></Success>";
                                        gifts.Add(candidates[int.Parse(Query["choice"])]);
                                        CurrentGames.Remove(acc.AccountId);
                                    }
                                }
                            }
                            else if (currency == FORTUNETOKENS)
                            {
                                if (Query["status"] == "0")
                                {
                                    if (CurrentGames.ContainsKey(acc.AccountId))
                                        CurrentGames.Remove(acc.AccountId);
                                    CurrentGames.Add(acc.AccountId, candidates.ToArray());
                                    price = rdr.GetInt32("priceFirstInToken");
                                    status = "<Success><Candidates>" +
                                        Utils.GetCommaSepString(candidates.ToArray()) +
                                        "</Candidates><FortuneToken>" +
                                        (acc.FortuneTokens - price) +
                                        "</FortuneToken></Success>";
                                }
                                else if (Query["status"] == "1")
                                {
                                    if (CurrentGames.ContainsKey(acc.AccountId))
                                    {
                                        candidates = CurrentGames[acc.AccountId].ToList();
                                        candidates.Shuffle();
                                        status = "<Success><Awards>" + candidates[int.Parse(Query["choice"])] + "</Awards></Success>";
                                        gifts.Add(candidates[int.Parse(Query["choice"])]);
                                        candidates.Remove(candidates[int.Parse(Query["choice"])]);
                                        CurrentGames[acc.AccountId] = candidates.ToArray();
                                    }
                                }
                                else if (Query["status"] == "2")
                                {
                                    status = "<Error>You can not play twiche with a Fortune Token</Error>";
                                }
                            }
                            else
                                status = "<Error>Invalid currency</Error>";
                        }
                        else
                            status = "<Error>Invalid gameId</Error>";
                    }
                    if (currency == GOLD)
                        db.UpdateCredit(acc, price == -1 ? 0 : -price);
                    else if (currency == FORTUNETOKENS)
                        db.UpdateFortuneToken(acc, price == -1 ? 0 : -price);

                    db.AddGifts(acc, gifts);
                }
                else
                    status = "<Error>Account not found</Error>";

                using (StreamWriter wtr = new StreamWriter(Context.Response.OutputStream))
                    wtr.Write(status);
            }
        }
    }
}

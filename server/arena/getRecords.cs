#region

using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using db;
using MySql.Data.MySqlClient;

#endregion

namespace server.Arena
{
    internal class getRecords : RequestHandler
    {
        protected override void HandleRequest()
        {
            string result = "";
            using (Database dbx = new Database())
            {
                Account acc = dbx.Verify(Query["guid"], Query["password"], Program.GameData);
                if (String.IsNullOrEmpty(Query["guid"]) ||
                    String.IsNullOrEmpty(Query["password"]) ||
                    String.IsNullOrEmpty(Query["type"]) ||
                    acc == null)
                {
                    Context.Response.StatusCode = 400;
                    result = "<Error>Invalid GUID/password combination</Error>";
                }
                else
                {
                    string[][] ranks = dbx.GetArenaLeaderboards(Query["type"], acc);
                    result += "<ArenaRecords>";
                    foreach (string[] i in ranks)
                    {
                        MySqlCommand cmd = dbx.CreateQuery();
                        cmd.CommandText =
                            "SELECT 'skin','tex1','tex2','inventory','class' FROM characters WHERE charid = @charid";
                        cmd.Parameters.AddWithValue("@charid", i[2]);
                        string skin, tex1, tex2, inventory, cclass;
                        skin = tex1 = tex2 = inventory = cclass = null;
                        using (MySqlDataReader drdr = cmd.ExecuteReader())
                        {
                            while (drdr.Read())
                            {
                                skin = drdr.GetString("skin");
                                tex1 = drdr.GetString("tex1");
                                tex2 = drdr.GetString("tex2");
                                inventory = drdr.GetString("inventory");
                                cclass = drdr.GetString("class");
                            }
                        }
                        cmd = dbx.CreateQuery();
                        cmd.CommandText =
                            "SELECT 'skin','tex1','tex2','inventory','class' FROM characters WHERE charid = @charid";
                        cmd.Parameters.AddWithValue("@charid", i[2]);
                        using (MySqlDataReader drdr = cmd.ExecuteReader())
                        {
                            while (drdr.Read())
                            {
                                skin = drdr.GetString("skin");
                                tex1 = drdr.GetString("tex1");
                                tex2 = drdr.GetString("tex2");
                                inventory = drdr.GetString("inventory");
                                cclass = drdr.GetString("class");
                            }
                        }
                        result += "<Record>";
                        //wave number
                        result += "<WaveNumber>" + i[0] + "</WaveNumber>";
                        //playtime
                        result += "<Time>" + i[4] + "</Time>";
                        result += "<PlayData>";
                        result += "<CharacterData>";
                        result += "<GuildName>" + acc.Guild.Name + "</GuildName>";
                        result += "<GuildRank>" + acc.Guild.Rank + "</GuildRank>";
                        result += "<Id>" + i[2] + "</Id>";
                        result += "<Texture>" + skin + "</Texture>";
                        result += "<Invantory>" + inventory + "</Inventory>";
                        result += "<Name>" + acc.Name + "</Name>";
                        result += "<Class>" + cclass + "</Class>";
                        result += "</CharacterData>";
                        result += "<Pet name=\"" +
                                  "\" type=\"" +
                                  "\" instanceId=\"" +
                                  "\" rarity=\"" +
                                  "\" maxAbilityPower=\"" +
                                  "\" skin=\"" +
                                  "\" family=\"" +
                                  "\">";
                        result += "<Abilities>";
                        result += "</Abilities>";
                        result += "</Pet>";
                        result += "</PlayData>";
                        result += "</Record>";
                    }
                    result += "</ArenaRecords";
                }
            }
            byte[] buf = Encoding.UTF8.GetBytes(result);
            Context.Response.OutputStream.Write(buf, 0, buf.Length);
        }
    }
}
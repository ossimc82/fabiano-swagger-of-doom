#region

using db;
using System;
using wServer.networking.svrPackets;
using wServer.realm.entities.player;

#endregion

namespace wServer.realm.entities
{
    public class GuildMerchant : SellableObject
    {
        public const int UP1 = 0x0736;
        public const int UP1C = 10000;
        public const int UP2 = 0x0737;
        public const int UP2C = 100000;
        public const int UP3 = 0x0738;
        public const int UP3C = 250000;

        public GuildMerchant(RealmManager manager, ushort objType)
            : base(manager, objType)
        {
            RankReq = 0;
            Currency = CurrencyType.GuildFame;
            switch (objType)
            {
                case UP1:
                    Price = UP1C;
                    break;
                case UP2:
                    Price = UP2C;
                    break;
                case UP3:
                    Price = UP3C;
                    break;
            }
        }

        public override void Buy(Player player)
        {
            if (!player.Guild.IsDefault)
            {
                if (player.Guild[player.AccountId].Rank >= 30)
                {
                    using (var db = new Database())
                    {
                        if (db.GetGuild(db.GetGuildId(player.Guild[player.AccountId].Name)).GuildFame >= Price)
                        {
                            var cmd = db.CreateQuery();
                            cmd.CommandText = "UPDATE guilds SET level=level+1, guildFame=guildFame-@price WHERE name=@guildName";
                            cmd.Parameters.AddWithValue("@guildName", player.Guild.Name);
                            cmd.Parameters.AddWithValue("@price", Price);
                            if (cmd.ExecuteNonQuery() == 1)
                            {
                                player.Client.SendPacket(new BuyResultPacket
                                {
                                    Message = "{\"key\":\"server.sale_succeeds\"}",
                                    Result = -1
                                });
                                player.SendInfo("Please leave the Guild Hall, we need some minutes to update the Guild Hall.");
                                player.Guild.UpdateGuildHall();
                            }
                        }
                        else
                        {
                            player.SendHelp("FUCK");
                            player.Client.SendPacket(new BuyResultPacket
                            {
                                Message = "{\"key\":\"server.not_enough_fame\"}",
                                Result = 9
                            });
                        }
                    }
                }
                else
                {
                    player.Client.SendPacket(new BuyResultPacket
                    {
                        Message = "Founder or Leader rank required.",
                        Result = 0
                    });
                }
            }
        }
    }
}
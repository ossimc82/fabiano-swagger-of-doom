#region

using System;
using System.Linq;
using db;
using MySql.Data.MySqlClient;
using wServer.networking.cliPackets;
using wServer.networking.svrPackets;
using System.IO;

#endregion

namespace wServer.networking.handlers
{
    internal class ChooseNameHandler : PacketHandlerBase<ChooseNamePacket>
    {
        public override PacketID ID
        {
            get { return PacketID.CHOOSENAME; }
        }

        protected override void HandlePacket(Client client, ChooseNamePacket packet)
        {
            string[] bannednames = { "Dick", "Fuck", "Pussy", "Cunt", "Bitch", "Nigger", "Nigga", "Niqqa", "Cunt", "Shit", "Penis", "Vagina", "Chent", "Niqqer", "Negro", "Ngr", "Chink", "Fag", "Faggot", "Fgt", "Fagot", "Fagit", "Ass", "Autistic", "Autism", "Schlong", "vag", "damn", "tits", "tlts", "retard", "asd", "Kalle", "Kaile", "Kalie", "xDalla", "xDalia", "xDaila", "McFarvo", "Pixl", "TheHangman", "White", "DrMini", "TEEBQNE", "TBQNEE", "FloFlorian", "Lore", "Dalla", "Daila", "Dalia", "Clocking", "Ciocking", "IArkani", "lArkani", "BunnyBomb", "Liinkii", "Gamingland", "GamingIand", "TheRegal", "TheRegaI", "ParagonX", "Cantplay", "Billyhendr", "Nilly", "Trapped", "Botmaker", "JustANoob", "JustANoobROTMG", "Niiiy", "niily", "niliy", "Lucifer", "Kithio", "Case", "Travoos", "XD", "DX", "Trol", "Troll", "lol", "lel", "OMG", "suck" };

            foreach (string i in bannednames)
            {
                if (i.ToLower().Equals(packet.Name) || packet.Name.ToLower().Contains(i.ToLower()))
                {
                    client.SendPacket(new NameResultPacket
                    {
                        Success = false,
                        ErrorText = "Error.nameAlreadyInUse"
                    });
                    return;
                }
            }
                
            using (Database db = new Database())
            {
                MySqlCommand cmdx = db.CreateQuery();
                cmdx.CommandText = "SELECT namechosen FROM accounts WHERE id=@accId";
                cmdx.Parameters.AddWithValue("@accId", client.Account.AccountId);
                object execx = cmdx.ExecuteScalar();
                bool namechosen = bool.Parse(execx.ToString());
                if (String.IsNullOrEmpty(packet.Name) || packet.Name.Length > 10)
                {
                    client.SendPacket(new NameResultPacket
                    {
                        Success = false,
                        ErrorText = "Error.nameIsNotAlpha"
                    });
                }
                else
                {
                    MySqlCommand cmd = db.CreateQuery();
                    cmd.CommandText = "SELECT COUNT(name) FROM accounts WHERE name=@name;";
                    cmd.Parameters.AddWithValue("@name", packet.Name);
                    object x = cmd.ExecuteScalar();
                    if (int.Parse(x.ToString()) > 0)
                    {
                        client.SendPacket(new NameResultPacket
                        {
                            Success = false,
                            ErrorText = "Error.nameAlreadyInUse"
                        });
                    }
                    else
                    {
                        db.ReadStats(client.Account);
                        if (client.Account.Credits < 1000 && namechosen)
                            client.SendPacket(new NameResultPacket
                            {
                                Success = false,
                                ErrorText = "server.not_enough_gold"
                            });
                        else
                        {
                            if (client.Account.NameChosen == false)
                            {
                                cmd = db.CreateQuery();
                                cmd.CommandText = "UPDATE accounts SET name=@name, namechosen=TRUE WHERE id=@accId;";
                                cmd.Parameters.AddWithValue("@accId", client.Account.AccountId);
                                cmd.Parameters.AddWithValue("@name", packet.Name);
                                if (cmd.ExecuteNonQuery() > 0)
                                {
                                    client.Player.Name = packet.Name;
                                    client.Player.NameChosen = true;
                                    client.Player.UpdateCount++;
                                    client.SendPacket(new NameResultPacket
                                    {
                                        Success = true,
                                        ErrorText = "server.buy_success"
                                    });
                                }
                                else
                                {
                                    client.SendPacket(new NameResultPacket
                                    {
                                        Success = false,
                                        ErrorText = "GuildChronicle.left"
                                    });
                                }
                            }
                            else
                            {
                                cmd = db.CreateQuery();
                                cmd.CommandText = "UPDATE accounts SET name=@name, namechosen=TRUE WHERE id=@accId;";
                                cmd.Parameters.AddWithValue("@accId", client.Account.AccountId);
                                cmd.Parameters.AddWithValue("@name", packet.Name);
                                if (cmd.ExecuteNonQuery() > 0)
                                {
                                    client.Player.Credits = db.UpdateCredit(client.Account, -1000);
                                    client.Player.Name = packet.Name;
                                    client.Player.NameChosen = true;
                                    client.Player.UpdateCount++;
                                    client.SendPacket(new NameResultPacket
                                    {
                                        Success = true,
                                        ErrorText = "server.buy_success"
                                    });
                                }
                                else
                                {
                                    client.SendPacket(new NameResultPacket
                                    {
                                        Success = false,
                                        ErrorText = "GuildChronicle.left"
                                    });
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
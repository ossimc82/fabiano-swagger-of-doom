#region

using System.Collections.Generic;
using wServer.networking.svrPackets;

#endregion

namespace wServer.realm.entities.player
{
    partial class Player
    {
        public string GetLanguageString(string key, params KeyValuePair<string, object>[] tokens)
        {
            var ret = "{\"key\":\"" + key + "\"";
            if (tokens != null)
            {
                ret += ",\"tokens\":{";
                for (var i = 0; i < tokens.Length; i++)
                {
                    ret += "\"" + tokens[i].Key + "\":\"" + tokens[i].Value + "\"";
                    if (i + 1 != tokens.Length)
                        ret += ",";
                }
                ret += "}";
            }
            ret += "}";
            return ret;
        }

        public void SendInfoWithTokens(string key, params KeyValuePair<string, object>[] tokens)
        {
            var toSend = "{\"key\":\"" + key + "\"";
            if(tokens != null)
            {
                toSend += ",\"tokens\":{";
                for(var i = 0; i < tokens.Length; i++)
                {
                    toSend += "\"" + tokens[i].Key + "\":\"" + tokens[i].Value + "\"";
                    if (i + 1 != tokens.Length)
                        toSend += ",";
                }
                toSend += "}";
            }
            toSend += "}";
            SendInfo(toSend);
        }

        public void SendInfo(string text)
        {
            Client.SendPacket(new TextPacket()
            {
                BubbleTime = 0,
                Stars = -1,
                Name = "",
                Text = text
            });
        }
        public void SendError(string text)
        {
            Client.SendPacket(new TextPacket()
            {
                BubbleTime = 0,
                Stars = -1,
                Name = "*Error*",
                Text = text
            });
        }
        public void SendpsrText(string text)
        {
            Client.SendPacket(new TextPacket()
            {
                BubbleTime = 0,
                Stars = -1,
                Name = "*psr*",
                Text = text
            });
        }
        public void SendHelp(string text)
        {
            Client.SendPacket(new TextPacket()
            {
                BubbleTime = 0,
                Stars = -1,
                Name = "*Help*",
                Text = text
            });
        }
        public void SendEnemy(string name, string text)
        {
            Client.SendPacket(new TextPacket()
            {
                BubbleTime = 0,
                Stars = -1,
                Name = "#" + name,
                Text = text
            });
        }
        public void SendText(string sender, string text)
        {
            Client.SendPacket(new TextPacket()
            {
                BubbleTime = 0,
                Stars = -1,
                Name = sender,
                Text = text
            });
        }
        public void SendGuild(string text)
        {
            Client.SendPacket(new TextPacket
            {
                BubbleTime = 0,
                Stars = -1,
                Name = "",
                Recipient = "*Guild*",
                Text = text
            });
        }
        public void GuildRecieved(int objId, int stars, string from, string text)
        {
            Client.SendPacket(new TextPacket
            {
                BubbleTime = 10,
                Stars = stars,
                Name = "*Guild*",
                Recipient = from,
                Text = text
            });
        }
    }
}

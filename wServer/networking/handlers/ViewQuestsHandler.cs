using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.networking.cliPackets;
using wServer.networking.svrPackets;

namespace wServer.networking.handlers
{
    internal class ViewQuestsHandler : PacketHandlerBase<ViewQuestsPacket>
    {
        public override PacketID ID
        {
            get { return PacketID.VIEWQUESTS; }
        }

        protected override void HandlePacket(Client client, ViewQuestsPacket packet)
        {
            client.Manager.Database.DoActionAsync(db =>
            {
                client.Player.DailyQuest = db.GetDailyQuest(client.Account.AccountId, Manager.GameData);

                if (client.Player.DailyQuest.Tier == -1)
                {
                    client.SendPacket(AllQuestsCompleted());
                    client.Player.SendInfo("No available quests found.");
                    return;
                }

                client.SendPacket(new QuestFetchResponsePacket
                {
                    Description = client.Player.DailyQuest.Description,
                    Goal = client.Player.DailyQuest.Goal,
                    Tier = client.Player.DailyQuest.Tier,
                    Image = client.Player.DailyQuest.Image
                });
                //1 token image: http://rotmg.kabamcdn.com/DailyQuest1FortuneToken.png
                //2 token image: http://rotmg.kabamcdn.com/DailyQuest2FortuneToken.png
            });
        }

        public QuestFetchResponsePacket AllQuestsCompleted()
        {
            return new QuestFetchResponsePacket
            {
                Description = "",
                Goal = "",
                Tier = -1,
                Image = ""
            };
        }
    }
}

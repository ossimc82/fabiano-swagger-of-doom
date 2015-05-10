#region

using db;
using System;
using wServer.networking.cliPackets;
using wServer.networking.svrPackets;

#endregion

namespace wServer.realm.entities.player
{
    public partial class Player
    {
        private const int PING_PERIOD = 1000;
        private const int DC_THRESOLD = 15000;

        private int updateLastSeen;

        private int curClientTime;
        private int oldClientTime;
        private int first5;

        public WorldTimer PongDCTimer { get; private set; }

        private bool KeepAlive(RealmTime time)
        {
            return true;
        }

        internal void Pong(int time, PongPacket pkt)
        {
            try
            {
                if (Random.Next(1, 100000) == 1)
                    Client.GiftCodeReceived("Pong");

                updateLastSeen++;

                if (PongDCTimer != null && Owner.Timers.Contains(PongDCTimer))
                    Owner.Timers.Remove(PongDCTimer);

                //Owner.Timers.Add(PongDCTimer = new WorldTimer(DC_THRESOLD, (w, t) =>
                //{
                //    SendError("Lost connection to server.");
                //    Client.Disconnect();
                //}));

                if (updateLastSeen >= 60)
                {
                    Manager.Database.DoActionAsync(db =>
                    {
                        db.UpdateLastSeen(Client.Account.AccountId, Client.Character.CharacterId, WorldInstance.Name);
                        updateLastSeen = 0;
                    });
                }
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }



        public void ClientTick(RealmTime time, MovePacket packet)
        {
            //oldClientTime = curClientTime;
            //curClientTime = packet.Time;
            //if (first5 < 5) first5++;
            //if (oldClientTime == 0 || first5 < 5) return;

            //if (curClientTime - oldClientTime >= 500 || curClientTime - oldClientTime <= 100)
            //{
            //    Client.Disconnect();
            //    Owner.BroadcastPacket(new TextPacket
            //    {
            //        Name = "@ANNOUNCEMENT",
            //        BubbleTime = 0,
            //        CleanText = "",
            //        ObjectId = 0,
            //        Recipient = "",
            //        Stars = -1,
            //        Text = String.Format("{0} just tried to speedhack!\ntimedifference: {1}", Name, curClientTime - oldClientTime)
            //    }, null);
            //}
            //SendInfo((curClientTime - oldClientTime).ToString());
        }
    }
}
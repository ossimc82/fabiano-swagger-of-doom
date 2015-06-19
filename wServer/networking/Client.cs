#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Sockets;
using log4net;
using log4net.Core;
using wServer.networking.cliPackets;
using wServer.networking.svrPackets;
using wServer.realm;
using wServer.realm.entities.player;
using System.Threading.Tasks;
using db.JsonObjects;

#endregion

namespace wServer.networking
{
    public enum ProtocalStage
    {
        Connected,
        Handshaked,
        Ready,
        Disconnected
    }

    public class Client : IDisposable
    {
        public const string SERVER_VERSION = "27.3.2";
        private bool disposed;

        private static readonly ILog log = LogManager.GetLogger(typeof (Client));

        public uint UpdateAckCount = 0;

        private NetworkHandler handler;

        public Client(RealmManager manager, Socket skt)
        {
            Socket = skt;
            Manager = manager;
            ReceiveKey =
                new RC4(new byte[] {0x31, 0x1f, 0x80, 0x69, 0x14, 0x51, 0xc7, 0x1d, 0x09, 0xa1, 0x3a, 0x2a, 0x6e});
            SendKey = new RC4(new byte[] {0x72, 0xc5, 0x58, 0x3c, 0xaf, 0xb6, 0x81, 0x89, 0x95, 0xcd, 0xd7, 0x4b, 0x80});
            BeginProcess();
        }

        public RC4 ReceiveKey { get; private set; }
        public RC4 SendKey { get; private set; }
        public RealmManager Manager { get; private set; }

        public int Id { get; internal set; }

        public Socket Socket { get; internal set; }

        public Char Character { get; internal set; }

        public Account Account { get; internal set; }

        public ProtocalStage Stage { get; internal set; }

        public Player Player { get; internal set; }

        public wRandom Random { get; internal set; }
        public string ConnectedBuild { get; internal set; }
        public int TargetWorld { get; internal set; }

        public void BeginProcess()
        {
            log.InfoFormat($"Received client @ {Socket.RemoteEndPoint}.");
            handler = new NetworkHandler(this, Socket);
            handler.BeginHandling();
        }

        public void SendPacket(Packet pkt)
        {
            handler?.SendPacket(pkt);
        }

        public void SendPackets(IEnumerable<Packet> pkts)
        {
            handler?.SendPackets(pkts);
        }

        public bool IsReady()
        {
            if (Stage == ProtocalStage.Disconnected)
                return false;
            return Stage != ProtocalStage.Ready || (Player != null && (Player == null || Player.Owner != null));
        }

        internal void ProcessPacket(Packet pkt)
        {
            try
            {
                log.Logger.Log(typeof (Client), Level.Verbose,
                   $"Handling packet '{pkt}'...", null);
                if (pkt.ID == (PacketID) 255) return;
                IPacketHandler handler;
                if (!PacketHandlers.Handlers.TryGetValue(pkt.ID, out handler))
                    log.Warn($"Unhandled packet '{pkt.ID}'.");
                else
                    handler.Handle(this, (ClientPacket) pkt);
            }
            catch (Exception e)
            {
                log.Error($"Error when handling packet '{pkt}'...", e);
                Disconnect();
            }
        }

        public void Disconnect()
        {
            try
            {
                if (Stage == ProtocalStage.Disconnected) return;
                Stage = ProtocalStage.Disconnected;
                if (Account != null)
                    DisconnectFromRealm();

                Socket.Close();
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }

        public Task Save()
        {
            return Manager.Database.DoActionAsync(db =>
            {
                try
                {
                    string w = null;
                    if (Player != null)
                    {
                        Player.SaveToCharacter();
                        if (Player.Owner != null)
                        {
                            if (Player.Owner.Id == -6) return;
                            w = Player.Owner.Name;
                        }
                    }

                    if (Character != null)
                    {
                        if (w != null) db.UpdateLastSeen(Account.AccountId, Character.CharacterId, w);
                        db.SaveCharacter(Account, Character);
                    }

                    db.UnlockAccount(Account);
                }
                catch (Exception ex)
                {
                    log.Fatal("SaveException", ex);
                }
            });
        }

        //Following must execute, network loop will discard disconnected client, so logic loop
        private void DisconnectFromRealm()
        {
            Manager.Logic.AddPendingAction(t =>
            {
                Save();
                Manager.Disconnect(this);
            }, PendingPriority.Destruction);
        }

        public void Reconnect(ReconnectPacket pkt)
        {
            Manager.Logic.AddPendingAction(t =>
            {
                Save();
                SendPacket(pkt);
            }, PendingPriority.Destruction);
        }

        public void GiftCodeReceived(string type)
        {
            //Use later
            switch (type)
            {
                case "Pong":
                    break;
                case "LevelUp":
                    break;
            }

            AddGiftCode(GiftCode.GenerateRandom(Manager.GameData));
        }

        private void AddGiftCode(GiftCode code)
        {
            Manager.Database.DoActionAsync(db =>
            {
                var key = db.GenerateGiftcode(code.ToJson(), Account.AccountId);

                //var message = new MailMessage();
                //message.To.Add(Account.Email);
                //message.IsBodyHtml = true;
                //message.Subject = "You received a new GiftCode";
                //message.From = new MailAddress(Program.Settings.GetValue<string>("serverEmail", ""));
                //message.Body = "<center>Your giftcode is: " + code + "</br> Check the items in your giftcode <a href=\"" + Program.Settings.GetValue<string>("serverDomain", "localhost") + "/CheckGiftCode.html\" target=\"_blank\">here</a> or redeem the code <a href=\"" + Program.Settings.GetValue<string>("serverDomain", "localhost") + "/RedeemGiftCode.html\" target=\"_blank\">here</a></center>";

                //Program.SendEmail(message);
                Player.SendInfo($"You have received a new GiftCode: {key}\nRedeem it at: {Program.Settings.GetValue("serverDomain")}/GiftCode.html or\n type /giftcode to scan it with your mobile via qr code");
            });
        }

        public void Dispose()
        {
            if (disposed) return;
            handler?.Dispose();
            handler = null;
            ReceiveKey = null;
            SendKey = null;
            Manager = null;
            Socket = null;
            Character = null;
            Account = null;
            Player?.Dispose();
            Player = null;
            Random = null;
            ConnectedBuild = null;
            disposed = true;
        }
    }
}
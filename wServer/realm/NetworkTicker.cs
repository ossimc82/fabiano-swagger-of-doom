#region

using System;
using System.Collections.Concurrent;
using System.Threading;
using log4net;
using wServer.networking;

#endregion

namespace wServer.realm
{
    #region

    using Work = Tuple<Client, Packet>;

    #endregion

    public class NetworkTicker //Sync network processing
    {
        private static readonly ConcurrentQueue<Work> pendings = new ConcurrentQueue<Work>();
        private static SpinWait loopLock = new SpinWait();
        private readonly ILog log = LogManager.GetLogger(typeof (NetworkTicker));

        public NetworkTicker(RealmManager manager)
        {
            Manager = manager;
        }

        public RealmManager Manager { get; private set; }

        public void AddPendingPacket(Client parrent, Packet pkt)
        {
            pendings.Enqueue(new Work(parrent, pkt));
        }


        public void TickLoop()
        {
            log.Info("Network loop started.");
            Work work;
            while (true)
            {
                try
                {
                    if (Manager.Terminating) break;
                    loopLock.Reset();
                    while (pendings.TryDequeue(out work))
                    {
                        try
                        {
                            if (Manager.Terminating) return;
                            if (work.Item1.Stage == ProtocalStage.Disconnected)
                            {
                                Client client;
                                var accId = work.Item1?.Account?.AccountId;
                                if(accId != null)
                                Manager.Clients.TryRemove(accId, out client);
                                continue;
                            }
                            try
                            {
                                work.Item1.ProcessPacket(work.Item2);
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                        }
                    }
                    while (pendings.Count == 0 && !Manager.Terminating)
                        loopLock.SpinOnce();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            log.Info("Network loop stopped.");
        }
    }
}
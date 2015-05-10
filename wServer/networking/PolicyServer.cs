#region

using System;
using System.Net;
using System.Net.Sockets;
using log4net;

#endregion

namespace wServer.networking
{
    internal class PolicyServer
    {
        private static readonly ILog log = LogManager.GetLogger(typeof (PolicyServer));

        private readonly TcpListener listener;
        private bool started;

        public PolicyServer()
        {
            listener = new TcpListener(IPAddress.Any, 843);
        }

        private static void ServePolicyFile(IAsyncResult ar)
        {
            try
            {
                TcpClient cli = (ar.AsyncState as TcpListener).EndAcceptTcpClient(ar);
                (ar.AsyncState as TcpListener).BeginAcceptTcpClient(ServePolicyFile, ar.AsyncState);
                NetworkStream s = cli.GetStream();
                NReader rdr = new NReader(s);
                NWriter wtr = new NWriter(s);
                if (rdr.ReadNullTerminatedString() == "<policy-file-request/>")
                {
                    wtr.WriteNullTerminatedString(@"<cross-domain-policy>
     <allow-access-from domain=""*"" to-ports=""*"" />
</cross-domain-policy>");
                    wtr.Write((byte) '\r');
                    wtr.Write((byte) '\n');
                }
                cli.Close();
            }
            catch (ObjectDisposedException) { }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        public void Start()
        {
            log.Info("Starting policy server...");
            try
            {
                listener.Start();
                listener.BeginAcceptTcpClient(ServePolicyFile, listener);
                started = true;
            }
            catch (ObjectDisposedException) { }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Warn("Could not start Socket Policy Server, is port 843 occupied?");
                started = false;
            }
        }

        public void Stop()
        {
            if (started)
            {
                log.Info("Stopping policy server...");
                listener.Stop();
            }
        }
    }
}
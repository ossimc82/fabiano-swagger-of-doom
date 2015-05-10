#region

using System.IO;
using System.Net;
using System.Text;

#endregion

namespace server.app
{
    internal class init : RequestHandler
    {
        private readonly string text = File.ReadAllText("init.txt");

        protected override void HandleRequest()
        {
            byte[] buf = Encoding.ASCII.GetBytes(text);
            Context.Response.OutputStream.Write(buf, 0, buf.Length);
        }
    }
}
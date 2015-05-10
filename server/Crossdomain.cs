#region

using System.Net;
using System.Text;

#endregion

namespace server
{
    internal class crossdomain : RequestHandler
    {
        protected override void HandleRequest()
        {
            byte[] status = Encoding.UTF8.GetBytes(@"<cross-domain-policy>
<allow-access-from domain=""*""/>
</cross-domain-policy>");
            Context.Response.ContentType = "text/*";
            Context.Response.OutputStream.Write(status, 0, status.Length);
        }
    }
}
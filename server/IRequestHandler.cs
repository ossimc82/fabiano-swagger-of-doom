#region

using System.Net;

#endregion

namespace server
{
    internal interface IRequestHandler
    {
        void HandleRequest(HttpListenerContext context);
    }
}
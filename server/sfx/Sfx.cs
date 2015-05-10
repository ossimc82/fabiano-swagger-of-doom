#region

using System;
using System.IO;
using System.Net;

#endregion

namespace server.sfx
{
    internal class Sfx : RequestHandler
    {
        protected override void HandleRequest()
        {
            string file = Context.Request.Url.LocalPath.StartsWith("/music") ? "sfx/" + Context.Request.Url.LocalPath : Context.Request.Url.LocalPath;

            //context.Response.Redirect("http://realmofthemadgod.appspot.com/" + file);

            if (File.Exists(file))
            {
                using (FileStream i = File.OpenRead(file))
                {
                    byte[] buff = new byte[i.Length];
                    int c;
                    while ((c = i.Read(buff, 0, buff.Length)) > 0)
                        Context.Response.OutputStream.Write(buff, 0, c);
                }
            }
            else
                Context.Response.Redirect("http://realmofthemadgod.appspot.com/" +
                                          (file.Split('/')[1].Contains("music")
                                              ? file.Replace("sfx/", String.Empty)
                                              : file));
        }
    }
}
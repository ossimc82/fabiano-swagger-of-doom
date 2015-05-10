#region

using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Web;

#endregion

namespace server.picture
{
    internal class get : RequestHandler
    {
        private readonly byte[] buff = new byte[0x10000];

        protected override void HandleRequest()
        {
            //warning: maybe has hidden url injection
            string id = Query["id"];

            if (!id.StartsWith("draw:") && !id.StartsWith("file:"))
            {
                //just temponary for testing :)
                byte[] buf = new WebClient().DownloadData("http://rotmgtesting.appspot.com/picture/get?id=" + id);
                Context.Response.OutputStream.Write(buf, 0, buf.Length);
                //string path = Path.GetFullPath("texture/_" + id + ".png");
                //if (!File.Exists(path))
                //{
                //    byte[] status = Encoding.UTF8.GetBytes("<Error>Invalid ID.</Error>");
                //    context.Response.OutputStream.Write(status, 0, status.Length);
                //    return;
                //}
                //using (FileStream i = File.OpenRead(path))
                //{
                //    int c;
                //    string[] rid = id.Split(':');
                //    new WebClient().DownloadData("http://realmofthemadgod.appspot.com/picture/get?id=" + rid[1]);
                //    while ((c = i.Read(buff, 0, buff.Length)) > 0)
                //        context.Response.OutputStream.Write(buff, 0, c);
                //}
            }
            else
            {
                try
                {
                    string[] rid = id.Split(':');
                    //if (XmlDatas.RemoteTextures.ContainsKey(id))
                    //    context.Response.OutputStream.Write(XmlDatas.RemoteTextures[id], 0,
                    //        XmlDatas.RemoteTextures[id].Length);
                    //else if (rid.Length > 1)
                    //    if (rid[0] == "draw")
                    //        XmlDatas.RemoteTextures.Add(id,
                    //            new WebClient().DownloadData("http://realmofthemadgod.appspot.com/picture/get?id=" + rid[1]));
                    //    else if (rid[0] == "tdraw")
                    //        XmlDatas.RemoteTextures.Add(id,
                    //            new WebClient().DownloadData("http://rotmgtesting.appspot.com/picture/get?id=" + rid[1]));
                    //    else
                    //        XmlDatas.RemoteTextures.Add(id, File.ReadAllBytes("texture/" + rid[1] + ".png"));
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
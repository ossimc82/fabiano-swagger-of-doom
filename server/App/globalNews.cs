#region

using db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

#endregion

namespace server.app
{
    internal class globalNews : RequestHandler
    {
        protected override void HandleRequest()
        {
            string s = "[";
            
            using (Database db = new Database())
            {
                var toSerialize = GetGlobalNews(db);
                int len = toSerialize.Count;
            
                for (int i = 0; i < len; i++)
                {
                    if (toSerialize.Count > 1)
                        s += JsonConvert.SerializeObject(toSerialize[0]) + ",";
                    else
                        s += JsonConvert.SerializeObject(toSerialize[0]);
                    toSerialize.RemoveAt(0);
                }
                s += "]";
            }

            byte[] buf = Encoding.UTF8.GetBytes(s);
            Context.Response.OutputStream.Write(buf, 0, buf.Length);
        }

        private List<globalNews_struct> GetGlobalNews(Database db)
        {
            List<globalNews_struct> ret = new List<globalNews_struct>();
            var cmd = db.CreateQuery();
            cmd.CommandText = "SELECT * FROM globalNews WHERE endTime >= now();";
            using (var rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    ret.Add(new globalNews_struct
                    {
                        slot = rdr.GetInt32("slot"),
                        linkType = rdr.GetInt32("linkType"),
                        title = rdr.GetString("title"),
                        image = rdr.GetString("image"),
                        priority = rdr.GetInt32("priority"),
                        linkDetail = rdr.GetString("linkDetail"),
                        platform = rdr.GetString("platform"),
                        startTime = long.Parse(Database.DateTimeToUnixTimestamp(rdr.GetDateTime("startTime")).ToString() + "000"),
                        endTime = long.Parse(Database.DateTimeToUnixTimestamp(rdr.GetDateTime("endTime")).ToString() + "000")
                    });
                }
            }

            return ret;
        }
    }

    public struct globalNews_struct
    {
        public int slot;
        public int linkType;
        public string title;
        public string image;
        public int priority;
        public string linkDetail;
        public string platform;
        public long startTime;
        public long endTime;
    }
}
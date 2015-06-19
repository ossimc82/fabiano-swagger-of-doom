#region

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using db;
using MySql.Data.MySqlClient;
using System.Net.Sockets;
using GoogleMaps.LocationServices;
using Newtonsoft.Json;
using db.data;

#endregion

namespace server.@char
{
    internal class list : RequestHandler
    {
        public MapPoint GetLatLong(string address)
        {
            if (address == "") return null;
            try
            {
                var locationService = new GoogleLocationService();
                return locationService.GetLatLongFromAddress(address);
            }
            catch (Exception) { }
            return null;
        }

        public MapPoint GetLatLong(IPAddress ip)
        {
            if (ip == IPAddress.Any) return null;
            try
            {
                //string json;
                //WebRequest wb = WebRequest.Create("http://freegeoip.net/json/" + ip.ToString());
                //using (StreamReader rdr = new StreamReader(wb.GetResponse().GetResponseStream()))
                //    json = rdr.ReadToEnd();
                //
                //JsonSerializer serializer = new JsonSerializer();
                //var point = (locationPoint)serializer.Deserialize(new StringReader(json), typeof(locationPoint));
                //return new MapPoint
                //{
                //    Latitude = point.latitude,
                //    Longitude = point.longitude
                //};
            }
            catch (Exception) { }
            return null;
        }

        protected override void HandleRequest()
        {
            using (Database db = new Database())
            {
                Account a = db.Verify(Query["guid"], Query["password"], Program.GameData);
                if (!CheckAccount(a, db)) return;
                db.LockAccount(a);
                Chars chrs = new Chars
                {
                    Characters = new List<Char>(),
                    NextCharId = 2,
                    MaxNumChars = 1,
                    Account = a,
                    Servers = GetServerList()
                };
                if (chrs.Account != null)
                {
                    db.GetCharData(chrs.Account, chrs);
                    db.LoadCharacters(chrs.Account, chrs);
                    chrs.News = db.GetNews(Program.GameData, chrs.Account);
                    chrs.OwnedSkins = Utils.GetCommaSepString(chrs.Account.OwnedSkins.ToArray());
                    db.UnlockAccount(chrs.Account);
                }
                else
                {
                    chrs.Account = Database.CreateGuestAccount(Query["guid"] ?? "");
                    chrs.News = db.GetNews(Program.GameData, null);
                }
                MapPoint p = GetLatLong(Context.Request.RemoteEndPoint.Address);
                if (p != null)
                {
                    chrs.Lat = p.Latitude.ToString().Replace(',', '.');
                    chrs.Long = p.Longitude.ToString().Replace(',', '.');
                }
                chrs.ClassAvailabilityList = GetClassAvailability(chrs.Account);
                chrs.TOSPopup = chrs.Account.NotAcceptedNewTos;

                chrs.ClassAvailabilityList = GetClassAvailability(chrs.Account);
                XmlSerializer serializer = new XmlSerializer(chrs.GetType(),
                    new XmlRootAttribute(chrs.GetType().Name) {Namespace = ""});

                XmlWriterSettings xws = new XmlWriterSettings();
                xws.OmitXmlDeclaration = true;
                xws.Encoding = Encoding.UTF8;
                xws.Indent = true;
                XmlWriter xtw = XmlWriter.Create(Context.Response.OutputStream, xws);
                serializer.Serialize(xtw, chrs, chrs.Namespaces);
            }
        }

        private string GetAddress(IPAddress ip)
        {
            string ret = String.Empty;
            WebRequest wb = WebRequest.Create("http://api.hostip.info/get_html.php?ip=" + ip.ToString());
            using(StreamReader rdr = new StreamReader(wb.GetResponse().GetResponseStream()))
            {
                string tmp = null;
                while (!String.IsNullOrWhiteSpace(tmp = rdr.ReadLine()))
                {
                    if (tmp.StartsWith("Country:"))
                    {
                        if (tmp.Split(':')[1].Contains("(Unknown Country?)")) continue;
            
                        ret += tmp.Split(':')[1].Remove(tmp.Split(':')[1].IndexOf("("));
                    }
                    else if (tmp.StartsWith("City:"))
                    {
                        if (tmp.Split(':')[1].Contains("(Unknown City?)")) continue;
            
                        ret += tmp.Split(':')[1];
                    }
                }
            }
            return ret;
        }

        List<ServerItem> GetServerList()
        {
            var ret = new List<ServerItem>();
            int num = Program.Settings.GetValue<int>("svrNum");
            for (int i = 0; i < num; i++)
            {
                MapPoint p = GetLatLong(Program.Settings.GetValue<string>("svr" + i + "Location", ""));
                double usage = GetUsage(Program.Settings.GetValue<string>("svr" + i + "Adr"));

                ret.Add(new ServerItem()
                {
                    Name = Program.Settings.GetValue<string>("svr" + i + "Name"),
                    Lat = p != null ? p.Latitude : 0,
                    Long = p != null ? p.Longitude : 0,
                    DNS = Program.Settings.GetValue<string>("svr" + i + "Adr", "127.0.0.1"),
                    Usage = usage,
                    AdminOnly = Program.Settings.GetValue<bool>("svr" + i + "Admin", "false")
                });
            }
            return ret;
        }

        private static double GetUsage(string host, int port = 2050)
        {
            try
            {
                using (var cli = new TcpClient(host, 2050))
                {
                    var stream = cli.GetStream();
                    stream.Write(new byte[5] {0x4d, 0x61, 0x64, 0x65, 0xff}, 0, 5);
                    var buffer = new byte[cli.ReceiveBufferSize];
                    Array.Resize<byte>(ref buffer, cli.Client.Receive(buffer));
                    var s = Encoding.ASCII.GetString(buffer).Split(':');
                    return double.Parse(s[1])/double.Parse(s[0]);
                }
            }
            catch
            {
                Program.Logger.Error("Not supported server detected: \nHost: " + host + "\nPort: " + port);
                return -1;
            }
        }

        private List<ClassAvailabilityItem> GetClassAvailability(Account acc)
        {
            var classes = new string[14]
            {
                "Rogue",
                "Assassin",
                "Huntress",
                "Mystic",
                "Trickster",
                "Sorcerer",
                "Ninja",
                "Archer",
                "Wizard",
                "Priest",
                "Necromancer",
                "Warrior",
                "Knight",
                "Paladin"
            };

            if (acc == null)
            {
                return new List<ClassAvailabilityItem>
                {
                    new ClassAvailabilityItem {Class = "Rogue", Restricted = "restricted"},
                    new ClassAvailabilityItem {Class = "Assassin", Restricted = "restricted"},
                    new ClassAvailabilityItem {Class = "Huntress", Restricted = "restricted"},
                    new ClassAvailabilityItem {Class = "Mystic", Restricted = "restricted"},
                    new ClassAvailabilityItem {Class = "Trickster", Restricted = "restricted"},
                    new ClassAvailabilityItem {Class = "Sorcerer", Restricted = "restricted"},
                    new ClassAvailabilityItem {Class = "Ninja", Restricted = "restricted"},
                    new ClassAvailabilityItem {Class = "Archer", Restricted = "restricted"},
                    new ClassAvailabilityItem {Class = "Wizard", Restricted = "unrestricted"},
                    new ClassAvailabilityItem {Class = "Priest", Restricted = "restricted"},
                    new ClassAvailabilityItem {Class = "Necromancer", Restricted = "restricted"},
                    new ClassAvailabilityItem {Class = "Warrior", Restricted = "restricted"},
                    new ClassAvailabilityItem {Class = "Knight", Restricted = "restricted"},
                    new ClassAvailabilityItem {Class = "Paladin", Restricted = "restricted"},
                };
            }

            List<ClassAvailabilityItem> ret = new List<ClassAvailabilityItem>();

            using (Database db = new Database())
            {
                MySqlCommand cmd = db.CreateQuery();
                cmd.CommandText = "SELECT class, available FROM unlockedclasses WHERE accId=@accId;";
                cmd.Parameters.AddWithValue("@accId", acc.AccountId);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (!rdr.HasRows)
                {
                    rdr.Close();
                    foreach (string s in classes)
                    {
                        MySqlCommand xcmd = db.CreateQuery();
                        xcmd.CommandText =
                            "INSERT INTO unlockedclasses(accId, class, available) VALUES(@accId, @class, @restricted);";
                        xcmd.Parameters.AddWithValue("@accId", acc.AccountId);
                        xcmd.Parameters.AddWithValue("@class", s);
                        xcmd.Parameters.AddWithValue("@restricted", s == "Wizard" ? "unrestricted" : "restricted");
                        xcmd.ExecuteNonQuery();
                        ret.Add(new ClassAvailabilityItem
                        {
                            Class = s,
                            Restricted = s == "Wizard" ? "unrestricted" : "restricted"
                        });
                    }
                }
                else
                {
                    while (rdr.Read())
                    {
                        ret.Add(new ClassAvailabilityItem
                        {
                            Class = rdr.GetString("class"),
                            Restricted = rdr.GetString("available")
                        });
                    }
                }
            }
            return ret;
        }

        public Chars GetChars(string guid, string password, XmlData data)
        {
            using (var db = new Database())
            {
                Account a = db.Verify(guid, password, data);
                if (a != null)
                {
                    if (a.Banned)
                        return null;
                }

                Chars chrs = new Chars
                {
                    Characters = new List<Char>(),
                    NextCharId = 2,
                    MaxNumChars = 1,
                    Account = a,
                };
                db.GetCharData(chrs.Account, chrs);
                db.LoadCharacters(chrs.Account, chrs);
                chrs.News = db.GetNews(Program.GameData, chrs.Account);
                chrs.OwnedSkins = Utils.GetCommaSepString(chrs.Account.OwnedSkins.ToArray());
                return chrs;
            }
        }

        private class locationPoint
        {
            public string ip;
            public string country_code;
            public string country_name;
            public string region_code;
            public string region_name;
            public string city;
            public string zip_code;
            public string time_zone;
            public double latitude;
            public double longitude;
            public int metro_code;
        }
    }
}
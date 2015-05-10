#region

using System;
using System.Collections.Generic;
using System.IO;
using log4net;

#endregion

namespace db
{
    public class SimpleSettings : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof (SimpleSettings));

        private readonly string cfgFile;
        private readonly string id;
        private readonly Dictionary<string, string> values;

        public SimpleSettings(string id)
        {
            log.InfoFormat("Loading settings for '{0}'...", id);

            values = new Dictionary<string, string>();
            this.id = id;
            cfgFile = Path.Combine(Environment.CurrentDirectory, id + ".cfg");
            if (File.Exists(cfgFile))
                using (StreamReader rdr = new StreamReader(File.OpenRead(cfgFile)))
                {
                    string line;
                    int lineNum = 1;
                    while ((line = rdr.ReadLine()) != null)
                    {
                        if (line.StartsWith("#")) continue;
                        int i = line.IndexOf(":");
                        if (i == -1)
                        {
                            log.InfoFormat("Invalid settings at line {0}.", lineNum);
                            throw new ArgumentException("Invalid settings.");
                        }
                        string val = line.Substring(i + 1);

                        values.Add(line.Substring(0, i),
                            val.Equals("null", StringComparison.InvariantCultureIgnoreCase) ? null : val);
                        lineNum++;
                    }
                    log.InfoFormat("Settings loaded.");
                }
            else
                log.Info("Settings not found.");
        }

        public void Reload()
        {
            log.InfoFormat("Reloading settings for '{0}'...", id);
            values.Clear();
            if (File.Exists(cfgFile))
                using (StreamReader rdr = new StreamReader(File.OpenRead(cfgFile)))
                {
                    string line;
                    int lineNum = 1;
                    while ((line = rdr.ReadLine()) != null)
                    {
                        if (line.StartsWith("#")) continue;
                        int i = line.IndexOf(":");
                        if (i == -1)
                        {
                            log.InfoFormat("Invalid settings at line {0}.", lineNum);
                            throw new ArgumentException("Invalid settings.");
                        }
                        string val = line.Substring(i + 1);

                        values.Add(line.Substring(0, i),
                            val.Equals("null", StringComparison.InvariantCultureIgnoreCase) ? null : val);
                        lineNum++;
                    }
                    log.InfoFormat("Settings loaded.");
                }
            else
                log.Info("Settings not found.");
        }

        public void Dispose()
        {
            try
            {
                log.InfoFormat("Saving settings for '{0}'...", id);
                using (StreamWriter writer = new StreamWriter(File.OpenWrite(cfgFile)))
                    foreach (KeyValuePair<string, string> i in values)
                        writer.WriteLine("{0}:{1}", i.Key, i.Value == null ? "null" : i.Value);
            }
            catch (Exception e)
            {
                log.Error("Error when saving settings.", e);
            }
        }

        public string GetValue(string key, string def = null)
        {
            string ret;
            if (!values.TryGetValue(key, out ret))
            {
                if (def == null)
                {
                    log.ErrorFormat("Attempt to access nonexistant settings '{0}'.", key);
                    throw new ArgumentException(string.Format("'{0}' does not exist in settings.", key));
                }
                ret = values[key] = def;
            }
            return ret;
        }

        public T GetValue<T>(string key, string def = null)
        {
            string ret;
            if (!values.TryGetValue(key, out ret))
            {
                if (def == null)
                {
                    log.ErrorFormat("Attempt to access nonexistant settings '{0}'.", key);
                    throw new ArgumentException(string.Format("'{0}' does not exist in settings.", key));
                }
                ret = values[key] = def;
            }
            return (T) Convert.ChangeType(ret, typeof (T));
        }

        public void SetValue(string key, string val)
        {
            values[key] = val;
        }
    }
}
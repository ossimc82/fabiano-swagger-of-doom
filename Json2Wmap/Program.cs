#region

using System;
using System.IO;
using db.data;

#endregion

namespace Json2Wmap
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length <= 0)
            {
                Console.WriteLine("usage: Json2wmapConv.exe jsonfile");
                return;
            }
            try
            {
                XmlData data = new XmlData();
                FileInfo fi = new FileInfo(args[0]);
                if (fi.Exists)
                    terrain.Json2Wmap.Convert(data, args[0], args[0].Replace(".jm", ".wmap"));
                else
                {
                    Console.WriteLine("input file not found: " + fi.FullName);
                    Console.ReadLine();
                    return;
                }
                Console.WriteLine("done");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception : " + e);
                Console.ReadLine();
            }
        }
    }
}
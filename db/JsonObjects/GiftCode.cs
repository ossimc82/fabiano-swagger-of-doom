using db.data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace db.JsonObjects
{
    public class GiftCode
    {
        public int CharSlots { get; set; }
        public int VaultChests { get; set; }
        public int Fame { get; set; }
        public int Gold { get; set; }
        public List<int> Gifts { get; set; }

        public GiftCode()
        {
            Gifts = new List<int>();
        }

        public string ToJson()
        {
            var wtr = new StringWriter();
            var serializer = new JsonSerializer();
            serializer.Serialize(new JsonTextWriter(wtr), this);
            return wtr.ToString();
        }

        public override string ToString() => ToJson();

        public static GiftCode FromJson(string json) => new JsonSerializer().Deserialize<GiftCode>(new JsonTextReader(new StringReader(json)));

        public static GiftCode GenerateRandom(XmlData data, int minGold=0, int maxGold=10000, int minFame=0, int maxFame=10000, int minCharSlots=0, int maxCharSlots=4, int minVaultChests=0, int maxVaultChests=4, int maxItemStack=10, int minItemStack=1, int maxItemTypes=10, int minItemTypes=1)
        {
            var rand = new Random();
            var ret = new GiftCode();

            var types = rand.Next(minItemTypes, maxItemTypes);
            var c = rand.Next(minItemStack, maxItemStack);

            for (var i = 0; i < types; i++)
            {
                var t = data.Items.ElementAt(rand.Next(0, data.Items.Count)).Key;
                for (var j = 0; j < c; j++)
                    ret.Gifts.Add(t);
                c = rand.Next(minItemStack, maxItemStack);
            }
            ret.CharSlots = rand.Next(minCharSlots, maxCharSlots);
            ret.VaultChests = rand.Next(minVaultChests, maxVaultChests);
            ret.Gold = rand.Next(minGold, maxGold);
            ret.Fame = rand.Next(minFame, maxFame);
            return ret;
        }
    }
}

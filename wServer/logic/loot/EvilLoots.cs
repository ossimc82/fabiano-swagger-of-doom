using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.logic.loot
{
    class EvilLoot : ILoot     //Blood of evil hen! :D
    {
        public Item GetLoot(Random rand)
        {
            return XmlDatas.ItemDescs[0x0a22];
        }
    }
}

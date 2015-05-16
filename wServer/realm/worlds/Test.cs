#region

using System.Collections.Generic;
using System.IO;
using wServer.realm.entities.player;
using wServer.realm.terrain;

#endregion

namespace wServer.realm.worlds
{
    public class Test : World
    {
        public string js = null;

        public Test()
        {
            Id = TEST_ID;
            Name = "Test";
            Background = 0;
        }

        public void LoadJson(string json)
        {
            js = json;
            LoadMap(json);
        }

        public override void Tick(RealmTime time)
        {
            base.Tick(time);

            foreach (KeyValuePair<int, Player> i in Players)
            {
                if (i.Value.Client.Account.Rank < 2)
                {
                    i.Value.Client.Disconnect();
                }
            }
        }

        protected override void Init() { }
    }
}
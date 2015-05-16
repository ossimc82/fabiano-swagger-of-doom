#region

using System;
using System.Collections.Generic;
using wServer.networking.svrPackets;
using wServer.realm.entities;
using wServer.realm.entities.player;

#endregion

namespace wServer.realm.worlds
{
    public class Arena : World
    {
        private bool ready = true;
        private bool waiting;
        public int wave = 1;

        public Arena()
        {
            Id = ARENA;
            Name = "Arena";
            Background = 0;
            AllowTeleport = true;
        }

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.arena.wmap", MapType.Wmap);
        }

        private readonly string[] WeakEnemies =
        {
            "Flamer King", "Lair Skeleton King", "Native Fire Sprite", "Native Ice Sprite", "Native Magic Sprite", "Nomadic Shaman", "Ogre King", "Orc King", "Red Spider", "Sand Phantom",
            "Swarm", "Tawny Warg", "Vampire Bat", "Wasp Queen", "Weretiger"
        };
        private readonly string[] NormalEnemies =
        {
            "Aberrant of Oryx", "Abomination of Oryx", "Adult White Dragon", "Assassin of Oryx", "Bile of Oryx", "Gigacorn",
            "Great Lizard", "Minotaur", "Monstrosity of Oryx", "Phoenix Reborn", "Shambling Sludge", "Urgle"
        };
        private readonly string[] Gods =
        {
            "Beholder", "Ent God", "Flying Brain", "Djinn", "Ghost God", "Leviathan", "Medusa", "Slime God", "Sprite God", "White Demon"
        };
        private readonly string[] Bosses =
        {
            "Tomb Defender", "Tomb Attacker", "Tomb Support", "Arachna the Spider Queen", "Archdemon Malphas", "Crystal Prisoner", "Grand Sphinx", "Limon the Sprite God",
            "Lord Ruthven", "Oryx the Mad God 1", "Septavius the Ghost God", "Stheno the Snake Queen", "Thessal the Mermaid Goddess"
        };

        public bool OutOfBounds(float x, float y)
        {
            if (Map.Height >= y && Map.Width >= x && x > -1 && y > 0)
                return (Map[(int)x, (int)y].Region == TileRegion.Outside_Arena);
            else
                return true;
        }

        public override void Tick(RealmTime time)
        {
            base.Tick(time);
            CheckOutOfBounds();

            if (CheckPopulation())
            {
                if (ready)
                {
                    if (waiting) return;
                    ready = false;
                    wave++;
                    foreach (KeyValuePair<int, Player> i in Players)
                    {
                        i.Value.Client.SendPacket(new ArenaNextWavePacket
                        {
                            Type = wave
                        });
                    }
                    waiting = true;
                    Timers.Add(new WorldTimer(5000, (world, t) =>
                    {
                        ready = false;
                        Spawn();
                        waiting = false;
                    }));
                }
                ready = true;
            }
        }

        private void Spawn()
        {
            /*
             * if(wave % 5 == 0)
             * {
             *      nextDifficulty();
             * }
             */
            try
            {
                List<string> enems = new List<string>();
                Random r = new Random();

                for (int i = 0; i < wave/3 + 1; i++)
                {
                    enems.Add(Gods[r.Next(0, Gods.Length)]);
                }
                for (int i = 0; i < wave/3 + 1; i++)
                {
                    enems.Add(NormalEnemies[r.Next(0, NormalEnemies.Length)]);
                }
                for (int i = 0; i < wave/3 + 1; i++)
                {
                    enems.Add(WeakEnemies[r.Next(0, WeakEnemies.Length)]);
                }
                Random r2 = new Random();
                foreach (string i in enems)
                {
                    ushort id = Manager.GameData.IdToObjectType[i];
                    int xloc = r2.Next(10, Map.Width) - 6;
                    int yloc = r2.Next(10, Map.Height) - 6;
                    Entity enemy = Entity.Resolve(Manager, id);
                    enemy.Move(xloc, yloc);
                    EnterWorld(enemy);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        //private void SpawnBosses()
        //{
        //    List<string> enems = new List<string>();
        //    Random r = new Random();
        //    for (int i = 0; i < (1); i++)
        //    {
        //        enems.Add(Bosses[r.Next(0, Bosses.Length)]);
        //    }
        //    Random r2 = new Random();
        //    foreach (string i in enems)
        //    {
        //        ushort id = Manager.GameData.IdToObjectType[i];
        //        int xloc = r2.Next(10, Map.Width) - 6;
        //        int yloc = r2.Next(10, Map.Height) - 6;
        //        Entity enemy = Entity.Resolve(Manager, id);
        //        enemy.Move(xloc, yloc);
        //        EnterWorld(enemy);
        //    }
        //}

        private bool CheckPopulation()
        {
            if (Enemies.Count == 0)
            {
                return true;
            }
            return false;
        }

        private void CheckOutOfBounds()
        {
            foreach (KeyValuePair<int, Enemy> i in Enemies)
            {
                if (OutOfBounds(i.Value.X, i.Value.Y))
                {
                    LeaveWorld(i.Value);
                }
            }
        }
    }
}
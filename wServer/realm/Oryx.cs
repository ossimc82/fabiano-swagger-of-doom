#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using wServer.networking.svrPackets;
using wServer.realm.entities;
using wServer.realm.entities.player;
using wServer.realm.setpieces;
using wServer.realm.worlds;
using wServer.networking;

#endregion

namespace wServer.realm
{
    //The mad god who look after the realm
    internal class Oryx : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Oryx));
        private readonly int[] enemyCounts = new int[12];
        private readonly int[] enemyMaxCounts = new int[12];

        private readonly List<Tuple<string, ISetPiece>> events = new List<Tuple<string, ISetPiece>>
        {
            Tuple.Create("Skull Shrine", (ISetPiece) new SkullShrine()),
            Tuple.Create("Pentaract", (ISetPiece) new Pentaract()),
            Tuple.Create("Grand Sphinx", (ISetPiece) new Sphinx()),
            //"Lord of the Lost Lands",
            //"Hermit God",
            //"Ghost Ship",
            Tuple.Create("Cube God", (ISetPiece) new CubeGod()),
        };

        private readonly Random rand = new Random();
        private GameWorld world;
        public bool ClosingStarted = false;
        public bool RealmClosed = false;
        private bool disposed = false;
        private long prevTick;

        private int x;

        public Oryx(GameWorld world)
        {
            this.world = world;
            Init();
        }

        #region "Spawn data"

        private static readonly Dictionary<WmapTerrain, Tuple<int, Tuple<string, double>[]>>
            spawn = new Dictionary<WmapTerrain, Tuple<int, Tuple<string, double>[]>>
            {
                {
                    WmapTerrain.ShoreSand, Tuple.Create(
                        100, new[]
                        {
                            Tuple.Create("Pirate", 0.3),
                            Tuple.Create("Piratess", 0.1),
                            Tuple.Create("Snake", 0.2),
                            Tuple.Create("Scorpion Queen", 0.4)
                        })
                },
                {
                    WmapTerrain.ShorePlains, Tuple.Create(
                        150, new[]
                        {
                            Tuple.Create("Bandit Leader", 0.4),
                            Tuple.Create("Red Gelatinous Cube", 0.2),
                            Tuple.Create("Purple Gelatinous Cube", 0.2),
                            Tuple.Create("Green Gelatinous Cube", 0.2)
                        })
                },
                {
                    WmapTerrain.LowPlains, Tuple.Create(
                        200, new[]
                        {
                            Tuple.Create("Hobbit Mage", 0.5),
                            Tuple.Create("Undead Hobbit Mage", 0.4),
                            Tuple.Create("Sumo Master", 0.1)
                        })
                },
                {
                    WmapTerrain.LowForest, Tuple.Create(
                        200, new[]
                        {
                            Tuple.Create("Elf Wizard", 0.2),
                            Tuple.Create("Goblin Mage", 0.2),
                            Tuple.Create("Easily Enraged Bunny", 0.3),
                            Tuple.Create("Forest Nymph", 0.3)
                        })
                },
                {
                    WmapTerrain.LowSand, Tuple.Create(
                        200, new[]
                        {
                            Tuple.Create("Sandsman King", 0.4),
                            Tuple.Create("Giant Crab", 0.2),
                            Tuple.Create("Sand Devil", 0.4)
                        })
                },
                {
                    WmapTerrain.MidPlains, Tuple.Create(
                        150, new[]
                        {
                            Tuple.Create("Fire Sprite", 0.1),
                            Tuple.Create("Ice Sprite", 0.1),
                            Tuple.Create("Magic Sprite", 0.1),
                            Tuple.Create("Pink Blob", 0.07),
                            Tuple.Create("Gray Blob", 0.07),
                            Tuple.Create("Earth Golem", 0.04),
                            Tuple.Create("Paper Golem", 0.04),
                            Tuple.Create("Big Green Slime", 0.08),
                            Tuple.Create("Swarm", 0.05),
                            Tuple.Create("Wasp Queen", 0.2),
                            Tuple.Create("Shambling Sludge", 0.03),
                            Tuple.Create("Orc King", 0.06)
                        })
                },
                {
                    WmapTerrain.MidForest, Tuple.Create(
                        150, new[]
                        {
                            Tuple.Create("Dwarf King", 0.3),
                            Tuple.Create("Metal Golem", 0.05),
                            Tuple.Create("Clockwork Golem", 0.05),
                            Tuple.Create("Werelion", 0.1),
                            Tuple.Create("Horned Drake", 0.3),
                            Tuple.Create("Red Spider", 0.1),
                            Tuple.Create("Black Bat", 0.1)
                        })
                },
                {
                    WmapTerrain.MidSand, Tuple.Create(
                        300, new[]
                        {
                            Tuple.Create("Desert Werewolf", 0.25),
                            Tuple.Create("Fire Golem", 0.1),
                            Tuple.Create("Darkness Golem", 0.1),
                            Tuple.Create("Sand Phantom", 0.2),
                            Tuple.Create("Nomadic Shaman", 0.25),
                            Tuple.Create("Great Lizard", 0.1)
                        })
                },
                {
                    WmapTerrain.HighPlains, Tuple.Create(
                        300, new[]
                        {
                            Tuple.Create("Shield Orc Key", 0.2),
                            Tuple.Create("Urgle", 0.2),
                            Tuple.Create("Undead Dwarf God", 0.6)
                        })
                },
                {
                    WmapTerrain.HighForest, Tuple.Create(
                        300, new[]
                        {
                            Tuple.Create("Ogre King", 0.4),
                            Tuple.Create("Dragon Egg", 0.1),
                            Tuple.Create("Lizard God", 0.5)
                        })
                },
                {
                    WmapTerrain.HighSand, Tuple.Create(
                        250, new[]
                        {
                            Tuple.Create("Minotaur", 0.4),
                            Tuple.Create("Flayer God", 0.4),
                            Tuple.Create("Flamer King", 0.2)
                        })
                },
                {
                    WmapTerrain.Mountains, Tuple.Create(
                    100, new []
                    {
                        Tuple.Create("White Demon", 0.1),
                        Tuple.Create("Sprite God", 0.09),
                        Tuple.Create("Medusa", 0.1),
                        Tuple.Create("Ent God", 0.1),
                        Tuple.Create("Beholder", 0.1),
                        Tuple.Create("Flying Brain", 0.1),
                        Tuple.Create("Slime God", 0.09),
                        Tuple.Create("Ghost God", 0.09),
                        Tuple.Create("Rock Bot", 0.05),
                        Tuple.Create("Djinn", 0.09),
                        Tuple.Create("Leviathan", 0.09),
                        Tuple.Create("Arena Headless Horseman", 0.01)
                    })
                },
            };

        #endregion "Spawn data"

        public static double GetNormal(Random rand)
        {
            // Use Box-Muller algorithm
            var u1 = GetUniform(rand);
            var u2 = GetUniform(rand);
            var r = Math.Sqrt(-2.0 * Math.Log(u1));
            var theta = 2.0 * Math.PI * u2;
            return r * Math.Sin(theta);
        }

        public static double GetNormal(Random rand, double mean, double standardDeviation)
        {
            return mean + standardDeviation * GetNormal(rand);
        }

        public static double GetUniform(Random rand)
        {
            // 0 <= u < 2^32
            var u = (uint)(rand.NextDouble() * uint.MaxValue);
            // The magic number below is 1/(2^32 + 2).
            // The result is strictly between 0 and 1.
            return (u + 1.0) * 2.328306435454494e-10;
        }

        public bool CheckFinalQuests()
        {
            if (CountEnemies(
                "Lich", "Actual Lich",
                "Ent Ancient", "Actual Ent Ancient",
                "Phoenix Reborn",
                "Oasis Giant", "Ghost King", "Cyclops God", "Red Demon",
                "Skull Shrine", "Cube God", "Grand Sphinx", "Hermit God") != 0) return false;
            RealmClosed = true;
            return true;
        }

        public void CloseRealm()
        {
            World ocWorld = null;
            world.Timers.Add(new WorldTimer(2000, (w, t) =>
            {
                ocWorld = world.Manager.AddWorld(new OryxCastle());
                ocWorld.Manager = world.Manager;
            }));
            world.Timers.Add(new WorldTimer(8000, (w, t) =>
            {
                foreach (var i in world.Players.Values)
                {
                    if (ocWorld == null) i.Client.Disconnect();
                    i.Client.SendPacket(new ReconnectPacket
                    {
                        Host = "",
                        Port = Program.Settings.GetValue<int>("port"),
                        GameId = ocWorld.Id,
                        Name = ocWorld.Name,
                        Key = ocWorld.PortalKey
                    });
                }
            }));
            foreach (var i in world.Players.Values)
            {
                SendMsg(i, "MY MINIONS HAVE FAILED ME!", "#Oryx the Mad God");
                SendMsg(i, "BUT NOW YOU SHALL FEEL MY WRATH!", "#Oryx the Mad God");
                SendMsg(i, "COME MEET YOUR DOOM AT THE WALLS OF MY CASTLE!", "#Oryx the Mad God");
                i.Client.SendPacket(new ShowEffectPacket
                {
                    EffectType = EffectType.Earthquake
                });
            }
            world.Timers.Add(new WorldTimer(10000, (w, t) => w.Manager.RemoveWorld(w)));
        }

        public int CountEnemies(params string[] enemies)
        {
            var enemyList = new List<ushort>();
            foreach (var i in enemies)
            {
                try
                {
                    enemyList.Add(world.Manager.GameData.IdToObjectType[i]);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            return world.Enemies.Count(i => enemyList.Contains(i.Value.ObjectType));
        }

        public void Init()
        {
            log.InfoFormat("Oryx is controlling world {0}({1})...", world.Id, world.Name);
            var w = world.Map.Width;
            var h = world.Map.Height;
            var stats = new int[12];
            for (var y = 0; y < h; y++)
                for (var x = 0; x < w; x++)
                {
                    var tile = world.Map[x, y];
                    if (tile.Terrain != WmapTerrain.None)
                        stats[(int)tile.Terrain - 1]++;
                }

            log.Info("Spawning minions...");
            foreach (var i in spawn)
            {
                var terrain = i.Key;
                var idx = (int)terrain - 1;
                var enemyCount = stats[idx] / i.Value.Item1;
                enemyMaxCounts[idx] = enemyCount;
                enemyCounts[idx] = 0;
                for (var j = 0; j < enemyCount; j++)
                {
                    var objType = GetRandomObjType(i.Value.Item2);
                    if (objType == 0) continue;

                    enemyCounts[idx] += Spawn(world.Manager.GameData.ObjectDescs[objType], terrain, w, h);
                    if (enemyCounts[idx] >= enemyCount) break;
                }
            }
            log.Info("Oryx is done.");
        }

        public void InitCloseRealm()
        {
            log.InfoFormat("Oryx has closed realm {0}...", world.Name);
            ClosingStarted = true;
            foreach (var i in world.Players.Values)
            {
                SendMsg(i, "I HAVE CLOSED THIS REALM!", "#Oryx the Mad God");
                SendMsg(i, "YOU WILL NOT LIVE TO SEE THE LIGHT OF DAY!", "#Oryx the Mad God");
            }
            world.Timers.Add(new WorldTimer(120000, (ww, tt) => { CloseRealm(); }));
            world.Manager.GetWorld(World.NEXUS_ID).Timers.Add(new WorldTimer(130000, (w, t) => Task.Factory.StartNew(() => GameWorld.AutoName(1, true)).ContinueWith(_ => w.Manager.AddWorld(_.Result), TaskScheduler.Default)));
            world.Manager.CloseWorld(world);
        }

        public void OnEnemyKilled(Enemy enemy, Player killer)
        {
            if (enemy.ObjectDesc != null && enemy.ObjectDesc.Quest)
            {
                TauntData? dat = null;
                foreach (var i in criticalEnemies)
                    if ((enemy.ObjectDesc.DisplayId ?? enemy.ObjectDesc.ObjectId) == i.Item1)
                    {
                        dat = i.Item2;
                        break;
                    }
                if (dat == null) return;

                if (dat.Value.killed != null)
                {
                    var arr = dat.Value.killed;
                    var msg = arr[rand.Next(0, arr.Length)];
                    while (killer == null && msg.Contains("{PLAYER}"))
                        msg = arr[rand.Next(0, arr.Length)];
                    msg = msg.Replace("{PLAYER}", killer.Name);
                    BroadcastMsg(msg);
                }

                if (rand.NextDouble() < 0.25)
                {
                    var evt = events[rand.Next(0, events.Count)];
                    if (
                        world.Manager.GameData.ObjectDescs[world.Manager.GameData.IdToObjectType[evt.Item1]].PerRealmMax ==
                        1)
                        events.Remove(evt);
                    SpawnEvent(evt.Item1, evt.Item2);

                    dat = null;
                    foreach (var i in criticalEnemies)
                        if (evt.Item1 == i.Item1)
                        {
                            dat = i.Item2;
                            break;
                        }
                    if (dat == null) return;

                    if (dat.Value.spawn != null)
                    {
                        var arr = dat.Value.spawn;
                        var msg = arr[rand.Next(0, arr.Length)];
                        BroadcastMsg(msg);
                    }
                }
            }
        }

        public void OnPlayerEntered(Player player)
        {
            player.SendInfo("Welcome to Realm of the Mad God");
            player.SendEnemy("Oryx the Mad God", "You are food for my minions!");
            player.SendInfo("Use [WASDQE] to move; click to shoot!");
            player.SendInfo("Type \"/help\" for more help");
        }

        public void Tick(RealmTime time)
        {
            if (!disposed)
            {
                if (CheckFinalQuests())
                {
                    if (!ClosingStarted)
                    {
                        InitCloseRealm();
                    }
                }
                else
                {
                    if (time.tickTimes - prevTick > 25000)
                    {
                        if (x % 2 == 0)
                            HandleAnnouncements();
                        if (x % 6 == 0)
                            EnsurePopulation();
                        x++;
                        prevTick = time.tickTimes;
                    }
                }
            }
        }

        private void BroadcastMsg(string message)
        {
            world.Manager.Chat.Oryx(world, message);
        }

        private void EnsurePopulation()
        {
            log.Info("Oryx is controlling population...");
            RecalculateEnemyCount();
            var state = new int[12];
            var diff = new int[12];
            var c = 0;
            for (var i = 0; i < state.Length; i++)
            {
                if (enemyCounts[i] > enemyMaxCounts[i] * 1.5)  //Kill some
                {
                    state[i] = 1;
                    diff[i] = enemyCounts[i] - enemyMaxCounts[i];
                    c++;
                }
                else if (enemyCounts[i] < enemyMaxCounts[i] * 0.75) //Add some
                {
                    state[i] = 2;
                    diff[i] = enemyMaxCounts[i] - enemyCounts[i];
                }
                else
                {
                    state[i] = 0;
                }
            }
            foreach (var i in world.Enemies)    //Kill
            {
                var idx = (int)i.Value.Terrain - 1;
                if (idx == -1 || state[idx] == 0 ||
                    i.Value.GetNearestEntity(10, true) != null ||
                    diff[idx] == 0)
                    continue;

                if (state[idx] == 1)
                {
                    world.LeaveWorld(i.Value);
                    diff[idx]--;
                    if (diff[idx] == 0)
                        c--;
                }
                if (c == 0) break;
            }

            int w = world.Map.Width, h = world.Map.Height;
            for (var i = 0; i < state.Length; i++)  //Add
            {
                if (state[i] != 2) continue;
                var x = diff[i];
                var t = (WmapTerrain)(i + 1);
                for (var j = 0; j < x; )
                {
                    var objType = GetRandomObjType(spawn[t].Item2);
                    if (objType == 0) continue;

                    j += Spawn(world.Manager.GameData.ObjectDescs[objType], t, w, h);
                }
            }
            RecalculateEnemyCount();

            GC.Collect();
            log.Info("Oryx is back to sleep.");
        }

        private ushort GetRandomObjType(Tuple<string, double>[] dat)
        {
            var p = rand.NextDouble();
            double n = 0;
            ushort objType = 0;
            foreach (var k in dat)
            {
                n += k.Item2;
                if (n > p)
                {
                    objType = world.Manager.GameData.IdToObjectType[k.Item1];
                    break;
                }
            }
            return objType;
        }

        private void HandleAnnouncements()
        {
            var taunt = criticalEnemies[rand.Next(0, criticalEnemies.Length)];
            var count = 0;
            foreach (var i in world.Enemies)
            {
                var desc = i.Value.ObjectDesc;
                if (desc == null || (desc.DisplayId ?? desc.ObjectId) != taunt.Item1)
                    continue;
                count++;
            }

            if (count == 0) return;
            if (count == 1 && taunt.Item2.final != null)
            {
                var arr = taunt.Item2.final;
                var msg = arr[rand.Next(0, arr.Length)];
                BroadcastMsg(msg);
            }
            else
            {
                var arr = taunt.Item2.numberOfEnemies;
                var msg = arr[rand.Next(0, arr.Length)];
                msg = msg.Replace("{COUNT}", count.ToString());
                BroadcastMsg(msg);
            }
        }

        private void RecalculateEnemyCount()
        {
            for (var i = 0; i < enemyCounts.Length; i++)
                enemyCounts[i] = 0;
            foreach (var i in world.Enemies)
            {
                if (i.Value.Terrain == WmapTerrain.None) continue;
                enemyCounts[(int)i.Value.Terrain - 1]++;
            }
        }

        //tick of 10 seconds
        private void SendMsg(Player player, string message, string src = "")
        {
            player.Client.SendPacket(new TextPacket
            {
                Name = src,
                ObjectId = -1,
                Stars = -1,
                BubbleTime = 0,
                Recipient = "",
                Text = message,
                CleanText = ""
            });
        }

        private int Spawn(ObjectDesc desc, WmapTerrain terrain, int w, int h)
        {
            Entity entity;
            var ret = 0;
            var pt = new IntPoint();
            if (desc.Spawn != null)
            {
                var num = (int)GetNormal(rand, desc.Spawn.Mean, desc.Spawn.StdDev);
                if (num > desc.Spawn.Max) num = desc.Spawn.Max;
                else if (num < desc.Spawn.Min) num = desc.Spawn.Min;

                do
                {
                    pt.X = rand.Next(0, w);
                    pt.Y = rand.Next(0, h);
                } while (world.Map[pt.X, pt.Y].Terrain != terrain ||
                         !world.IsPassable(pt.X, pt.Y) ||
                         world.AnyPlayerNearby(pt.X, pt.Y));

                for (var k = 0; k < num; k++)
                {
                    entity = Entity.Resolve(world.Manager, desc.ObjectType);
                    entity.Move(
                        pt.X + (float)(rand.NextDouble() * 2 - 1) * 5,
                        pt.Y + (float)(rand.NextDouble() * 2 - 1) * 5);
                    (entity as Enemy).Terrain = terrain;
                    if (entity.GetNearestEntity(10, true) == null)
                        world.EnterWorld(entity);
                    ret++;
                }
            }
            else
            {
                do
                {
                    pt.X = rand.Next(0, w);
                    pt.Y = rand.Next(0, h);
                } while (world.Map[pt.X, pt.Y].Terrain != terrain ||
                         !world.IsPassable(pt.X, pt.Y) ||
                         world.AnyPlayerNearby(pt.X, pt.Y));

                entity = Entity.Resolve(world.Manager, desc.ObjectType);
                entity.Move(pt.X, pt.Y);
                (entity as Enemy).Terrain = terrain;
                world.EnterWorld(entity);
                ret++;
            }
            return ret;
        }

        private void SpawnEvent(string name, ISetPiece setpiece)
        {
            var pt = new IntPoint();
            do
            {
                pt.X = rand.Next(0, world.Map.Width);
                pt.Y = rand.Next(0, world.Map.Height);
            } while ((world.Map[pt.X, pt.Y].Terrain < WmapTerrain.Mountains ||
                      world.Map[pt.X, pt.Y].Terrain > WmapTerrain.MidForest) ||
                     !world.IsPassable(pt.X, pt.Y) ||
                     world.AnyPlayerNearby(pt.X, pt.Y));

            pt.X -= (setpiece.Size - 1) / 2;
            pt.Y -= (setpiece.Size - 1) / 2;
            setpiece.RenderSetPiece(world, pt);
            log.InfoFormat("Oryx spawned {0} at ({1}, {2}).", name, pt.X, pt.Y);
        }

        private struct TauntData
        {
            public string[] final;
            public string[] killed;
            public string[] numberOfEnemies;
            public string[] spawn;
        }

        #region "Taunt data"

        //https://forums.wildshadow.com/node/119997
        private static readonly Tuple<string, TauntData>[] criticalEnemies =
        {
            Tuple.Create("Lich", new TauntData
            {
                numberOfEnemies = new[]
                {
                    "I am invincible while my {COUNT} Liches still stand!",
                    "My {COUNT} Liches will feast on your essence!"
                },
                final = new[]
                {
                    "My final Lich shall consume your souls!",
                    "My final Lich will protect me forever!"
                }
            }),
            Tuple.Create("Ent Ancient", new TauntData
            {
                numberOfEnemies = new[]
                {
                    "Mortal scum! My {COUNT} Ent Ancients will defend me forever!",
                    "My forest of {COUNT} Ent Ancients is all the protection I need!"
                },
                final = new[]
                {
                    "My final Ent Ancient will destroy you all!"
                }
            }),
            Tuple.Create("Oasis Giant", new TauntData
            {
                numberOfEnemies = new[]
                {
                    "My {COUNT} Oasis Giants will feast on your flesh!",
                    "You have no hope against my {COUNT} Oasis Giants!"
                },
                final = new[]
                {
                    "A powerful Oasis Giant still fights for me!",
                    "You will never defeat me while an Oasis Giant remains!"
                }
            }),
            Tuple.Create("Phoenix Lord", new TauntData
            {
                numberOfEnemies = new[]
                {
                    "Maggots! My {COUNT} Phoenix Lord will burn you to ash!"
                },
                final = new[]
                {
                    "My final Phoenix Lord will never fall!",
                    "My last Phoenix Lord will blacken your bones!"
                }
            }),
            Tuple.Create("Ghost King", new TauntData
            {
                numberOfEnemies = new[]
                {
                    "My {COUNT} Ghost Kings give me more than enough protection!",
                    "Pathetic humans! My {COUNT} Ghost Kings shall destroy you utterly!"
                },
                final = new[]
                {
                    "A mighty Ghost King remains to guard me!",
                    "My final Ghost King is untouchable!"
                }
            }),
            Tuple.Create("Cyclops God", new TauntData
            {
                numberOfEnemies = new[]
                {
                    "Cretins! I have {COUNT} Cyclops Gods to guard me!",
                    "My {COUNT} powerful Cyclops Gods will smash you!"
                },
                final = new[]
                {
                    "My last Cyclops God will smash you to pieces!"
                }
            }),
            Tuple.Create("Red Demon", new TauntData
            {
                numberOfEnemies = new[]
                {
                    "Fools! There is no escape from my {COUNT} Red Demons!",
                    "My legion of {COUNT} Red Demons live only to serve me!"
                },
                final = new[]
                {
                    "My final Red Demon is unassailable!"
                }
            }),
            Tuple.Create("Skull Shrine", new TauntData
            {
                spawn = new[]
                {
                    "Your futile efforts are no match for a Skull Shrine!"
                },
                numberOfEnemies = new[]
                {
                    "Imbeciles! My {COUNT} Skull Shrines make me invincible!"
                },
                killed = new[]
                {
                    "{PLAYER} defaced a Skull Shrine! Minions, to arms!",
                    "{PLAYER} razed one of my Skull Shrines -- I WILL HAVE MY REVENGE!",
                    "{PLAYER}, you will rue the day you dared to defile my Skull Shrine!",
                    "{PLAYER}, you contemptible pig! Ruining my Skull Shrine will be the last mistake you ever make!",
                    "{PLAYER}, you insignificant cur! The penalty for destroying a Skull Shrine is death!"
                }
            }),
            Tuple.Create("Cube God", new TauntData
            {
                spawn = new[]
                {
                    "Your meager abilities cannot possibly challenge a Cube God!"
                },
                numberOfEnemies = new[]
                {
                    "Filthy vermin! My {COUNT} Cube Gods will exterminate you!",
                    "Loathsome slugs! My {COUNT} Cube Gods will defeat you!",
                    "You piteous cretins! {COUNT} Cube Gods still guard me!",
                    "Your pathetic rabble will never survive against my {COUNT} Cube Gods!"
                },
                final = new[]
                {
                    "Worthless mortals! A mighty Cube God defends me!"
                },
                killed = new[]
                {
                    "You have dispatched my Cube God, {PLAYER}, but you will never escape my Realm!",
                    "I have many more Cube Gods, {PLAYER}!",
                    "{PLAYER}, you wretched dog! You killed my Cube God!",
                    "{PLAYER}, you pathetic swine! How dare you assault my Cube God!"
                }
            }),
            Tuple.Create("Pentaract", new TauntData
            {
                spawn = new[]
                {
                    "Behold my Pentaract, and despair!"
                },
                numberOfEnemies = new[]
                {
                    "Wretched creatures! {COUNT} Pentaracts remain!",
                    "You detestable humans will never defeat my {COUNT} Pentaracts!",
                    "Defiance is useless! My {COUNT} Pentaracts will crush you!"
                },
                final = new[]
                {
                    "Ignorant fools! A Pentaract guards me still!"
                },
                killed = new[]
                {
                    "That was but one of many Pentaracts, {PLAYER}!",
                    "{PLAYER}, you flea-ridden animal! You destoryed my Pentaract!",
                    "{PLAYER}, by destroying my Pentaract you have sealed your own doom!"
                }
            }),
            Tuple.Create("Grand Sphinx", new TauntData
            {
                spawn = new[]
                {
                    "At last, a Grand Sphinx will teach you to respect!",
                    "A Grand Sphinx is more than a match for this rabble."
                },
                numberOfEnemies = new[]
                {
                    "You dull-spirited apes! You shall pose no challenge for {COUNT} Grand Sphinxes!",
                    "Regret your choices, blasphemers! My {COUNT} Grand Sphinxes will teach you respect!",
                    "My Grand Sphinxes will bewitch you with their beauty!"
                },
                final = new[]
                {
                    "You festering rat-catchers! A Grand Sphinx will make you doubt your purpose!",
                    "Gaze upon the beauty of the Grand Sphinx and feel your last hopes drain away."
                },
                killed = new[]
                {
                    "My Grand Sphinx, she was so beautiful. I will kill you myself, {PLAYER}!",
                    "My Grand Sphinx had lived for thousands of years! You, {PLAYER}, will not survive the day!",
                    "{PLAYER}, you up-jumped goat herder! You shall pay for defeating my Grand Sphinx!",
                    "{PLAYER}, you pestiferous lout! I will not forget what you did to my Grand Sphinx!",
                    "{PLAYER}, you foul ruffian! Do not think I forget your defiling of my Grand Sphinx!"
                }
            }),
            Tuple.Create("Lord of the Lost Lands", new TauntData
            {
                spawn = new[]
                {
                    "Pathetic fools! My Lord of the Lost Lands will crush you all!",
                    "My Lord of the Lost Lands will make short work of you!"
                },
                //numberOfEnemies = new string[] {
                //    "You dull-spirited apes! You shall pose no challenge for {COUNT} Grand Sphinxes!",
                //    "Regret your choices, blasphemers! My {COUNT} Grand Sphinxes will teach you respect!",
                //    "My Grand Sphinxes will bewitch you with their beauty!"
                //},
                //final = new string[] {
                //    "You festering rat-catchers! A Grand Sphinx will make you doubt your purpose!",
                //    "Gaze upon the beauty of the Grand Sphinx and feel your last hopes drain away."
                //},
                killed = new[]
                {
                    "How dare you foul-mouthed hooligans treat my Lord of the Lost Lands with such indignity!",
                    "What trickery is this?! My Lord of the Lost Lands was invincible!"
                }
            }),
            Tuple.Create("Hermit God", new TauntData
            {
                spawn = new[]
                {
                    "My Hermit God's thousand tentacles shall drag you to a watery grave!"
                },
                //numberOfEnemies = new string[] {
                //    "You dull-spirited apes! You shall pose no challenge for {COUNT} Grand Sphinxes!",
                //    "Regret your choices, blasphemers! My {COUNT} Grand Sphinxes will teach you respect!",
                //    "My Grand Sphinxes will bewitch you with their beauty!"
                //},
                //final = new string[] {
                //    "You festering rat-catchers! A Grand Sphinx will make you doubt your purpose!",
                //    "Gaze upon the beauty of the Grand Sphinx and feel your last hopes drain away."
                //},
                killed = new[]
                {
                    "My Hermit God was more than you'll ever be, {PLAYER}. I will kill you myself!",
                    "You naive imbecile, {PLAYER}! Without my Hermit God, Dreadstump is free to roam the seas without fear!"
                }
            }),
            Tuple.Create("Ghost Ship", new TauntData
            {
                spawn = new[]
                {
                    "My Ghost Ship will terrorize you pathetic peasants!",
                    "A Ghost Ship has entered the Realm."
                },
                //numberOfEnemies = new string[] {
                //    "You dull-spirited apes! You shall pose no challenge for {COUNT} Grand Sphinxes!",
                //    "Regret your choices, blasphemers! My {COUNT} Grand Sphinxes will teach you respect!",
                //    "My Grand Sphinxes will bewitch you with their beauty!"
                //},
                //final = new string[] {
                //    "You festering rat-catchers! A Grand Sphinx will make you doubt your purpose!",
                //    "Gaze upon the beauty of the Grand Sphinx and feel your last hopes drain away."
                //},
                killed = new[]
                {
                    "How could a creature like {PLAYER} defeat my dreaded Ghost Ship?!",
                    "{PLAYER}, has crossed me for the last time! My Ghost Ship shall be avenged.",
                    "The spirits of the sea will seek revenge on your worthless soul, {PLAYER}!"
                }
            })
        };

        #endregion "Taunt data"

        public void Dispose()
        {
            disposed = true;
        }
    }
}
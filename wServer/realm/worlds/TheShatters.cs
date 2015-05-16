#region

using wServer.networking;

#endregion

namespace wServer.realm.worlds
{
    public class TheShatters : World
    {
        public TheShatters()
        {
            Name = "The Shatters";
            ClientWorldName = "shatters.The_Shatters";
            Background = 0;
            AllowTeleport = false;
        }

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.shittersmep.wmap", MapType.Wmap);
            Entity en = Entity.Resolve(Manager, "shtrs Bridge Titanum");
            en.Move(233.5f, 36.5f);
            this.EnterWorld(en);
            en = Entity.Resolve(Manager, "shtrs Bridge Titanum");
            en.Move(212.5f, 49.5f);
            this.EnterWorld(en);
            en = Entity.Resolve(Manager, "shtrs Bridge Titanum");
            en.Move(212.5f, 36.5f);
            this.EnterWorld(en);
        }

        public override World GetInstance(Client psr)
        {
            return Manager.AddWorld(new TheShatters());
        }

        public void CloseBridge1()
        {
            WmapTile tile;
            foreach (var p in Players)
            {
                for (int i = 138; i < 152; i++)
                {
                    tile = p.Value.Owner.Map[i, 163].Clone();
                    tile.TileId = Manager.GameData.IdToTileType["shtrs Pure Evil"];
                    p.Value.Owner.Map[i, 163] = tile;

                    tile = p.Value.Owner.Map[i, 164].Clone();
                    tile.TileId = Manager.GameData.IdToTileType["shtrs Pure Evil"];
                    p.Value.Owner.Map[i, 164] = tile;

                    tile = p.Value.Owner.Map[i, 165].Clone();
                    tile.TileId = Manager.GameData.IdToTileType["shtrs Pure Evil"];
                    p.Value.Owner.Map[i, 165] = tile;

                    tile = p.Value.Owner.Map[i, 177].Clone();
                    tile.TileId = Manager.GameData.IdToTileType["shtrs Pure Evil"];
                    p.Value.Owner.Map[i, 177] = tile;

                    tile = p.Value.Owner.Map[i, 178].Clone();
                    tile.TileId = Manager.GameData.IdToTileType["shtrs Pure Evil"];
                    p.Value.Owner.Map[i, 178] = tile;

                    tile = p.Value.Owner.Map[i, 179].Clone();
                    tile.TileId = Manager.GameData.IdToTileType["shtrs Pure Evil"];
                    p.Value.Owner.Map[i, 179] = tile;
                }
            }

            Timers.Add(new WorldTimer(10000, (w, t) =>
            {
                foreach (var p in Players)
                {
                    for (int i = 138; i < 152; i++)
                    {
                        tile = p.Value.Owner.Map[i, 166].Clone();
                        tile.TileId = Manager.GameData.IdToTileType["shtrs Pure Evil"];
                        p.Value.Owner.Map[i, 166] = tile;

                        tile = p.Value.Owner.Map[i, 167].Clone();
                        tile.TileId = Manager.GameData.IdToTileType["shtrs Pure Evil"];
                        p.Value.Owner.Map[i, 167] = tile;

                        tile = p.Value.Owner.Map[i, 175].Clone();
                        tile.TileId = Manager.GameData.IdToTileType["shtrs Pure Evil"];
                        p.Value.Owner.Map[i, 175] = tile;

                        tile = p.Value.Owner.Map[i, 176].Clone();
                        tile.TileId = Manager.GameData.IdToTileType["shtrs Pure Evil"];
                        p.Value.Owner.Map[i, 176] = tile;
                    }
                }

                Timers.Add(new WorldTimer(10000, (w1, t1) =>
                {
                    foreach (var p in Players)
                    {
                        for (int i = 138; i < 152; i++)
                        {
                            tile = p.Value.Owner.Map[i, 168].Clone();
                            tile.TileId = Manager.GameData.IdToTileType["shtrs Pure Evil"];
                            p.Value.Owner.Map[i, 168] = tile;

                            tile = p.Value.Owner.Map[i, 169].Clone();
                            tile.TileId = Manager.GameData.IdToTileType["shtrs Pure Evil"];
                            p.Value.Owner.Map[i, 169] = tile;

                            tile = p.Value.Owner.Map[i, 173].Clone();
                            tile.TileId = Manager.GameData.IdToTileType["shtrs Pure Evil"];
                            p.Value.Owner.Map[i, 173] = tile;

                            tile = p.Value.Owner.Map[i, 174].Clone();
                            tile.TileId = Manager.GameData.IdToTileType["shtrs Pure Evil"];
                            p.Value.Owner.Map[i, 174] = tile;
                        }
                    }

                    Timers.Add(new WorldTimer(10000, (w2, t2) =>
                    {
                        foreach (var p in Players)
                        {
                            for (int i = 138; i < 152; i++)
                            {
                                tile = p.Value.Owner.Map[i, 170].Clone();
                                tile.TileId = Manager.GameData.IdToTileType["shtrs Pure Evil"];
                                p.Value.Owner.Map[i, 170] = tile;

                                tile = p.Value.Owner.Map[i, 172].Clone();
                                tile.TileId = Manager.GameData.IdToTileType["shtrs Pure Evil"];
                                p.Value.Owner.Map[i, 172] = tile;
                            }
                        }

                        Timers.Add(new WorldTimer(5000, (w3, t3) =>
                        {
                            foreach (var p in Players)
                            {
                                for (int i = 138; i < 152; i++)
                                {
                                    tile = p.Value.Owner.Map[i, 171].Clone();
                                    tile.TileId = Manager.GameData.IdToTileType["shtrs Pure Evil"];
                                    p.Value.Owner.Map[i, 171] = tile;
                                }
                            }
                        }));
                    }));
                }));
            }));
        }

        public void OpenBridge1Behind()
        {
            WmapTile tile;

            foreach (var p in Players)
            {
                for (int x = 174; x < 200; x++)
                {
                    for (int y = 162; y < 181; y++)
                    {
                        tile = p.Value.Owner.Map[x, y].Clone();
                        tile.TileId = Manager.GameData.IdToTileType["shtrs Bridge"];
                        p.Value.Owner.Map[x, y] = tile;
                    }
                }
            }

        }
    }
}
#region

using System.Collections.Generic;
using wServer.realm.entities;
using wServer.realm.entities.player;

#endregion

namespace wServer.realm.worlds
{
    public class DailyQuestRoom : World
    {
        public DailyQuestRoom()
        {
            Name = "Daily Quest Room";
            ClientWorldName = "{nexus.Daily_Quest_Room}";
            Background = 0;
            AllowTeleport = false;
            Difficulty = -1;
        }

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.dailyQuest.wmap", MapType.Wmap);
        }

        public override int EnterWorld(Entity entity)
        {
            int ret = base.EnterWorld(entity);
            if (entity is Player)
            {
                Timers.Add(new WorldTimer(2000, (w, t) =>
                {
                    Manager.Database.DoActionAsync(db =>
                    {
                        var q = db.GetDailyQuest((entity as Player).AccountId, Manager.GameData);
                        (entity as Player).Client.SendPacket(new networking.svrPackets.QuestFetchResponsePacket
                        {
                            Tier = q.Tier,
                            Image = q.Image,
                            Goal = q.Goal,
                            Description = q.Description
                        });
                    });
                }));
            }
            return ret;
        }
    }
}
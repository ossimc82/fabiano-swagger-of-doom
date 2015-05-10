#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using wServer.networking.svrPackets;

#endregion

namespace wServer.realm.entities.player
{
    public partial class Player
    {
        private static readonly Dictionary<string, Tuple<int, int, int>> QuestDat =
            new Dictionary<string, Tuple<int, int, int>> //Priority, Min, Max
            {
                {"Scorpion Queen", Tuple.Create(1, 1, 6)},
                {"Bandit Leader", Tuple.Create(1, 1, 6)},
                {"Hobbit Mage", Tuple.Create(3, 3, 8)},
                {"Undead Hobbit Mage", Tuple.Create(3, 3, 8)},
                {"Giant Crab", Tuple.Create(3, 3, 8)},
                {"Desert Werewolf", Tuple.Create(3, 3, 8)},
                {"Sandsman King", Tuple.Create(4, 4, 9)},
                {"Goblin Mage", Tuple.Create(4, 4, 9)},
                {"Elf Wizard", Tuple.Create(4, 4, 9)},
                {"Dwarf King", Tuple.Create(5, 5, 10)},
                {"Swarm", Tuple.Create(6, 6, 11)},
                {"Shambling Sludge", Tuple.Create(6, 6, 11)},
                {"Great Lizard", Tuple.Create(7, 7, 12)},
                {"Wasp Queen", Tuple.Create(8, 7, 1000)},
                {"Horned Drake", Tuple.Create(8, 7, 1000)},
                {"Deathmage", Tuple.Create(5, 6, 11)},
                {"Great Coil Snake", Tuple.Create(6, 6, 12)},
                {"Lich", Tuple.Create(9, 6, 1000)},
                {"Actual Lich", Tuple.Create(9, 7, 1000)},
                {"Ent Ancient", Tuple.Create(10, 7, 1000)},
                {"Actual Ent Ancient", Tuple.Create(10, 7, 1000)},
                {"Oasis Giant", Tuple.Create(11, 8, 1000)},
                {"Phoenix Lord", Tuple.Create(11, 9, 1000)},
                {"Ghost King", Tuple.Create(12, 10, 1000)},
                {"Actual Ghost King", Tuple.Create(12, 10, 1000)},
                {"Cyclops God", Tuple.Create(13, 10, 1000)},
                {"Red Demon", Tuple.Create(15, 15, 1000)},
                {"Lucky Djinn", Tuple.Create(15, 15, 1000)},
                {"Lucky Ent", Tuple.Create(15, 15, 1000)},
                {"Skull Shrine", Tuple.Create(16, 15, 1000)},
                {"Pentaract", Tuple.Create(16, 15, 1000)},
                {"Cube God", Tuple.Create(16, 15, 1000)},
                {"Grand Sphinx", Tuple.Create(16, 15, 1000)},
                {"Lord of the Lost Lands", Tuple.Create(16, 15, 1000)},
                {"Hermit God", Tuple.Create(16, 15, 1000)},
                {"Ghost Ship", Tuple.Create(16, 15, 1000)},
                {"Unknown Giant Golem", Tuple.Create(16, 15, 1000)},
                {"Evil Chicken God", Tuple.Create(20, 1, 1000)},
                {"Bonegrind The Butcher", Tuple.Create(20, 1, 1000)},
                {"Dreadstump the Pirate King", Tuple.Create(20, 1, 1000)},
                {"Arachna the Spider Queen", Tuple.Create(20, 1, 1000)},
                {"Stheno the Snake Queen", Tuple.Create(20, 1, 1000)},
                {"Mixcoatl the Masked God", Tuple.Create(20, 1, 1000)},
                {"Limon the Sprite God", Tuple.Create(20, 1, 1000)},
                {"Septavius the Ghost God", Tuple.Create(20, 1, 1000)},
                {"Davy Jones", Tuple.Create(20, 1, 1000)},
                {"Lord Ruthven", Tuple.Create(20, 1, 1000)},
                {"Archdemon Malphas", Tuple.Create(20, 1, 1000)},
                {"Elder Tree", Tuple.Create(20, 1, 1000)},
                {"Thessal the Mermaid Goddess", Tuple.Create(20, 1, 1000)},
                {"Dr. Terrible", Tuple.Create(20, 1, 1000)},
                {"Horrific Creation", Tuple.Create(20, 1, 1000)},
                {"Masked Party God", Tuple.Create(20, 1, 10000)},
                {"Stone Guardian Left", Tuple.Create(20, 1, 1000)},
                {"Stone Guardian Right", Tuple.Create(20, 1, 1000)},
                {"Oryx the Mad God 1", Tuple.Create(20, 1, 1000)},
                {"Oryx the Mad God 2", Tuple.Create(20, 1, 1000)},
            };

        private Entity questEntity;

        public Entity Quest
        {
            get { return questEntity; }
        }

        private static int GetExpGoal(int level)
        {
            return 50 + (level - 1)*100;
        }

        private static int GetLevelExp(int level)
        {
            if (level == 1) return 0;
            return 50*(level - 1) + (level - 2)*(level - 1)*50;
        }

        private static int GetFameGoal(int fame)
        {
            if (fame >= 2000) return 0;
            if (fame >= 800) return 2000;
            if (fame >= 400) return 800;
            if (fame >= 150) return 400;
            if (fame >= 20) return 150;
            return 0;
        }

        public int GetStars()
        {
            int ret = 0;
            foreach (ClassStats i in Client.Account.Stats.ClassStates)
            {
                if (i.BestFame >= 2000) ret += 5;
                else if (i.BestFame >= 800) ret += 4;
                else if (i.BestFame >= 400) ret += 3;
                else if (i.BestFame >= 150) ret += 2;
                else if (i.BestFame >= 20) ret += 1;
            }
            return ret;
        }

        private float Dist(Entity a, Entity b)
        {
            float dx = a.X - b.X;
            float dy = a.Y - b.Y;
            return (float) Math.Sqrt(dx*dx + dy*dy);
        }

        private Entity FindQuest()
        {
            Entity ret = null;
            try
            {
                float bestScore = 0;
                foreach (Enemy i in Owner.Quests.Values
                    .OrderBy(quest => MathsUtils.DistSqr(quest.X, quest.Y, X, Y)))
                {
                    if (i.ObjectDesc == null || !i.ObjectDesc.Quest) continue;

                    Tuple<int, int, int> x;
                    if (!QuestDat.TryGetValue(i.ObjectDesc.ObjectId, out x)) continue;

                    if ((Level >= x.Item2 && Level <= x.Item3))
                    {
                        float score = (20 - Math.Abs((i.ObjectDesc.Level ?? 0) - Level))*x.Item1 -
                                      //priority * level diff
                                      Dist(this, i)/100; //minus 1 for every 100 tile distance
                        if (score > bestScore)
                        {
                            bestScore = score;
                            ret = i;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return ret;
        }

        private void HandleQuest(RealmTime time)
        {
            if (time.tickCount%500 == 0 || questEntity == null || questEntity.Owner == null)
            {
                Entity newQuest = FindQuest();
                if (newQuest != null && newQuest != questEntity)
                {
                    Owner.Timers.Add(new WorldTimer(100, (w, t) =>
                    {
                        Client.SendPacket(new QuestObjIdPacket
                        {
                            ObjectId = newQuest.Id
                        });
                    }));
                    questEntity = newQuest;
                }
            }
        }

        private void CalculateFame()
        {
            int newFame = 0;
            if (Experience < 200*1000) newFame = Experience/1000;
            else newFame = 200 + (Experience - 200*1000)/1000;
            if (newFame != Fame)
            {
                Fame = newFame;
                int newGoal;
                ClassStats state =
                    Client.Account.Stats.ClassStates.SingleOrDefault(_ => Utils.FromString(_.ObjectType) == ObjectType);
                if (state != null && state.BestFame > Fame)
                    newGoal = GetFameGoal(state.BestFame);
                else
                    newGoal = GetFameGoal(Fame);
                if (newGoal > FameGoal)
                {
                    Owner.BroadcastPacket(new NotificationPacket
                    {
                        ObjectId = Id,
                        Color = new ARGB(0xFF00FF00),
                        Text = "{\"key\":\"blank\",\"tokens\":{\"data\":\"Class Quest Complete!\"}}",
                    }, null);
                    Stars = GetStars();
                }
                FameGoal = newGoal;
                UpdateCount++;
            }
        }

        private bool CheckLevelUp()
        {
            if (Experience - GetLevelExp(Level) >= ExperienceGoal && Level < 20)
            {
                Level++;
                ExperienceGoal = GetExpGoal(Level);
                foreach (XElement i in Manager.GameData.ObjectTypeToElement[ObjectType].Elements("LevelIncrease"))
                {
                    Random rand = new Random();
                    int min = int.Parse(i.Attribute("min").Value);
                    int max = int.Parse(i.Attribute("max").Value) + 1;
                    int limit =
                        int.Parse(
                            Manager.GameData.ObjectTypeToElement[ObjectType].Element(i.Value).Attribute("max").Value);
                    int idx = StatsManager.StatsNameToIndex(i.Value);
                    Stats[idx] += rand.Next(min, max);
                    if (Stats[idx] > limit) Stats[idx] = limit;
                }
                HP = Stats[0] + Boost[0];
                MP = Stats[1] + Boost[1];

                UpdateCount++;

                if (Level == 20)
                {
                    foreach (Player i in Owner.Players.Values)
                        i.SendInfo(Name + " achieved level 20");
                    XpBoosted = false;
                    XpBoostTimeLeft = 0;
                }
                questEntity = null;
                return true;
            }
            CalculateFame();
            return false;
        }

        public bool EnemyKilled(Enemy enemy, int exp, bool killer)
        {
            if (enemy == questEntity)
                Owner.BroadcastPacket(new NotificationPacket
                {
                    ObjectId = Id,
                    Color = new ARGB(0xFF00FF00),
                    Text = "{\"key\":\"blank\",\"tokens\":{\"data\":\"Quest Complete!\"}}",
                }, null);
            if (exp > 0)
            {
                if(XpBoosted)
                    Experience += exp * 2;
                else
                    Experience += exp;
                UpdateCount++;
                foreach (Entity i in Owner.PlayersCollision.HitTest(X, Y, 16))
                {
                    if (i != this)
                    {
                        if (i is Player)
                        {
                            try
                            {
                                (i as Player).Experience += (i as Player).XpBoosted ? exp * 2 : exp;
                                (i as Player).UpdateCount++;
                                (i as Player).CheckLevelUp();
                                if (Random.Next(1, 100000) <= 50)
                                    Client.GiftCodeReceived("LevelUp");
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                            }
                        }
                    }
                }
            }
            FameCounter.Killed(enemy, killer);
            return CheckLevelUp();
        }
    }
}
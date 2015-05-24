using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.networking.svrPackets;
using wServer.realm;
using wServer.realm.entities;
using wServer.realm.entities.player;

namespace wServer.logic.behaviors.PetBehaviors
{
    internal class PetHealMP : Behavior
    {
        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            state = 1000;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            int cool = (int)state;

            if (cool <= 0)
            {
                if (!(host is Pet)) return;
                Pet pet = host as Pet;
                if (pet.PlayerOwner == null) return;
                Player player = host.GetEntity(pet.PlayerOwner.Id) as Player;
                if (player == null) return;

                if (player != null)
                {
                    int maxMp = player.Stats[1] + player.Boost[1];
                    int h = GetMP(pet, ref cool);
                    if (h == -1) return;
                    int newMp = Math.Min(player.MaxMp, player.Mp + h);
                    if (newMp != player.Mp)
                    {
                        int n = newMp - player.Mp;
                        if (player.HasConditionEffect(ConditionEffectIndex.Quiet))
                        {
                            player.Owner.BroadcastPacket(new ShowEffectPacket
                            {
                                EffectType = EffectType.Trail,
                                TargetId = host.Id,
                                PosA = new Position { X = player.X, Y = player.Y },
                                Color = new ARGB(0xffffffff)
                            }, null);
                            player.Owner.BroadcastPacket(new NotificationPacket
                            {
                                ObjectId = player.Id,
                                Text = "{\"key\":\"blank\",\"tokens\":{\"data\":\"No Effect\"}}",
                                Color = new ARGB(0xFF0000)
                            }, null);
                            cool = 1000;
                            state = cool;
                            return;
                        }
                        player.Mp = newMp;
                        player.UpdateCount++;
                        player.Owner.BroadcastPacket(new ShowEffectPacket
                        {
                            EffectType = EffectType.Trail,
                            TargetId = host.Id,
                            PosA = new Position { X = player.X, Y = player.Y },
                            Color = new ARGB(0xffffffff)
                        }, null);
                        player.Owner.BroadcastPacket(new ShowEffectPacket
                        {
                            EffectType = EffectType.Potion,
                            TargetId = player.Id,
                            Color = new ARGB(0x6084e0)
                        }, null);
                        player.Owner.BroadcastPacket(new NotificationPacket
                        {
                            ObjectId = player.Id,
                            Text = "{\"key\":\"blank\",\"tokens\":{\"data\":\"+" + n + "\"}}",
                            Color = new ARGB(0x6084e0)
                        }, null);
                    }
                }
            }
            else
                cool -= time.thisTickTimes;

            state = cool;
        }

        private int GetMP(Pet host, ref int cooldown)
        {
            for (int i = 0; i < 3; i++)
            {
                switch (i)
                {
                    case 0:
                        if (host.FirstPetLevel.Ability == Ability.MagicHeal)
                        {
                            return CalculateMagicHeal(host.FirstPetLevel.Level, ref cooldown);
                        }
                        break;
                    case 1:
                        if (host.SecondPetLevel.Ability == Ability.MagicHeal)
                        {
                            return CalculateMagicHeal(host.SecondPetLevel.Level, ref cooldown);
                        }
                        break;
                    case 2:
                        if (host.ThirdPetLevel.Ability == Ability.MagicHeal)
                        {
                            return CalculateMagicHeal(host.ThirdPetLevel.Level, ref cooldown);
                        }
                        break;
                    default:
                        break;
                }
            }
            return -1;
        }

        private int CalculateMagicHeal(int level, ref int cooldown)
        {
            if (Enumerable.Range(0, 30).Contains(level))
            {
                cooldown = 4000;
                return 3;
            }
            else if (Enumerable.Range(30, 20).Contains(level))
            {
                cooldown = 3000;
                return 8;
            }
            else if (Enumerable.Range(50, 20).Contains(level))
            {
                cooldown = 1500;
                return 17;
            }
            else if (Enumerable.Range(70, 20).Contains(level))
            {
                cooldown = 1000;
                return 33;
            }
            else if (Enumerable.Range(90, 11).Contains(level))
            {
                cooldown = 750;
                return 45;
            }
            throw new Exception("Invalid PetLevel");
            //switch (level)
            //{
            //    case 1:
            //        cooldown = 10000;
            //        return 10;
            //    case 2:
            //        cooldown = 9200;
            //        return 11;
            //    default:
            //        throw new Exception("Invalid PetLevel");
            //}
        }
    }
}

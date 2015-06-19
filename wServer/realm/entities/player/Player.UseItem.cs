#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using db;
using MySql.Data.MySqlClient;
using wServer.networking;
using wServer.networking.cliPackets;
using wServer.networking.svrPackets;

#endregion

namespace wServer.realm.entities.player
{
    partial class Player
    {
        private static readonly ConditionEffect[] NegativeEffs =
        {
            new ConditionEffect
            {
                Effect = ConditionEffectIndex.Slowed,
                DurationMS = 0
            },
            new ConditionEffect
            {
                Effect = ConditionEffectIndex.Paralyzed,
                DurationMS = 0
            },
            new ConditionEffect
            {
                Effect = ConditionEffectIndex.Weak,
                DurationMS = 0
            },
            new ConditionEffect
            {
                Effect = ConditionEffectIndex.Stunned,
                DurationMS = 0
            },
            new ConditionEffect
            {
                Effect = ConditionEffectIndex.Confused,
                DurationMS = 0
            },
            new ConditionEffect
            {
                Effect = ConditionEffectIndex.Blind,
                DurationMS = 0
            },
            new ConditionEffect
            {
                Effect = ConditionEffectIndex.Quiet,
                DurationMS = 0
            },
            new ConditionEffect
            {
                Effect = ConditionEffectIndex.ArmorBroken,
                DurationMS = 0
            },
            new ConditionEffect
            {
                Effect = ConditionEffectIndex.Bleeding,
                DurationMS = 0
            },
            new ConditionEffect
            {
                Effect = ConditionEffectIndex.Dazed,
                DurationMS = 0
            },
            new ConditionEffect
            {
                Effect = ConditionEffectIndex.Sick,
                DurationMS = 0
            },
            new ConditionEffect
            {
                Effect = ConditionEffectIndex.Drunk,
                DurationMS = 0
            },
            new ConditionEffect
            {
                Effect = ConditionEffectIndex.Hallucinating,
                DurationMS = 0
            },
            new ConditionEffect
            {
                Effect = ConditionEffectIndex.Hexed,
                DurationMS = 0
            },
            new ConditionEffect
            {
                Effect = ConditionEffectIndex.Unstable,
                DurationMS = 0
            }
        };

        public static int oldstat { get; set; }

        public static Position targetlink { get; set; }

        public static void ActivateHealHp(Player player, int amount, List<Packet> pkts)
        {
            int maxHp = player.Stats[0] + player.Boost[0];
            int newHp = Math.Min(maxHp, player.HP + amount);
            if (newHp != player.HP)
            {
                pkts.Add(new ShowEffectPacket
                {
                    EffectType = EffectType.Potion,
                    TargetId = player.Id,
                    Color = new ARGB(0xffffffff)
                });
                pkts.Add(new NotificationPacket
                {
                    Color = new ARGB(0xff00ff00),
                    ObjectId = player.Id,
                    Text = "{\"key\":\"blank\",\"tokens\":{\"data\":\"+" + (newHp - player.HP) + "\"}}"
                    //"+" + (newHp - player.HP)
                });
                player.HP = newHp;
                player.UpdateCount++;
            }
        }

        private static void ActivateHealMp(Player player, int amount, List<Packet> pkts)
        {
            int maxMp = player.Stats[1] + player.Boost[1];
            int newMp = Math.Min(maxMp, player.Mp + amount);
            if (newMp != player.Mp)
            {
                pkts.Add(new ShowEffectPacket
                {
                    EffectType = EffectType.Potion,
                    TargetId = player.Id,
                    Color = new ARGB(0x6084e0)
                });
                pkts.Add(new NotificationPacket
                {
                    Color = new ARGB(0x6084e0),
                    ObjectId = player.Id,
                    Text = "{\"key\":\"blank\",\"tokens\":{\"data\":\"+" + (newMp - player.Mp) + "\"}}"
                });
                player.Mp = newMp;
                player.UpdateCount++;
            }
        }

        private static void ActivateBoostStat(Player player, int idxnew, List<Packet> pkts)
        {
            var OriginalStat = 0;
            OriginalStat = player.Stats[idxnew] + OriginalStat;
            oldstat = OriginalStat;
        }

        private void ActivateShoot(RealmTime time, Item item, Position target)
        {
            double arcGap = item.ArcGap*Math.PI/180;
            double startAngle = Math.Atan2(target.Y - Y, target.X - X) - (item.NumProjectiles - 1)/2*arcGap;
            ProjectileDesc prjDesc = item.Projectiles[0]; //Assume only one

            for (int i = 0; i < item.NumProjectiles; i++)
            {
                Projectile proj = CreateProjectile(prjDesc, item.ObjectType,
                    (int) StatsManager.GetAttackDamage(prjDesc.MinDamage, prjDesc.MaxDamage),
                    time.tickTimes, new Position {X = X, Y = Y}, (float) (startAngle + arcGap*i));
                Owner.EnterWorld(proj);
                FameCounter.Shoot(proj);
            }
        }

        private void PoisonEnemy(Enemy enemy, ActivateEffect eff)
        {
            try
            {
                if (eff.ConditionEffect != null)
                    enemy.ApplyConditionEffect(new[]
                    {
                        new ConditionEffect
                        {
                            Effect = (ConditionEffectIndex) eff.ConditionEffect,
                            DurationMS = (int) eff.EffectDuration
                        }
                    });
                int remainingDmg = (int) StatsManager.GetDefenseDamage(enemy, eff.TotalDamage, enemy.ObjectDesc.Defense);
                int perDmg = remainingDmg*1000/eff.DurationMS;
                WorldTimer tmr = null;
                int x = 0;
                tmr = new WorldTimer(100, (w, t) =>
                {
                    if (enemy.Owner == null) return;
                    w.BroadcastPacket(new ShowEffectPacket
                    {
                        EffectType = EffectType.Dead,
                        TargetId = enemy.Id,
                        Color = new ARGB(0xffddff00)
                    }, null);

                    if (x%10 == 0)
                    {
                        int thisDmg;
                        if (remainingDmg < perDmg) thisDmg = remainingDmg;
                        else thisDmg = perDmg;

                        enemy.Damage(this, t, thisDmg, true);
                        remainingDmg -= thisDmg;
                        if (remainingDmg <= 0) return;
                    }
                    x++;

                    tmr.Reset();

                    Manager.Logic.AddPendingAction(_ => w.Timers.Add(tmr), PendingPriority.Creation);
                });
                Owner.Timers.Add(tmr);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        public bool Activate(RealmTime time, Item item, UseItemPacket pkt)
        {
            bool endMethod = false;
            Position target = pkt.ItemUsePos;
            Mp -= item.MpCost;

            IContainer con = Owner.GetEntity(pkt.SlotObject.ObjectId) as IContainer;
            if(con == null) return true;

            if (pkt.SlotObject.SlotId != 255 && pkt.SlotObject.SlotId != 254 && con.Inventory[pkt.SlotObject.SlotId] != item)
            {
                log.FatalFormat("Cheat engine detected for player {0},\nItem should be {1}, but its {2}.",
                    Name, Inventory[pkt.SlotObject.SlotId].ObjectId, item.ObjectId);
                foreach (Player player in Owner.Players.Values)
                    if (player.Client.Account.Rank >= 2)
                        player.SendInfo(String.Format("Cheat engine detected for player {0},\nItem should be {1}, but its {2}.",
                    Name, Inventory[pkt.SlotObject.SlotId].ObjectId, item.ObjectId));
                Client.Disconnect();
                return true;
            }

            if (item.IsBackpack)
            {
                if (HasBackpack) return true;
                Client.Character.Backpack = new [] {-1, -1, -1, -1, -1, -1, -1, -1};
                HasBackpack = true;
                Client.Character.HasBackpack = 1;
                Manager.Database.DoActionAsync(db =>
                    db.SaveBackpacks(Client.Character, Client.Account));
                Array.Resize(ref inventory, 20);
                int[] slotTypes =
                    Utils.FromCommaSepString32(
                        Manager.GameData.ObjectTypeToElement[ObjectType].Element("SlotTypes").Value);
                Array.Resize(ref slotTypes, 20);
                for (int i = 0; i < slotTypes.Length; i++)
                    if (slotTypes[i] == 0) slotTypes[i] = 10;
                SlotTypes = slotTypes;
                return false;
            }
            if (item.XpBooster)
            {
                if (!XpBoosted)
                {
                    XpBoostTimeLeft = (float)item.Timer;
                    XpBoosted = item.XpBooster;
                    xpFreeTimer = (float)item.Timer == -1.0 ? false : true;
                    return false;
                }
                else
                {
                    SendInfo("You have already an active XP Booster.");
                    return true;
                }
            }

            if (item.LootDropBooster)
            {
                if (!LootDropBoost)
                {
                    LootDropBoostTimeLeft = (float)item.Timer;
                    lootDropBoostFreeTimer = (float)item.Timer == -1.0 ? false : true;
                    return false;
                }
                else
                {
                    SendInfo("You have already an active Loot Drop Booster.");
                    return true;
                }
            }

            if (item.LootTierBooster)
            {
                if (!LootTierBoost)
                {
                    LootTierBoostTimeLeft = (float)item.Timer;
                    lootTierBoostFreeTimer = (float)item.Timer == -1.0 ? false : true;
                    return false;
                }
                else
                {
                    SendInfo("You have already an active Loot Tier Booster.");
                    return true;
                }
            }

            foreach (ActivateEffect eff in item.ActivateEffects)
            {
                switch (eff.Effect)
                {
                    case ActivateEffects.BulletNova:
                    {
                        ProjectileDesc prjDesc = item.Projectiles[0]; //Assume only one
                        Packet[] batch = new Packet[21];
                        uint s = Random.CurrentSeed;
                        Random.CurrentSeed = (uint)(s * time.tickTimes);
                        for (int i = 0; i < 20; i++)
                        {
                            Projectile proj = CreateProjectile(prjDesc, item.ObjectType,
                                (int)StatsManager.GetAttackDamage(prjDesc.MinDamage, prjDesc.MaxDamage),
                                time.tickTimes, target, (float)(i * (Math.PI * 2) / 20));
                            Owner.EnterWorld(proj);
                            FameCounter.Shoot(proj);
                            batch[i] = new Shoot2Packet()
                            {
                                BulletId = proj.ProjectileId,
                                OwnerId = Id,
                                ContainerType = item.ObjectType,
                                StartingPos = target,
                                Angle = proj.Angle,
                                Damage = (short)proj.Damage
                            };
                        }
                        Random.CurrentSeed = s;
                        batch[20] = new ShowEffectPacket()
                        {
                            EffectType = EffectType.Trail,
                            PosA = target,
                            TargetId = Id,
                            Color = new ARGB(0xFFFF00AA)
                        };
                        BroadcastSync(batch, p => this.Dist(p) < 35);
                    } break;
                    case ActivateEffects.Shoot:
                    {
                        ActivateShoot(time, item, target);
                    }
                        break;

                    case ActivateEffects.StatBoostSelf:
                    {
                        int idx = -1;

                        if (eff.Stats == StatsType.MaximumHP) idx = 0;
                        else if (eff.Stats == StatsType.MaximumMP) idx = 1;
                        else if (eff.Stats == StatsType.Attack) idx = 2;
                        else if (eff.Stats == StatsType.Defense) idx = 3;
                        else if (eff.Stats == StatsType.Speed) idx = 4;
                        else if (eff.Stats == StatsType.Vitality) idx = 5;
                        else if (eff.Stats == StatsType.Wisdom) idx = 6;
                        else if (eff.Stats == StatsType.Dexterity) idx = 7;

                        List<Packet> pkts = new List<Packet>();

                        ActivateBoostStat(this, idx, pkts);
                        int OGstat = oldstat;
                        int bit = idx + 39;

                        int s = eff.Amount;
                        Boost[idx] += s;
                        ApplyConditionEffect(new ConditionEffect
                        {
                            DurationMS = eff.DurationMS,
                            Effect = (ConditionEffectIndex)bit
                        });
                        UpdateCount++;
                        Owner.Timers.Add(new WorldTimer(eff.DurationMS, (world, t) =>
                        {
                            Boost[idx] = OGstat;
                            UpdateCount++;
                        }));
                        Owner.BroadcastPacket(new ShowEffectPacket
                        {
                            EffectType = EffectType.Potion,
                            TargetId = Id,
                            Color = new ARGB(0xffffffff)
                        }, null);
                    }
                        break;

                    case ActivateEffects.StatBoostAura:
                        {
                            int idx = -1;

                            if(eff.Stats == StatsType.MaximumHP) idx = 0;
                            if(eff.Stats == StatsType.MaximumMP) idx = 1;
                            if(eff.Stats == StatsType.Attack) idx = 2;
                            if(eff.Stats == StatsType.Defense) idx = 3;
                            if(eff.Stats == StatsType.Speed) idx = 4;
                            if(eff.Stats == StatsType.Vitality) idx = 5;
                            if(eff.Stats == StatsType.Wisdom) idx = 6;
                            if(eff.Stats == StatsType.Dexterity) idx = 7;
                            
                            int bit = idx + 39;

                            var amountSBA = eff.Amount;
                            var durationSBA = eff.DurationMS;
                            var rangeSBA = eff.Range;
                            if (eff.UseWisMod)
                            {
                                amountSBA = (int)UseWisMod(eff.Amount, 0);
                                durationSBA = (int)(UseWisMod(eff.DurationSec) * 1000);
                                rangeSBA = UseWisMod(eff.Range);
                            }
                            
                            this.Aoe(rangeSBA, true, player =>
                            {
                                // TODO support for noStack StatBoostAura attribute (paladin total hp increase / insta heal)
                                ApplyConditionEffect(new ConditionEffect
                                {
                                    DurationMS = durationSBA,
                                    Effect = (ConditionEffectIndex)bit
                                });
                                (player as Player).Boost[idx] += amountSBA;
                                player.UpdateCount++;
                                Owner.Timers.Add(new WorldTimer(durationSBA, (world, t) =>
                                {
                                    (player as Player).Boost[idx] -= amountSBA;
                                    player.UpdateCount++;
                                }));
                            });
                            BroadcastSync(new ShowEffectPacket()
                            {
                                EffectType = EffectType.AreaBlast,
                                TargetId = Id,
                                Color = new ARGB(0xffffffff),
                                PosA = new Position() { X = rangeSBA }
                            }, p => this.Dist(p) < 25);
                        } break;

                    case ActivateEffects.ConditionEffectSelf:
                    {
                        var durationCES = eff.DurationMS;
                        if (eff.UseWisMod)
                            durationCES = (int) (UseWisMod(eff.DurationSec)*1000);

                        var color = 0xffffffff;
                        switch (eff.ConditionEffect.Value)
                        {
                            case ConditionEffectIndex.Damaging:
                                color = 0xffff0000;
                                break;
                            case ConditionEffectIndex.Berserk:
                                color = 0x808080;
                                break;
                        }

                        ApplyConditionEffect(new ConditionEffect
                        {
                            Effect = eff.ConditionEffect.Value,
                            DurationMS = durationCES
                        });
                        Owner.BroadcastPacket(new ShowEffectPacket
                        {
                            EffectType = EffectType.AreaBlast,
                            TargetId = Id,
                            Color = new ARGB(color),
                            PosA = new Position {X = 2F}
                        }, null);
                    }
                        break;

                    case ActivateEffects.ConditionEffectAura:
                    {
                        var durationCEA = eff.DurationMS;
                        var rangeCEA = eff.Range;
                        if (eff.UseWisMod)
                        {
                            durationCEA = (int)(UseWisMod(eff.DurationSec) * 1000);
                            rangeCEA = UseWisMod(eff.Range);
                        }

                        this.Aoe(rangeCEA, true, player =>
                        {
                            player.ApplyConditionEffect(new ConditionEffect
                            {
                                Effect = eff.ConditionEffect.Value,
                                DurationMS = durationCEA
                            });
                        });

                        var color = 0xffffffff;
                        switch (eff.ConditionEffect.Value)
                        {
                            case ConditionEffectIndex.Damaging:
                                color = 0xffff0000;
                                break;
                            case ConditionEffectIndex.Berserk:
                                color = 0x808080;
                                break;
                        }

                        BroadcastSync(new ShowEffectPacket
                        {
                            EffectType = EffectType.AreaBlast,
                            TargetId = Id,
                            Color = new ARGB(color),
                            PosA = new Position {X = rangeCEA}
                        }, p => this.Dist(p) < 25);
                    }
                        break;

                    case ActivateEffects.Heal:
                    {
                        List<Packet> pkts = new List<Packet>();
                        ActivateHealHp(this, eff.Amount, pkts);
                        Owner.BroadcastPackets(pkts, null);
                    }
                        break;

                    case ActivateEffects.HealNova:
                    {
                        var amountHN = eff.Amount;
                        var rangeHN = eff.Range;
                        if (eff.UseWisMod)
                        {
                            amountHN = (int)UseWisMod(eff.Amount, 0);
                            rangeHN = UseWisMod(eff.Range);
                        }
                        
                        List<Packet> pkts = new List<Packet>();
                        this.Aoe(rangeHN, true, player => { ActivateHealHp(player as Player, amountHN, pkts); });
                        pkts.Add(new ShowEffectPacket
                        {
                            EffectType = EffectType.AreaBlast,
                            TargetId = Id,
                            Color = new ARGB(0xffffffff),
                            PosA = new Position {X = rangeHN}
                        });
                        BroadcastSync(pkts, p => this.Dist(p) < 25);
                    }
                        break;

                    case ActivateEffects.Magic:
                    {
                        List<Packet> pkts = new List<Packet>();
                        ActivateHealMp(this, eff.Amount, pkts);
                        Owner.BroadcastPackets(pkts, null);
                    }
                        break;

                    case ActivateEffects.MagicNova:
                    {
                        List<Packet> pkts = new List<Packet>();
                        this.Aoe(eff.Range/2, true, player => { ActivateHealMp(player as Player, eff.Amount, pkts); });
                        pkts.Add(new ShowEffectPacket
                        {
                            EffectType = EffectType.AreaBlast,
                            TargetId = Id,
                            Color = new ARGB(0xffffffff),
                            PosA = new Position {X = eff.Range}
                        });
                        Owner.BroadcastPackets(pkts, null);
                    }
                        break;

                    case ActivateEffects.Teleport:
                    {
                        Move(target.X, target.Y);
                        UpdateCount++;
                        Owner.BroadcastPackets(new Packet[]
                        {
                            new GotoPacket
                            {
                                ObjectId = Id,
                                Position = new Position
                                {
                                    X = X,
                                    Y = Y
                                }
                            },
                            new ShowEffectPacket
                            {
                                EffectType = EffectType.Teleport,
                                TargetId = Id,
                                PosA = new Position
                                {
                                    X = X,
                                    Y = Y
                                },
                                Color = new ARGB(0xFFFFFFFF)
                            }
                        }, null);
                    }
                        break;

                    case ActivateEffects.VampireBlast:
                    {
                        List<Packet> pkts = new List<Packet>();
                        pkts.Add(new ShowEffectPacket
                        {
                            EffectType = EffectType.Trail,
                            TargetId = Id,
                            PosA = target,
                            Color = new ARGB(0xFFFF0000)
                        });
                        pkts.Add(new ShowEffectPacket
                        {
                            EffectType = EffectType.Diffuse,
                            Color = new ARGB(0xFFFF0000),
                            TargetId = Id,
                            PosA = target,
                            PosB = new Position {X = target.X + eff.Radius, Y = target.Y}
                        });

                        int totalDmg = 0;
                        List<Enemy> enemies = new List<Enemy>();
                        Owner.Aoe(target, eff.Radius, false, enemy =>
                        {
                            enemies.Add(enemy as Enemy);
                            totalDmg += (enemy as Enemy).Damage(this, time, eff.TotalDamage, false);
                        });
                        List<Player> players = new List<Player>();
                        this.Aoe(eff.Radius, true, player =>
                        {
                            players.Add(player as Player);
                            ActivateHealHp(player as Player, totalDmg, pkts);
                        });

                        if (enemies.Count > 0)
                        {
                            Random rand = new Random();
                            for (int i = 0; i < 5; i++)
                            {
                                Enemy a = enemies[rand.Next(0, enemies.Count)];
                                Player b = players[rand.Next(0, players.Count)];
                                pkts.Add(new ShowEffectPacket
                                {
                                    EffectType = EffectType.Flow,
                                    TargetId = b.Id,
                                    PosA = new Position {X = a.X, Y = a.Y},
                                    Color = new ARGB(0xffffffff)
                                });
                            }
                        }

                        BroadcastSync(pkts, p => this.Dist(p) < 25);
                    }
                        break;
                    case ActivateEffects.Trap:
                    {
                        BroadcastSync(new ShowEffectPacket
                        {
                            EffectType = EffectType.Throw,
                            Color = new ARGB(0xff9000ff),
                            TargetId = Id,
                            PosA = target
                        }, p => this.Dist(p) < 25);
                        Owner.Timers.Add(new WorldTimer(1500, (world, t) =>
                        {
                            Trap trap = new Trap(
                                this,
                                eff.Radius,
                                eff.TotalDamage,
                                eff.ConditionEffect ?? ConditionEffectIndex.Slowed,
                                eff.EffectDuration);
                            trap.Move(target.X, target.Y);
                            world.EnterWorld(trap);
                        }));
                    }
                        break;

                    case ActivateEffects.StasisBlast:
                    {
                        List<Packet> pkts = new List<Packet>();

                        pkts.Add(new ShowEffectPacket
                        {
                            EffectType = EffectType.Concentrate,
                            TargetId = Id,
                            PosA = target,
                            PosB = new Position {X = target.X + 3, Y = target.Y},
                            Color = new ARGB(0xFF00D0)
                        });
                        Owner.Aoe(target, 3, false, enemy =>
                        {
                            if (IsSpecial(enemy.ObjectType)) return;

                            if (enemy.HasConditionEffect(ConditionEffectIndex.StasisImmune))
                            {
                                if (!enemy.HasConditionEffect(ConditionEffectIndex.Invincible))
                                {
                                    pkts.Add(new NotificationPacket
                                    {
                                        ObjectId = enemy.Id,
                                        Color = new ARGB(0xff00ff00),
                                        Text = "{\"key\":\"blank\",\"tokens\":{\"data\":\"Immune\"}}"
                                    });
                                }
                            }
                            else if (!enemy.HasConditionEffect(ConditionEffectIndex.Stasis))
                            {
                                enemy.ApplyConditionEffect(new ConditionEffect
                                {
                                    Effect = ConditionEffectIndex.Stasis,
                                    DurationMS = eff.DurationMS
                                });
                                Owner.Timers.Add(new WorldTimer(eff.DurationMS, (world, t) =>
                                {
                                    enemy.ApplyConditionEffect(new ConditionEffect
                                    {
                                        Effect = ConditionEffectIndex.StasisImmune,
                                        DurationMS = 3000
                                    });
                                }));
                                pkts.Add(new NotificationPacket
                                {
                                    ObjectId = enemy.Id,
                                    Color = new ARGB(0xffff0000),
                                    Text = "{\"key\":\"blank\",\"tokens\":{\"data\":\"Stasis\"}}"
                                });
                            }
                        });
                        BroadcastSync(pkts, p => this.Dist(p) < 25);
                    }
                        break;

                    case ActivateEffects.Decoy:
                    {
                        Decoy decoy = new Decoy(Manager, this, eff.DurationMS, StatsManager.GetSpeed());
                        decoy.Move(X, Y);
                        Owner.EnterWorld(decoy);
                    }
                        break;

                    case ActivateEffects.Lightning:
                    {
                        Enemy start = null;
                        double angle = Math.Atan2(target.Y - Y, target.X - X);
                        double diff = Math.PI/3;
                        Owner.Aoe(target, 6, false, enemy =>
                        {
                            if (!(enemy is Enemy)) return;
                            double x = Math.Atan2(enemy.Y - Y, enemy.X - X);
                            if (Math.Abs(angle - x) < diff)
                            {
                                start = enemy as Enemy;
                                diff = Math.Abs(angle - x);
                            }
                        });
                        if (start == null)
                            break;

                        Enemy current = start;
                        Enemy[] targets = new Enemy[eff.MaxTargets];
                        for (int i = 0; i < targets.Length; i++)
                        {
                            targets[i] = current;
                            Enemy next = current.GetNearestEntity(8, false,
                                enemy =>
                                    enemy is Enemy &&
                                    Array.IndexOf(targets, enemy) == -1 &&
                                    this.Dist(enemy) <= 6) as Enemy;

                            if (next == null) break;
                            current = next;
                        }

                        List<Packet> pkts = new List<Packet>();
                        for (int i = 0; i < targets.Length; i++)
                        {
                            if (targets[i] == null) break;
                            if(targets[i].HasConditionEffect(ConditionEffectIndex.Invincible)) continue;
                            Entity prev = i == 0 ? (Entity) this : targets[i - 1];
                            targets[i].Damage(this, time, eff.TotalDamage, false);
                            if (eff.ConditionEffect != null)
                                targets[i].ApplyConditionEffect(new ConditionEffect
                                {
                                    Effect = eff.ConditionEffect.Value,
                                    DurationMS = (int) (eff.EffectDuration*1000)
                                });
                            pkts.Add(new ShowEffectPacket
                            {
                                EffectType = EffectType.Lightning,
                                TargetId = prev.Id,
                                Color = new ARGB(0xffff0088),
                                PosA = new Position
                                {
                                    X = targets[i].X,
                                    Y = targets[i].Y
                                },
                                PosB = new Position {X = 350}
                            });
                        }
                        BroadcastSync(pkts, p => this.Dist(p) < 25);
                    }
                        break;

                    case ActivateEffects.PoisonGrenade:
                    {
                        try
                        {
                            BroadcastSync(new ShowEffectPacket
                            {
                                EffectType = EffectType.Throw,
                                Color = new ARGB(0xffddff00),
                                TargetId = Id,
                                PosA = target
                            }, p => this.Dist(p) < 25);
                            Placeholder x = new Placeholder(Manager, 1500);
                            x.Move(target.X, target.Y);
                            Owner.EnterWorld(x);
                            try
                            {
                                Owner.Timers.Add(new WorldTimer(1500, (world, t) =>
                                {
                                    world.BroadcastPacket(new ShowEffectPacket
                                    {
                                        EffectType = EffectType.AreaBlast,
                                        Color = new ARGB(0xffddff00),
                                        TargetId = x.Id,
                                        PosA = new Position { X = eff.Radius }
                                    }, null);
                                    world.Aoe(target, eff.Radius, false,
                                        enemy => PoisonEnemy(enemy as Enemy, eff));
                                }));
                            }
                            catch (Exception ex)
                            {
                                log.ErrorFormat("Poison ShowEffect:\n{0}", ex);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.ErrorFormat("Poisons General:\n{0}", ex);
                        }
                    }
                        break;
                    case ActivateEffects.RemoveNegativeConditions:
                    {
                        this.Aoe(eff.Range/2, true, player => { ApplyConditionEffect(NegativeEffs); });
                        BroadcastSync(new ShowEffectPacket
                        {
                            EffectType = EffectType.AreaBlast,
                            TargetId = Id,
                            Color = new ARGB(0xffffffff),
                            PosA = new Position {X = eff.Range/2}
                        }, p => this.Dist(p) < 25);
                    }
                        break;

                    case ActivateEffects.RemoveNegativeConditionsSelf:
                    {
                        ApplyConditionEffect(NegativeEffs);
                        Owner.BroadcastPacket(new ShowEffectPacket
                        {
                            EffectType = EffectType.AreaBlast,
                            TargetId = Id,
                            Color = new ARGB(0xffffffff),
                            PosA = new Position {X = 1}
                        }, null);
                    }
                        break;

                    case ActivateEffects.IncrementStat:
                    {
                        int idx = -1;

                        if (eff.Stats == StatsType.MaximumHP) idx = 0;
                        else if (eff.Stats == StatsType.MaximumMP) idx = 1;
                        else if (eff.Stats == StatsType.Attack) idx = 2;
                        else if (eff.Stats == StatsType.Defense) idx = 3;
                        else if (eff.Stats == StatsType.Speed) idx = 4;
                        else if (eff.Stats == StatsType.Vitality) idx = 5;
                        else if (eff.Stats == StatsType.Wisdom) idx = 6;
                        else if (eff.Stats == StatsType.Dexterity) idx = 7;

                        Stats[idx] += eff.Amount;
                        int limit =
                            int.Parse(
                                Manager.GameData.ObjectTypeToElement[ObjectType].Element(
                                    StatsManager.StatsIndexToName(idx))
                                    .Attribute("max")
                                    .Value);
                        if (Stats[idx] > limit)
                            Stats[idx] = limit;
                        UpdateCount++;
                    }
                        break;

                    case ActivateEffects.UnlockPortal:

                        Portal portal = this.GetNearestEntity(5, Manager.GameData.IdToObjectType[eff.LockedName]) as Portal;

                        Packet[] packets = new Packet[3];
                        packets[0] = new ShowEffectPacket
                        {
                            EffectType = EffectType.AreaBlast,
                            Color = new ARGB(0xFFFFFF),
                            PosA = new Position { X = 5 },
                            TargetId = Id
                        };
                        if (portal == null) break;

                        portal.Unlock(eff.DungeonName);

                        packets[1] = new NotificationPacket
                        {
                            Color = new ARGB(0x00FF00),
                            Text =
                                "{\"key\":\"blank\",\"tokens\":{\"data\":\"Unlocked by " +
                                Name + "\"}}",
                            ObjectId = Id
                        };

                        packets[2] = new TextPacket
                        {
                            BubbleTime = 0,
                            Stars = -1,
                            Name = "",
                            Text = eff.DungeonName + " Unlocked by " + Name + "."
                        };

                        BroadcastSync(packets);

                        break;
                    case ActivateEffects.Create: //this is a portal
                    {
                        ushort objType;
                        if (!Manager.GameData.IdToObjectType.TryGetValue(eff.Id, out objType) ||
                            !Manager.GameData.Portals.ContainsKey(objType))
                            break; // object not found, ignore
                        Entity entity = Resolve(Manager, objType);
                        World w = Manager.GetWorld(Owner.Id); //can't use Owner here, as it goes out of scope
                        int TimeoutTime = Manager.GameData.Portals[objType].TimeoutTime;
                        string DungName = Manager.GameData.Portals[objType].DungeonName;

                        ARGB c = new ARGB(0x00FF00);


                        entity.Move(X, Y);
                        w.EnterWorld(entity);

                        w.BroadcastPacket(new NotificationPacket
                        {
                            Color = c,
                            Text =
                                "{\"key\":\"blank\",\"tokens\":{\"data\":\"" + DungName + " opened by " +
                                Client.Account.Name + "\"}}",
                            ObjectId = Client.Player.Id
                        }, null);

                        w.BroadcastPacket(new TextPacket
                        {
                            BubbleTime = 0,
                            Stars = -1,
                            Name = "",
                            Text = DungName + " opened by " + Client.Account.Name
                        }, null);
                        w.Timers.Add(new WorldTimer(TimeoutTime*1000,
                            (world, t) => //default portal close time * 1000
                            {
                                try
                                {
                                    w.LeaveWorld(entity);
                                }
                                catch (Exception ex)
                                    //couldn't remove portal, Owner became null. Should be fixed with RealmManager implementation
                                {
                                    log.ErrorFormat("Couldn't despawn portal.\n{0}", ex);
                                }
                            }));
                    }
                        break;

                    case ActivateEffects.Dye:
                    {
                        if (item.Texture1 != 0)
                        {
                            Texture1 = item.Texture1;
                        }
                        if (item.Texture2 != 0)
                        {
                            Texture2 = item.Texture2;
                        }
                        SaveToCharacter();
                    }
                        break;

                    case ActivateEffects.ShurikenAbility:
                        {
                            if (!ninjaShoot)
                            {
                                ApplyConditionEffect(new ConditionEffect
                                {
                                    Effect = ConditionEffectIndex.Speedy,
                                    DurationMS = -1
                                });
                                ninjaFreeTimer = true;
                                ninjaShoot = true;
                            }
                            else
                            {
                                ApplyConditionEffect(new ConditionEffect
                                {
                                    Effect = ConditionEffectIndex.Speedy,
                                    DurationMS = 0
                                });
                                ushort obj;
                                Manager.GameData.IdToObjectType.TryGetValue(item.ObjectId, out obj);
                                if (Mp >= item.MpEndCost)
                                {
                                    ActivateShoot(time, item, pkt.ItemUsePos);
                                    Mp -= (int)item.MpEndCost;
                                }
                                targetlink = target;
                                ninjaShoot = false;
                            }
                        }
                        break;

                    case ActivateEffects.UnlockSkin:
                        if (!Client.Account.OwnedSkins.Contains(item.ActivateEffects[0].SkinType))
                        {
                            Manager.Database.DoActionAsync(db =>
                            {
                                Client.Account.OwnedSkins.Add(item.ActivateEffects[0].SkinType);
                                MySqlCommand cmd = db.CreateQuery();
                                cmd.CommandText = "UPDATE accounts SET ownedSkins=@ownedSkins WHERE id=@id";
                                cmd.Parameters.AddWithValue("@ownedSkins",
                                    Utils.GetCommaSepString(Client.Account.OwnedSkins.ToArray()));
                                cmd.Parameters.AddWithValue("@id", AccountId);
                                cmd.ExecuteNonQuery();
                                SendInfo(
                                    "New skin unlocked successfully. Change skins in your Vault, or start a new character to use.");
                                Client.SendPacket(new UnlockedSkinPacket
                                {
                                    SkinID = item.ActivateEffects[0].SkinType
                                });
                            });
                            endMethod = false;
                            break;
                        }
                        SendInfo("Error.alreadyOwnsSkin");
                        endMethod = true;
                        break;

                    case ActivateEffects.PermaPet: //Doesnt exist anymore
                    {
                        //psr.Character.Pet = XmlDatas.IdToType[eff.ObjectId];
                        //GivePet(XmlDatas.IdToType[eff.ObjectId]);
                        //UpdateCount++;
                    }
                        break;

                    case ActivateEffects.Pet:
                        Entity en = Entity.Resolve(Manager, eff.ObjectId);
                        en.Move(X, Y);
                        en.SetPlayerOwner(this);
                        Owner.EnterWorld(en);
                        Owner.Timers.Add(new WorldTimer(30 * 1000, (w, t) =>
                        {
                            w.LeaveWorld(en);
                        }));
                        break;

                    case ActivateEffects.CreatePet:
                        if (!Owner.Name.StartsWith("Pet Yard"))
                        {
                            SendInfo("server.use_in_petyard");
                            return true;
                        }
                        Pet.Create(Manager, this, item);
                        break;
                    case ActivateEffects.MysteryPortal:
                        string[] dungeons = new []
                        {
                            "Pirate Cave Portal",
                            "Forest Maze Portal",
                            "Spider Den Portal",
                            "Snake Pit Portal",
                            "Glowing Portal",
                            "Forbidden Jungle Portal",
                            "Candyland Portal",
                            "Haunted Cemetery Portal",
                            "Undead Lair Portal",
                            "Davy Jones' Locker Portal",
                            "Manor of the Immortals Portal",
                            "Abyss of Demons Portal",
                            "Lair of Draconis Portal",
                            "Mad Lab Portal",
                            "Ocean Trench Portal",
                            "Tomb of the Ancients Portal",
                            "Beachzone Portal",
                            "The Shatters",
                            "Deadwater Docks",
                            "Woodland Labyrinth",
                            "The Crawling Depths",
                            "Treasure Cave Portal",
                            "Battle Nexus Portal",
                            "Belladonna's Garden Portal",
                            "Lair of Shaitan Portal"
                        };

                        var descs = Manager.GameData.Portals.Where(_ => dungeons.Contains<string>(_.Value.ObjectId)).Select(_ => _.Value).ToArray();
                        var portalDesc = descs[Random.Next(0, descs.Count())];
                        Entity por = Entity.Resolve(Manager, portalDesc.ObjectId);
                        por.Move(this.X, this.Y);
                        Owner.EnterWorld(por);

                        Client.SendPacket(new NotificationPacket
                        {
                            Color = new ARGB(0x00FF00),
                            Text =
                                "{\"key\":\"blank\",\"tokens\":{\"data\":\"" + portalDesc.DungeonName + " opened by " +
                                Client.Account.Name + "\"}}",
                            ObjectId = Client.Player.Id
                        });

                        Owner.BroadcastPacket(new TextPacket
                        {
                            BubbleTime = 0,
                            Stars = -1,
                            Name = "",
                            Text = portalDesc.ObjectId + " opened by " + Name
                        }, null);

                        Owner.Timers.Add(new WorldTimer(portalDesc.TimeoutTime * 1000, (w, t) => //default portal close time * 1000
                        {
                            try
                            {
                                w.LeaveWorld(por);
                            }
                            catch (Exception ex)
                            {
                                log.ErrorFormat("Couldn't despawn portal.\n{0}", ex);
                            }
                        }));
                        break;
                    case ActivateEffects.GenericActivate:
                        var targetPlayer = eff.Target.Equals("player");
                        var centerPlayer = eff.Center.Equals("player");
                        var duration = (eff.UseWisMod) ?
                            (int)(UseWisMod(eff.DurationSec) * 1000) :
                            eff.DurationMS;
                        var range = (eff.UseWisMod)
                            ? UseWisMod(eff.Range)
                            : eff.Range;
                        
                        Owner.Aoe((eff.Center.Equals("mouse")) ? target : new Position { X = X, Y = Y }, range, targetPlayer, entity =>
                        {
                            if (IsSpecial(entity.ObjectType)) return;
                            if (!entity.HasConditionEffect(ConditionEffectIndex.Stasis) &&
                                !entity.HasConditionEffect(ConditionEffectIndex.Invincible))
                            {
                                entity.ApplyConditionEffect(
                                new ConditionEffect()
                                {
                                    Effect = eff.ConditionEffect.Value,
                                    DurationMS = duration
                                });
                            }
                        });

                        // replaced this last bit with what I had, never noticed any issue with it. Perhaps I'm wrong?
                        BroadcastSync(new ShowEffectPacket()
                        {
                            EffectType = (EffectType)eff.VisualEffect,
                            TargetId = Id,
                            Color = new ARGB(eff.Color ?? 0xffffffff),
                            PosA = centerPlayer ? new Position { X = range } : target,
                            PosB = new Position(target.X - range, target.Y) //Its the range of the diffuse effect
                        }, p => this.DistSqr(p) < 25);
                        break;
                }
            }
            UpdateCount++;
            return endMethod;
        }

        private float UseWisMod(float value, int offset = 1)
        {
            double totalWisdom = Stats[6] + 2 * Boost[6];

            if (totalWisdom < 30)
                return value;

            double m = (value < 0) ? -1 : 1;
            double n = (value * totalWisdom / 150) + (value * m);
            n = Math.Floor(n * Math.Pow(10, offset)) / Math.Pow(10, offset);
            if (n - (int)n * m >= 1 / Math.Pow(10, offset) * m)
            {
                return ((int)(n * 10)) / 10.0f;
            }

            return (int)n;
        }

        private static bool IsSpecial(ushort objType)
        {
            return objType == 0x750d || objType == 0x750e || objType == 0x222c || objType == 0x222d;
        }
    }
}
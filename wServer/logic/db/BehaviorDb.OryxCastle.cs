using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ OryxCastle = () => Behav()
            .Init("Oryx Stone Guardian Right",
                new State(
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new PlayerWithinTransition(2, "Order")
                    ),
                    new State("Order",
                        new Order(10, "Oryx Stone Guardian Left", "Start"),
                        new TimedTransition(0, "Start")
                    ),
                    new State("Start",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xC0C0C0, 0.5, 3),
                        new TimedTransition(1500, "Lets go")
                    ),
                    new State("Together is better",
                        new EntityNotExistsTransition("Oryx Stone Guardian Left", 100, "Forever Alone"),
                        new State("Lets go",
                            new TimedTransition(10000, "Circle"),
                            new State("Imma Follow",
                                new Follow(1, 2, 0.3),
                                new Shoot(5, 5, shootAngle: 5, coolDown: 1000),
                                new TimedTransition(5000, "Imma chill")
                            ),
                            new State("Imma chill",
                                new Prioritize(
                                    new StayCloseToSpawn(0.5, 3),
                                    new Wander(0.5)
                                ),
                                new Shoot(0, 10, projectileIndex: 2, fixedAngle: 0, coolDown: 1000),
                                new TimedTransition(5000, "Imma Follow")
                            )
                        ),
                        new State("Circle",
                            new State("Prepare",
                                new MoveTo(127.5f, 39.5f, once: true, isMapPosition: true),
                                new EntityExistsTransition("Oryx Stone Guardian Left", 1, "Prepare2")
                            ),
                            new State("Prepare2",
                                new MoveTo(130.5f, 39.5f, once: true, isMapPosition: true),
                                new TimedTransition(1000, "PrepareEnd")
                            ),
                            new State("PrepareEnd",
                                new Orbit(1, 5, target: "Oryx Guardian TaskMaster"),
                                new State("cpe_1",
                                    new Shoot(0, 2, fixedAngle: 0, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_2")
                                ),
                                new State("cpe_2",
                                    new Shoot(0, 2, fixedAngle: 36, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_3")
                                ),
                                new State("cpe_3",
                                    new Shoot(0, 2, fixedAngle: 72, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_4")
                                ),
                                new State("cpe_4",
                                    new Shoot(0, 2, fixedAngle: 108, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_5")
                                ),
                                new State("cpe_5",
                                    new Shoot(0, 2, fixedAngle: 144, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_6")
                                ),
                                new State("cpe_6",
                                    new Shoot(0, 2, fixedAngle: 180, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_7")
                                ),
                                new State("cpe_7",
                                    new Shoot(0, 2, fixedAngle: 216, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_8")
                                ),
                                new State("cpe_8",
                                    new Shoot(0, 2, fixedAngle: 252, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_9")
                                ),
                                new State("cpe_9",
                                    new Shoot(0, 2, fixedAngle: 288, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_10")
                                ),
                                new State("cpe_10",
                                    new Shoot(0, 2, fixedAngle: 324, projectileIndex: 1),
                                    new TimedTransition(200, "checkEntities")
                                ),
                                new State("checkEntities",
                                    new PlayerWithinTransition(3, "cpe_Imma Follow"),
                                    new NoPlayerWithinTransition(3, "cpe_Imma chill")
                                ),
                                new State("cpe_x",
                                    new TimedTransition(5000, "Move Sideways"),
                                    new State("cpe_Imma Follow",
                                        new Follow(1, 3, 0.3),
                                        new Shoot(5, 5, coolDown: 1000),
                                        new TimedTransition(2500, "cpe_Imma chill")
                                    ),
                                    new State("cpe_Imma chill",
                                        new Prioritize(
                                            new StayCloseToSpawn(0.5, 3),
                                            new Wander(0.5)
                                        ),
                                        new Shoot(0, 10, projectileIndex: 2, fixedAngle: 0, coolDown: 1000),
                                        new TimedTransition(2500, "cpe_Imma Follow")
                                    )
                                )
                            )
                        ),
                        new State("Move Sideways",
                            new State("msw_prepare",
                                new MoveTo(141.5f, 39.5f, once: true, isMapPosition: true),
                                new TimedTransition(1500, "msw_shoot")
                            ),
                            new State("msw_shoot",
                                new Shoot(0, 2, fixedAngle: 90, coolDownOffset: 0),
                                new Shoot(0, 2, fixedAngle: 85.5, coolDownOffset: 100),
                                new Shoot(0, 2, fixedAngle: 81, coolDownOffset: 200),
                                new Shoot(0, 2, fixedAngle: 76.5, coolDownOffset: 300),
                                new Shoot(0, 2, fixedAngle: 72, coolDownOffset: 400),
                                new Shoot(0, 2, fixedAngle: 67.5, coolDownOffset: 500),
                                new Shoot(0, 2, fixedAngle: 63, coolDownOffset: 600),
                                new Shoot(0, 2, fixedAngle: 58.5, coolDownOffset: 700),
                                new Shoot(0, 2, fixedAngle: 54, coolDownOffset: 800),
                                new Shoot(0, 2, fixedAngle: 49.5, coolDownOffset: 900),
                                new Shoot(0, 2, fixedAngle: 45, coolDownOffset: 1000),
                                new Shoot(0, 2, fixedAngle: 40.5, coolDownOffset: 1100),
                                new Shoot(0, 2, fixedAngle: 36, coolDownOffset: 1200),
                                new Shoot(0, 2, fixedAngle: 31.5, coolDownOffset: 1300),
                                new Shoot(0, 2, fixedAngle: 27, coolDownOffset: 1400),
                                new Shoot(0, 2, fixedAngle: 22.5, coolDownOffset: 1500),
                                new Shoot(0, 2, fixedAngle: 18, coolDownOffset: 1600),
                                new Shoot(0, 2, fixedAngle: 13.5, coolDownOffset: 1700),
                                new Shoot(0, 2, fixedAngle: 9, coolDownOffset: 1800),
                                new Shoot(0, 2, fixedAngle: 4.5, coolDownOffset: 1900)
                            )
                        )
                    ),
                    new State("Forever Alone")
                )
            )
            .Init("Oryx Stone Guardian Left",
                new State(
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new PlayerWithinTransition(2, "Order")
                    ),
                    new State("Order",
                        new Order(10, "Oryx Stone Guardian Right", "Start"),
                        new TimedTransition(0, "Start")
                    ),
                    new State("Start",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xC0C0C0, 0.5, 3),
                        new TimedTransition(1500, "Together is better")
                    ),
                    new State("Together is better",
                        new EntityNotExistsTransition("Oryx Stone Guardian Right", 100, "Forever Alone"),
                        new State("Lets go",
                            new TimedTransition(10000, "Circle"),
                            new State("Imma Follow",
                                new Follow(1, 2, 0.3),
                                new Shoot(5, 5, shootAngle: 5, coolDown: 1000),
                                new TimedTransition(5000, "Imma chill")
                            ),
                            new State("Imma chill",
                                new Prioritize(
                                    new StayCloseToSpawn(0.5, 3),
                                    new Wander(0.5)
                                ),
                                new Shoot(0, 10, projectileIndex: 2, fixedAngle: 0, coolDown: 1000),
                                new TimedTransition(5000, "Imma Follow")
                            )
                        ),
                        new State("Circle",
                            new State("Prepare",
                                new MoveTo(127.5f, 39.5f, once: true, isMapPosition: true),
                                new EntityExistsTransition("Oryx Stone Guardian Right", 1, "Prepare2")
                            ),
                            new State("Prepare2",
                                new MoveTo(124.5f, 39.5f, once: true, isMapPosition: true),
                                new TimedTransition(1000, "PrepareEnd")
                            ),
                            new State("PrepareEnd",
                                new Orbit(1, 5, target: "Oryx Guardian TaskMaster"),
                                new State("cpe_1",
                                    new Shoot(0, 2, fixedAngle: 0, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_2")
                                ),
                                new State("cpe_2",
                                    new Shoot(0, 2, fixedAngle: 36, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_3")
                                ),
                                new State("cpe_3",
                                    new Shoot(0, 2, fixedAngle: 72, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_4")
                                ),
                                new State("cpe_4",
                                    new Shoot(0, 2, fixedAngle: 108, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_5")
                                ),
                                new State("cpe_5",
                                    new Shoot(0, 2, fixedAngle: 144, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_6")
                                ),
                                new State("cpe_6",
                                    new Shoot(0, 2, fixedAngle: 180, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_7")
                                ),
                                new State("cpe_7",
                                    new Shoot(0, 2, fixedAngle: 216, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_8")
                                ),
                                new State("cpe_8",
                                    new Shoot(0, 2, fixedAngle: 252, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_9")
                                ),
                                new State("cpe_9",
                                    new Shoot(0, 2, fixedAngle: 288, projectileIndex: 1),
                                    new TimedTransition(200, "cpe_10")
                                ),
                                new State("cpe_10",
                                    new Shoot(0, 2, fixedAngle: 324, projectileIndex: 1),
                                    new TimedTransition(200, "checkEntities")
                                ),
                                new State("checkEntities",
                                    new PlayerWithinTransition(3, "cpe_Imma Follow"),
                                    new NoPlayerWithinTransition(3, "cpe_Imma chill")
                                ),
                                new State("cpe_x",
                                    new TimedTransition(5000, "Move Sideways"),
                                    new State("cpe_Imma Follow",
                                        new Follow(1, 3, 0.3),
                                        new Shoot(5, 5, coolDown: 1000),
                                        new TimedTransition(2500, "cpe_Imma chill")
                                    ),
                                    new State("cpe_Imma chill",
                                        new Prioritize(
                                            new StayCloseToSpawn(0.5, 3),
                                            new Wander(0.5)
                                        ),
                                        new Shoot(0, 10, projectileIndex: 2, fixedAngle: 0, coolDown: 1000),
                                        new TimedTransition(2500, "cpe_Imma Follow")
                                    )
                                )
                            )
                        ),
                        new State("Move Sideways",
                            new State("msw_prepare",
                                new MoveTo(113.5f, 39.5f, once: true, isMapPosition: true),
                                new TimedTransition(1500, "msw_shoot")
                            ),
                            new State("msw_shoot",
                                new Shoot(0, 2, fixedAngle: 90, coolDownOffset: 0),
                                new Shoot(0, 2, fixedAngle: 85.5, coolDownOffset: 100),
                                new Shoot(0, 2, fixedAngle: 81, coolDownOffset: 200),
                                new Shoot(0, 2, fixedAngle: 76.5, coolDownOffset: 300),
                                new Shoot(0, 2, fixedAngle: 72, coolDownOffset: 400),
                                new Shoot(0, 2, fixedAngle: 67.5, coolDownOffset: 500),
                                new Shoot(0, 2, fixedAngle: 63, coolDownOffset: 600),
                                new Shoot(0, 2, fixedAngle: 58.5, coolDownOffset: 700),
                                new Shoot(0, 2, fixedAngle: 54, coolDownOffset: 800),
                                new Shoot(0, 2, fixedAngle: 49.5, coolDownOffset: 900),
                                new Shoot(0, 2, fixedAngle: 45, coolDownOffset: 1000),
                                new Shoot(0, 2, fixedAngle: 40.5, coolDownOffset: 1100),
                                new Shoot(0, 2, fixedAngle: 36, coolDownOffset: 1200),
                                new Shoot(0, 2, fixedAngle: 31.5, coolDownOffset: 1300),
                                new Shoot(0, 2, fixedAngle: 27, coolDownOffset: 1400),
                                new Shoot(0, 2, fixedAngle: 22.5, coolDownOffset: 1500),
                                new Shoot(0, 2, fixedAngle: 18, coolDownOffset: 1600),
                                new Shoot(0, 2, fixedAngle: 13.5, coolDownOffset: 1700),
                                new Shoot(0, 2, fixedAngle: 9, coolDownOffset: 1800),
                                new Shoot(0, 2, fixedAngle: 4.5, coolDownOffset: 1900)
                            )
                        )
                    ),
                    new State("Forever Alone")
                )
            )
            .Init("Oryx Guardian TaskMaster",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("Idle",
                        new EntitiesNotExistsTransition(100, "Death", "Oryx Stone Guardian Right", "Oryx Stone Guardian Left")
                    ),
                    new State("Death",
                        new Spawn("Oryx's Chamber Portal", 1, 1),
                        new Suicide()
                    )
                )
            )
            .Init("Oryx's Living Floor Fire Down",
                new State(
                    new State("Idle",
                        new PlayerWithinTransition(20, "Toss")
                    ),
                    new State("Toss",
                        new TossObject("Quiet Bomb", 10, coolDown: 1000),
                        new NoPlayerWithinTransition(21, "Idle"),
                        new PlayerWithinTransition(5, "Shoot and Toss")
                    ),
                    new State("Shoot and Toss",
                        new NoPlayerWithinTransition(21, "Idle"),
                        new NoPlayerWithinTransition(6, "Toss"),
                        new Shoot(0, 18, fixedAngle: 0, coolDown: new Cooldown(750, 250)),
                        new TossObject("Quiet Bomb", 10, coolDown: 1000)
                    )
                )
            )
            .Init("Oryx Knight",
        	    new State(
        	      	new State("waiting for u bae <3",
        	      	    new PlayerWithinTransition(10, "tim 4 rekkings")
        	      	    ),
        	      	new State("tim 4 rekkings",
        	      	    new Prioritize(
        	      	        new Wander(0.2),
        	      	        new Follow(0.6, 10, 3, -1, 0)
        	      	       ),
        	      	    new Shoot(10, 3, 20, 0, coolDown: 350),
        	      	    new TimedTransition(5000, "tim 4 singular rekt")
        	      	    ),
        	      	new State("tim 4 singular rekt",
        	      	    new Prioritize(
        	      	       	new Wander(0.2),
        	      	        new Follow(0.7, 10, 3, -1, 0)
        	      	        ),
        	      	    new Shoot(10, 1, projectileIndex: 0, coolDown: 50),
        	      	    new Shoot(10, 1, projectileIndex: 1, coolDown: 1000),
        	      	    new Shoot(10, 1, projectileIndex: 2, coolDown: 450),
        	      	    new TimedTransition(2500, "tim 4 rekkings")
        	      	   )
        	      )
        	)
        	.Init("Oryx Pet",
        	    new State(
        	      	new State("swagoo baboon",
        	      	    new PlayerWithinTransition(10, "anuspiddle")
        	      	    ),
        	      	new State("anuspiddle",
        	      	    new Prioritize(
        	      	        new Wander(0.2),
        	      	        new Follow(0.6, 10, 0, -1, 0)
        	      	        ),
        	      	    new Shoot(10, 2, shootAngle: 20, projectileIndex: 0, coolDown: 1),
						new Shoot(10, 1, projectileIndex: 0, coolDown: 1)
        	      	   )
        	      )
        	)
        	.Init("Oryx Insect Commander",
        	    new State(
        	      	new State("lol jordan is a nub",
        	      	    new Prioritize(
        	      	    	new Wander(0.2)
        	      	        ),
        	      	    new Reproduce("Oryx Insect Minion", 10, 20, 1, 50),
        	      	    new Shoot(10, 1, projectileIndex: 0, coolDown: 900)
        	      	   )
        	      )
        	)
        	.Init("Oryx Insect Minion",
        	    new State(
        	      	new State("its SWARMING time",
        	      	    new Prioritize(
        	      	        new Wander(0.2),
        	      	        new StayCloseToSpawn(0.4, 8),
        	      	       	new Follow(0.8, 10, 1, -1, 0)
        	      	        ),
        	      	    new Shoot(10, 5, projectileIndex: 0, coolDown: 1500),
        	      	    new Shoot(10, 1, projectileIndex: 0, coolDown: 230)
        	      	    )
        	      )
        	)
        	.Init("Oryx Suit of Armor",
        	    new State(
        	      	new State("idle",
        	      	    new PlayerWithinTransition(8, "attack me pl0x")
        	      	    ),
        	      	new State("attack me pl0x",
    	      	        new DamageTakenTransition(1, "jordan is stanking")
    	      	        ),
        	      	new State("jordan is stanking",
        	      	    new Prioritize(
        	      	     	new Wander(0.2),
        	      	     	new Follow(0.4, 10, 2, -1, 0)
        	      	        ),
        	      	    new SetAltTexture(1),
        	      	    new Shoot(10, 2, 15, 0, coolDown: 600),
        	      	    new HpLessTransition(0.2, "heal")
        	      	    ),
        	      	new State("heal",
        	      	    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
        	      	    new SetAltTexture(0),
        	      	    new Shoot(10, 6, projectileIndex: 0, coolDown: 200),
        	      	    new SpecificHeal(1, 200, "Self", 1),
        	      	    new TimedTransition(1500, "jordan is stanking")
        	      	   )
        	      )
        	)
			.Init("Oryx Eye Warrior",
			    new State(
				    new State("swaggin",
					    new PlayerWithinTransition(10, "penispiddle")
						),
				    new State("penispiddle",
        	      	    new Prioritize(
        	      	        new Follow(0.6, 10, 0, -1, 0)
        	      	        ),
        	      	    new Shoot(10, 5, projectileIndex: 0, coolDown: 1000),
        	      	    new Shoot(10, 1, projectileIndex: 1, coolDown: 500)
        	      	   )
        	      )
        	)
        	.Init("Oryx Brute",
        	    new State(
        	      	new State("swaggin",
        	      	    new PlayerWithinTransition(10, "piddle")
        	            ),   	      	    
        	      	new State("piddle",
        	      	    new Prioritize(
        	      	        new Wander(0.2),
        	      	        new Follow(0.4, 10, 1, -1, 0)
        	      	        ),
        	      	    new Shoot(10, 5, projectileIndex: 1, coolDown: 1000),
        	      	    new Reproduce("Oryx Eye Warrior", 10, 4, 2, 1750),
        	      	    new TimedTransition(5000, "charge")
        	      	    ),
        	      	new State("charge",
        	      	    new Prioritize(
        	      	        new Wander(0.3),
        	      	        new Follow(1.2, 10, 1, -1, 0)
        	      	        ),
        	      	    new Shoot(10, 5, projectileIndex: 1, coolDown: 1000),
        	      	    new Shoot(10, 5, projectileIndex: 2, coolDown: 750),
        	      	    new Reproduce("Oryx Eye Warrior", 10, 4, 2, 1750),
        	      	    new Shoot(10, 3, 10, projectileIndex: 0, coolDown: 300),
        	      	    new TimedTransition(4000, "piddle")
        	      	   )
        	      )
        	)
            .Init("Quiet Bomb",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new State("Idle",
                        new State("Tex1",
                            new TimedTransition(250, "Tex2")
                        ),
                        new State("Tex2",
                            new SetAltTexture(1),
                            new TimedTransition(250, "Tex3")
                        ),
                        new State("Tex3",
                            new SetAltTexture(0),
                            new TimedTransition(250, "Tex4")
                        ),
                        new State("Tex4",
                            new SetAltTexture(1),
                            new TimedTransition(250, "Explode")
                        )
                    ),
                    new State("Explode",
                        new SetAltTexture(0),
                        new Shoot(0, 18, fixedAngle: 0),
                        new Suicide()
                    )
                )
            );
    }
}

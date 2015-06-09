using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.networking.svrPackets;
using wServer.realm;
using wServer.realm.entities.player;

namespace wServer.logic.behaviors
{
    public class PlaySound : Behavior
    {
        private readonly int soundId;

        public PlaySound(int soundId = 0)
        {
            this.soundId = soundId;
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            foreach (var i in host.GetNearestEntities(25, null).OfType<Player>())
            {
                i.Client.SendPacket(new PlaySoundPacket
                {
                    OwnerId = host.Id,
                    SoundId = soundId
                });
            }
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state) { }
    }
}

#region

using System.Linq;
using wServer.networking.svrPackets;
using wServer.realm;
using wServer.realm.entities;
using wServer.realm.entities.player;

#endregion

namespace wServer.logic.behaviors
{
    public class Taunt : Behavior
    {
        //State storage: time

        private readonly bool broadcast;
        private readonly float probability = 1;
        private readonly string[] text;
        private readonly string[] states;
        private readonly bool checkForStates;
        private Cooldown cooldown = new Cooldown(0, 0);

        public Taunt(params string[] text)
        {
            this.text = text;
        }

        public Taunt(double probability, params string[] text)
        {
            this.text = text;
            this.probability = (float) probability;
        }

        public Taunt(bool broadcast, params string[] text)
        {
            this.text = text;
            this.broadcast = broadcast;
        }

        public Taunt(Cooldown cooldown, params string[] text)
        {
            this.text = text;
            this.cooldown = cooldown;
        }

        public Taunt(double probability, bool broadcast, params string[] text)
        {
            this.text = text;
            this.probability = (float) probability;
            this.broadcast = broadcast;
        }

        public Taunt(double probability, Cooldown cooldown, params string[] text)
        {
            this.text = text;
            this.probability = (float) probability;
            this.cooldown = cooldown;
        }

        public Taunt(bool broadcast, Cooldown cooldown, params string[] text)
        {
            this.text = text;
            this.broadcast = broadcast;
            this.cooldown = cooldown;
        }

        public Taunt(double probability, bool broadcast, Cooldown cooldown, params string[] text)
        {
            this.text = text;
            this.probability = (float) probability;
            this.broadcast = broadcast;
            this.cooldown = cooldown;
        }

        public Taunt(string[] states, Cooldown cooldown, params string[] text)
        {
            this.text = text;
            this.cooldown = cooldown;
            this.states = states;
            checkForStates = true;
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            state = null;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            if (checkForStates && !states.ToList().Contains(host.CurrentState.Name)) return;
            if (state != null && cooldown.CoolDown == 0) return; //cooldown = 0 -> once per state entry

            int c;
            if (state == null) c = cooldown.Next(Random);
            else c = (int) state;

            c -= time.thisTickTimes;
            state = c;
            if (c > 0) return;

            c = cooldown.Next(Random);
            state = c;

            if (Random.NextDouble() >= probability) return;

            string taunt = text.Length == 1 ? text[0] : text[Random.Next(text.Length)];
            if (taunt.Contains("{PLAYER}"))
            {
                Entity player = host.GetNearestEntity(10, null);
                if (player == null) return;
                taunt = taunt.Replace("{PLAYER}", player.Name);
            }
            taunt = taunt.Replace("{HP}", (host as Enemy).HP.ToString());

            TextPacket packet = new TextPacket
            {
                Name = "#" + (host.ObjectDesc.DisplayId ?? host.ObjectDesc.ObjectId),
                ObjectId = host.Id,
                Stars = -1,
                BubbleTime = 5,
                Recipient = "",
                Text = taunt,
                CleanText = ""
            };
            if (broadcast)
                host.Owner.BroadcastPacket(packet, null);
            else
                foreach (Player i in host.Owner.PlayersCollision.HitTest(host.X, host.Y, 15).OfType<Player>())
                    if (host.Dist(i) < 15)
                        i.Client.SendPacket(packet);
        }
    }
}
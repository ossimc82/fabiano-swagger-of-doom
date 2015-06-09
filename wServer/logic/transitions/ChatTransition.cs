using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.realm;

namespace wServer.logic.transitions
{
    public class ChatTransition : Transition
    {
        private readonly string[] texts;
        private bool transit;

        public ChatTransition(string targetState, params string[] texts)
            : base(targetState)
        {
            this.texts = texts ?? Empty<string>.Array;
            this.transit = false;
        }

        protected override bool TickCore(Entity host, RealmTime time, ref object state)
        {
            return transit;
        }

        public void OnChatReceived(string text)
        {
            if (texts.Contains(text))
                transit = true;
        }
    }
}

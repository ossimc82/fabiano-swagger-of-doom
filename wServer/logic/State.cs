#region

using System;
using System.Collections.Generic;
using wServer.realm;

#endregion

namespace wServer.logic
{
    public interface IStateChildren
    {
    }

    public class State : IStateChildren
    {
        public static readonly State NullState = new State();

        public State(params IStateChildren[] children) : this("", children)
        {
        }

        public State(string name, params IStateChildren[] children)
        {
            Name = name;
            States = new List<State>();
            Behaviors = new List<Behavior>();
            Transitions = new List<Transition>();
            foreach (IStateChildren i in children)
            {
                if (i is State)
                {
                    State state = i as State;
                    state.Parent = this;
                    States.Add(state);
                }
                else if (i is Behavior)
                    Behaviors.Add(i as Behavior);
                else if (i is Transition)
                    Transitions.Add(i as Transition);
                else
                    throw new NotSupportedException("Unknown children type.");
            }
        }

        public string Name { get; private set; }
        public State Parent { get; private set; }
        public IList<State> States { get; private set; }
        public IList<Behavior> Behaviors { get; private set; }
        public IList<Transition> Transitions { get; private set; }

        public static State CommonParent(State a, State b)
        {
            if (a == null || b == null) return null;
            return _CommonParent(a, a, b);
        }

        private static State _CommonParent(State current, State a, State b)
        {
            if (b.Is(current)) return current;
            if (a.Parent == null) return null;
            return _CommonParent(current.Parent, a, b);
        }

        //child is parent
        //parent is not child
        public bool Is(State state)
        {
            if (this == state) return true;
            if (Parent != null) return Parent.Is(state);
            return false;
        }

        public event EventHandler<BehaviorEventArgs> Death;

        internal void OnDeath(BehaviorEventArgs e)
        {
            if (Death != null)
                Death(this, e);
            if (Parent != null)
                Parent.OnDeath(e);
        }

        internal void Resolve(Dictionary<string, State> states)
        {
            states[Name] = this;
            foreach (State i in States)
                i.Resolve(states);
        }

        internal void ResolveChildren(Dictionary<string, State> states)
        {
            foreach (State i in States)
                i.ResolveChildren(states);
            foreach (Transition j in Transitions)
                j.Resolve(states);
            foreach (Behavior j in Behaviors)
                j.Resolve(this);
        }

        private void ResolveTransition(Dictionary<string, State> states)
        {
            foreach (Transition i in Transitions)
                i.Resolve(states);
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class BehaviorEventArgs : EventArgs
    {
        public BehaviorEventArgs(Entity host, RealmTime time)
        {
            Host = host;
            Time = time;
        }

        public Entity Host { get; private set; }
        public RealmTime Time { get; private set; }
    }
}
using System;
using System.Collections.Generic;

namespace XDynamicStateMachine
{
    public class XStatePosition<TState>
    {
        readonly TState _state;
        readonly string _action;
        readonly string _actor;

        public XStatePosition(TState currentState, string actor, string action)
        {
            if (EqualityComparer<TState>.Default.Equals(currentState, default(TState)))
                throw new ArgumentNullException("currentState");
            if (EqualityComparer<string>.Default.Equals(actor, default(string)))
                throw new ArgumentNullException("actor");
            if (EqualityComparer<string>.Default.Equals(action, default(string)))
                throw new ArgumentNullException("action");

            _state = currentState;
            _actor = actor;
            _action = action;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + _state.GetHashCode();
                hash = hash * 31 + _action.GetHashCode();
                hash = hash * 31 + _actor.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            var other = obj as XStatePosition<TState>;
            return 
                other != null && 
                EqualityComparer<TState>.Default.Equals(_state, other._state) &&
                _action == other._action && _actor == other._actor;
        }
    }
}

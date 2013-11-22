using System;
using System.Collections.Generic;

namespace XDynamicStateMachine
{
    public class XStatePosition<TState, TActor, TAction>
    {
        readonly TState _state;
        readonly TAction _action;
        readonly TActor _actor;

        public XStatePosition(TState currentState, TActor actor, TAction action)
        {
            if (EqualityComparer<TState>.Default.Equals(currentState, default(TState)))
                throw new ArgumentNullException("currentState");
            if (EqualityComparer<TActor>.Default.Equals(actor, default(TActor)))
                throw new ArgumentNullException("actor");
            if (EqualityComparer<TAction>.Default.Equals(action, default(TAction)))
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
            var other = obj as XStatePosition<TState, TActor, TAction>;
            return
                other != null &&
                EqualityComparer<TState>.Default.Equals(_state, other._state) &&
                EqualityComparer<TActor>.Default.Equals(_actor, other._actor) &&
                EqualityComparer<TAction>.Default.Equals(_action, other._action);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace XDynamicStateMachine
{
    public class XDynamicStateMachine<TState, TActor, TAction>
    {
        readonly Dictionary<XStatePosition<TState, TActor, TAction>, TState> _workflows;
        public TState CurrentState { get; private set; }

        public XDynamicStateMachine(Dictionary<XStatePosition<TState, TActor, TAction>, TState> workflows, TState initialState)
        {
            if (workflows == null || !workflows.Any())
                throw new ArgumentNullException("workflows", "Missing Workflow definitions in constructor");

            if (EqualityComparer<TState>.Default.Equals(initialState, default(TState)))
                throw new ArgumentNullException("initialState", "Missing initial workflow state");

            CurrentState = initialState;
            _workflows = workflows;
        }

        private TState FindNext(TState state, TActor actor, TAction action)
        {
            var position = new XStatePosition<TState, TActor, TAction>(state, actor, action);
            TState nextState;
            if (!_workflows.TryGetValue(position, out nextState))
                throw new ArgumentException(string.Format("exInvalidStateAction:{0}>>{1}>>{2}", CurrentState, actor, action));
            return nextState;
        }

        public bool CanMoveNext(TActor actor, TAction action)
        {
            var position = new XStatePosition<TState, TActor, TAction>(CurrentState, actor, action);
            TState nextState;
            return _workflows.TryGetValue(position, out nextState);
        }

        public TState MoveNext(TActor actor, TAction action)
        {
            CurrentState = FindNext(CurrentState, actor, action);
            return CurrentState;
        }
    }
}

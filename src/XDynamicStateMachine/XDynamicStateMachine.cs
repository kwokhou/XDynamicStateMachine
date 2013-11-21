using System;
using System.Collections.Generic;
using System.Linq;

namespace XDynamicStateMachine
{
    public class XDynamicStateMachine
    {
        readonly Dictionary<XStatePosition, string> _workflows;
        public string CurrentState { get; set; }

        public XDynamicStateMachine(Dictionary<XStatePosition, string> workflows, string initialState)
        {
            if (workflows == null || !workflows.Any())
                throw new ArgumentNullException("workflows", "Missing Workflow definitions in constructor");

            if (string.IsNullOrEmpty(initialState))
                throw new ArgumentNullException("initialState", "Missing initial workflow state");

            CurrentState = initialState;
            _workflows = workflows;
        }

        private string FindNext(string state, string actor, string action)
        {
            var position = new XStatePosition(state, actor, action);
            string nextState;
            if (!_workflows.TryGetValue(position, out nextState))
                throw new ArgumentException(string.Format("exInvalidStateAction:{0}>{1}>{2}", actor, this.CurrentState, action));
            return nextState;
        }

        public string MoveNext(string actor, string action)
        {
            CurrentState = FindNext(CurrentState, actor, action);
            return CurrentState;
        }
    }
}

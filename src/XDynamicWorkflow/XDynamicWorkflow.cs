using System;
using System.Collections.Generic;
using System.Linq;

namespace XDynamicWorkflow
{
    public class XDynamicWorkflow
    {
        readonly Dictionary<XStatePosition, string> _workflowDefinitions;
        public string CurrentState { get; set; }

        public XDynamicWorkflow(Dictionary<XStatePosition, string> workflowDefinitionses, string initialState)
        {
            if (workflowDefinitionses == null || !workflowDefinitionses.Any())
                throw new ArgumentNullException("workflowDefinitionses", "Missing Workflow definitions in constructor");

            if (string.IsNullOrEmpty(initialState))
                throw new ArgumentNullException("initialState", "Missing initial workflow state");

            CurrentState = initialState;
            _workflowDefinitions = workflowDefinitionses;
        }

        private void FindNext(string actor, string action, Action<string> successAction, Action failAction)
        {
            var position = new XStatePosition(this.CurrentState, actor, action);
            string nextState;
            if (!_workflowDefinitions.TryGetValue(position, out nextState))
            {
                if (failAction != null)
                    failAction();
            }
            else
            {
                CurrentState = nextState;
                if (successAction != null)
                    successAction(nextState);
            }
        }

        public string MoveNext(string actor, string action, Action<string> successAction, Action failAction)
        {
            FindNext(actor, action, successAction, failAction);
            return CurrentState;
        }
    }
}

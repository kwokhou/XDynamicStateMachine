using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XDynamicWorkflow
{
    public class XDynamicWorkflow
    {
        readonly Dictionary<XStatePosition, string> _definitions;
        public string CurrentState { get; set; }

        public XDynamicWorkflow(Dictionary<XStatePosition, string> stateDefinitions, string initialState = null)
        {
            CurrentState = initialState;
            _definitions = stateDefinitions;
        }

        private void FindNext(string actor, string action, Action<string> successAction, Action failAction)
        {
            var position = new XStatePosition(this.CurrentState, actor, action);
            string nextState;
            if (!_definitions.TryGetValue(position, out nextState))
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

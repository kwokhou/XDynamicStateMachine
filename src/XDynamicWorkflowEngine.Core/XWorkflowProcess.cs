using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XDynamicWorkflowEngine.Core
{
    public interface IXWorkflowSubject
    {
        int[] XAllWorkflowStates { get; }
        int XCurrentWorkflowState { get; }
        int XWorkflowTransition();
    }

    public class XWorkflowProcess
    {
        #region State Transition
        
        class StateTransition
        {
            readonly int State;
            readonly int Action;
            readonly string Actor;

            public StateTransition(int currentState, int action, string actor)
            {
                State = currentState;
                Action = action;
                Actor = actor;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    hash = hash * 31 + State.GetHashCode();
                    hash = hash * 31 + Action.GetHashCode();
                    hash = hash * 31 + Actor.GetHashCode();
                    return hash;
                }
            }

            public override bool Equals(object obj)
            {
                var other = obj as StateTransition;
                return other != null && State == other.State && Action == other.Action && Actor == other.Actor;
            }
        }

        #endregion
    }
}

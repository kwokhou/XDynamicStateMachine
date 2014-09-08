using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Xunit;

namespace XDynamicStateMachine.Tests
{
    public abstract class XDynamicStateMachineTests<TState, TActor, TAction>
    {
        private Fixture fixture;

        public XDynamicStateMachineTests()
        {
            fixture = new Fixture();
        }

        [Fact]
        public void Cannot_create_workflow_without_definitions()
        {
            var exception =
                Assert.Throws<ArgumentNullException>(() => new XDynamicStateMachine<TState, TActor, TAction>(null, default(TState)));
            Assert.NotNull(exception);
        }

        [Fact]
        public void Cannot_create_workflow_with_empty_definitions()
        {
            var exception =
                Assert.Throws<ArgumentNullException>(
                    () =>
                        new XDynamicStateMachine<TState, TActor, TAction>(new Dictionary<XStatePosition<TState, TActor, TAction>, TState>(),
                            default(TState)));
            Assert.NotNull(exception);
        }

        [Fact]
        public void Cannot_create_workflow_without_initialState()
        {
            var sampleDefinition = SampleWorkflowDefinition();
            var initialState = default(TState);
            var exception = Assert.Throws<ArgumentNullException>(() => new XDynamicStateMachine<TState, TActor, TAction>(sampleDefinition, initialState));
            Assert.NotNull(exception);
        }

        [Fact]
        public void Can_create_workflow_with_initialize_state()
        {
            var sampleDefinition = SampleWorkflowDefinition();
            var initialState = fixture.Create<TState>();
            var workflow = new XDynamicStateMachine<TState, TActor, TAction>(sampleDefinition, initialState);
            Assert.Equal(initialState, workflow.CurrentState);
        }

        [Fact]
        public void Can_transition_a_workflow()
        {
            var stateUnknown = fixture.Create<TState>();
            var stateDraft = fixture.Create<TState>();
            var statePending = fixture.Create<TState>();
            var stateApproved = fixture.Create<TState>();
            var stateRejected = fixture.Create<TState>();

            var actorCreator = fixture.Create<TActor>();
            var actorApprover = fixture.Create<TActor>();

            var actionSave = fixture.Create<TAction>();
            var actionSubmit = fixture.Create<TAction>();
            var actionApprove = fixture.Create<TAction>();
            var actionReject = fixture.Create<TAction>();

            var sampleDefinition = new Dictionary<XStatePosition<TState, TActor, TAction>, TState>
            {
                {new XStatePosition<TState, TActor, TAction>(stateUnknown, actorCreator, actionSave), stateDraft},
                {new XStatePosition<TState, TActor, TAction>(stateUnknown, actorCreator, actionSubmit), statePending},
                {new XStatePosition<TState, TActor, TAction>(stateDraft, actorCreator, actionSubmit), statePending},
                {new XStatePosition<TState, TActor, TAction>(stateRejected, actorCreator, actionSubmit), statePending},
                {new XStatePosition<TState, TActor, TAction>(statePending, actorApprover, actionApprove), stateApproved},
                {new XStatePosition<TState, TActor, TAction>(statePending, actorApprover, actionReject), stateRejected},
            };

            var stateMachine = new XDynamicStateMachine<TState, TActor, TAction>(sampleDefinition, stateUnknown);
            stateMachine.MoveNext(actorCreator, actionSave);
            stateMachine.MoveNext(actorCreator, actionSubmit);
            stateMachine.MoveNext(actorApprover, actionReject);
            Assert.Equal(stateRejected, stateMachine.CurrentState);

            stateMachine.MoveNext(actorCreator, actionSubmit);
            Assert.Equal(statePending, stateMachine.CurrentState);
        }

        [Fact]
        public void Cannot_transition_to_an_undefined_flow_state()
        {
            var stateUnknown = fixture.Create<TState>();
            var stateDraft = fixture.Create<TState>();
            var statePending = fixture.Create<TState>();
            var stateApproved = fixture.Create<TState>();
            var stateRejected = fixture.Create<TState>();

            var actorCreator = fixture.Create<TActor>();
            var actorApprover = fixture.Create<TActor>();

            var actionSave = fixture.Create<TAction>();
            var actionSubmit = fixture.Create<TAction>();
            var actionApprove = fixture.Create<TAction>();
            var actionReject = fixture.Create<TAction>();

            var sampleDefinition = new Dictionary<XStatePosition<TState, TActor, TAction>, TState>
            {
                {new XStatePosition<TState, TActor, TAction>(stateUnknown, actorCreator, actionSave), stateDraft},
                {new XStatePosition<TState, TActor, TAction>(stateUnknown, actorCreator, actionSubmit), statePending},
                {new XStatePosition<TState, TActor, TAction>(stateDraft, actorCreator, actionSubmit), statePending},
                {new XStatePosition<TState, TActor, TAction>(stateRejected, actorCreator, actionSubmit), statePending},
                {new XStatePosition<TState, TActor, TAction>(statePending, actorApprover, actionApprove), stateApproved},
                {new XStatePosition<TState, TActor, TAction>(statePending, actorApprover, actionReject), stateRejected},
            };

            var stateMachine = new XDynamicStateMachine<TState, TActor, TAction>(sampleDefinition, stateUnknown);
            stateMachine.MoveNext(actorCreator, actionSubmit); // -> PENDING 
            stateMachine.MoveNext(actorApprover, actionApprove); // -> APPROVE
            Assert.Equal(stateApproved, stateMachine.CurrentState);

            Assert.Throws<ArgumentException>(() =>
            {
                stateMachine.MoveNext(actorApprover, actionSave); // undefined workflow action
            });
        }

        [Fact]
        public void Can_check_if_StateMachine_are_able_to_transition_to_a_State()
        {
            var stateUnknown = fixture.Create<TState>();
            var stateDraft = fixture.Create<TState>();
            var statePending = fixture.Create<TState>();
            var stateApproved = fixture.Create<TState>();
            var stateRejected = fixture.Create<TState>();

            var actorCreator = fixture.Create<TActor>();
            var actorApprover = fixture.Create<TActor>();

            var actionSave = fixture.Create<TAction>();
            var actionSubmit = fixture.Create<TAction>();
            var actionApprove = fixture.Create<TAction>();
            var actionReject = fixture.Create<TAction>();

            var sampleDefinition = new Dictionary<XStatePosition<TState, TActor, TAction>, TState>
            {
                {new XStatePosition<TState, TActor, TAction>(stateUnknown, actorCreator, actionSave), stateDraft},
                {new XStatePosition<TState, TActor, TAction>(stateUnknown, actorCreator, actionSubmit), statePending},
                {new XStatePosition<TState, TActor, TAction>(stateDraft, actorCreator, actionSubmit), statePending},
                {new XStatePosition<TState, TActor, TAction>(stateRejected, actorCreator, actionSubmit), statePending},
                {new XStatePosition<TState, TActor, TAction>(statePending, actorApprover, actionApprove), stateApproved},
                {new XStatePosition<TState, TActor, TAction>(statePending, actorApprover, actionReject), stateRejected},
            };

            var stateMachine = new XDynamicStateMachine<TState, TActor, TAction>(sampleDefinition, stateUnknown);
            var result = stateMachine.CanMoveNext(actorCreator, actionSubmit); // -> PENDING 
            Assert.True(result);
        }

        [Fact]
        public void Can_check_if_StateMachine_are_not_able_to_transition_to_a_State()
        {
            var stateUnknown = fixture.Create<TState>();
            var stateDraft = fixture.Create<TState>();
            var statePending = fixture.Create<TState>();
            var stateApproved = fixture.Create<TState>();
            var stateRejected = fixture.Create<TState>();

            var actorCreator = fixture.Create<TActor>();
            var actorApprover = fixture.Create<TActor>();

            var actionSave = fixture.Create<TAction>();
            var actionSubmit = fixture.Create<TAction>();
            var actionApprove = fixture.Create<TAction>();
            var actionReject = fixture.Create<TAction>();

            var sampleDefinition = new Dictionary<XStatePosition<TState, TActor, TAction>, TState>
            {
                {new XStatePosition<TState, TActor, TAction>(stateUnknown, actorCreator, actionSave), stateDraft},
                {new XStatePosition<TState, TActor, TAction>(stateUnknown, actorCreator, actionSubmit), statePending},
                {new XStatePosition<TState, TActor, TAction>(stateDraft, actorCreator, actionSubmit), statePending},
                {new XStatePosition<TState, TActor, TAction>(stateRejected, actorCreator, actionSubmit), statePending},
                {new XStatePosition<TState, TActor, TAction>(statePending, actorApprover, actionApprove), stateApproved},
                {new XStatePosition<TState, TActor, TAction>(statePending, actorApprover, actionReject), stateRejected},
            };

            var stateMachine = new XDynamicStateMachine<TState, TActor, TAction>(sampleDefinition, stateDraft);
            var result = stateMachine.CanMoveNext(actorCreator, actionReject);

            Assert.False(result);
        }

        public Dictionary<XStatePosition<TState, TActor, TAction>, TState> SampleWorkflowDefinition()
        {
            var stateUnknown = fixture.Create<TState>();
            var stateDraft = fixture.Create<TState>();
            var statePending = fixture.Create<TState>();
            var stateApproved = fixture.Create<TState>();
            var stateRejected = fixture.Create<TState>();

            var actorCreator = fixture.Create<TActor>();
            var actorApprover = fixture.Create<TActor>();

            var actionSave = fixture.Create<TAction>();
            var actionSubmit = fixture.Create<TAction>();
            var actionApprove = fixture.Create<TAction>();
            var actionReject = fixture.Create<TAction>();

            var sampleDefinition = new Dictionary<XStatePosition<TState, TActor, TAction>, TState>
            {
                {new XStatePosition<TState, TActor, TAction>(stateUnknown, actorCreator, actionSave), stateDraft},
                {new XStatePosition<TState, TActor, TAction>(stateUnknown, actorCreator, actionSubmit), statePending},
                {new XStatePosition<TState, TActor, TAction>(stateDraft, actorCreator, actionSubmit), statePending},
                {new XStatePosition<TState, TActor, TAction>(stateRejected, actorCreator, actionSubmit), statePending},
                {new XStatePosition<TState, TActor, TAction>(statePending, actorApprover, actionApprove), stateApproved},
                {new XStatePosition<TState, TActor, TAction>(statePending, actorApprover, actionReject), stateRejected},
            };

            return sampleDefinition;
        }
    }

    public enum SampleEnumActor
    {
        Creator = 100,
        Approver = 200
    }

    public enum SampleEnumState
    {
        Unknown = 1,
        Request = 2,
        Submit = 3,
        Reject = 4,
        Approved = 5
    }

    public class IntegerXDynamicStateMachineTests : XDynamicStateMachineTests<SampleEnumState, SampleEnumActor, string>
    {
    }

    public class StringXDynamicStateMachineTests : XDynamicStateMachineTests<char, string, double>
    {
    }

    public class DecimalXDynamicStateMachineTests : XDynamicStateMachineTests<decimal, int, int>
    {
    }
}

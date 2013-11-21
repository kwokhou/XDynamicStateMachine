using System;
using System.Collections.Generic;
using Xunit;

namespace XDynamicStateMachine.Tests
{
    public abstract class XDynamicStateMachineTests<TState>
    {
        [Fact]
        public void Cannot_create_workflow_without_definitions()
        {
            var exception =
                Assert.Throws<ArgumentNullException>(() => new XDynamicStateMachine<TState>(null, default(TState)));
            Assert.NotNull(exception);
        }

        [Fact]
        public void Cannot_create_workflow_with_empty_definitions()
        {
            var exception =
                Assert.Throws<ArgumentNullException>(
                    () =>
                        new XDynamicStateMachine<TState>(new Dictionary<XStatePosition<TState>, TState>(),
                            default(TState)));
            Assert.NotNull(exception);
        }

        [Fact]
        public void Cannot_create_workflow_without_initialState()
        {
            var sampleDefinition = new Dictionary<XStatePosition<string>, string>
            {
                {new XStatePosition<string>("UNKNOWN", "REQUESTER", "SAVE AS DRAFT"), "DRAFT"},
                {new XStatePosition<string>("UNKNOWN", "REQUESTER", "SUBMIT FOR APPROVAL"), "PENDING"},
                {new XStatePosition<string>("DRAFT", "REQUESTER", "SUBMIT FOR APPROVAL"), "PENDING"}
            };

            var exception = Assert.Throws<ArgumentNullException>(() => new XDynamicStateMachine<string>(sampleDefinition, null));
            Assert.NotNull(exception);
        }

        [Fact]
        public void Can_create_workflow_with_initialize_state()
        {
            var sampleDefinition = new Dictionary<XStatePosition<string>, string>
            {
                {new XStatePosition<string>("UNKNOWN", "REQUESTER", "SAVE AS DRAFT"), "DRAFT"},
                {new XStatePosition<string>("UNKNOWN", "REQUESTER", "SUBMIT FOR APPROVAL"), "PENDING"},
                {new XStatePosition<string>("DRAFT", "REQUESTER", "SUBMIT FOR APPROVAL"), "PENDING"}
            };

            var workflow = new XDynamicStateMachine<string>(sampleDefinition, "DRAFT");
            Assert.Equal("DRAFT", workflow.CurrentState);
        }

        [Fact]
        public void Can_transition_a_workflow()
        {
            var simpleWorkflowDefinitions = new Dictionary<XStatePosition<string>, string>
            {
                {new XStatePosition<string>("UNKNOWN", "REQUESTER", "SAVE AS DRAFT"), "DRAFT"},
                {new XStatePosition<string>("UNKNOWN", "REQUESTER", "SUBMIT FOR APPROVAL"), "PENDING"},
                {new XStatePosition<string>("DRAFT", "REQUESTER", "SUBMIT FOR APPROVAL"), "PENDING"},
                {new XStatePosition<string>("PENDING", "APPROVER", "APPROVE A REQUEST"), "APPROVE"}
            };

            var stateMachine = new XDynamicStateMachine<string>(simpleWorkflowDefinitions, "UNKNOWN");
            stateMachine.MoveNext("REQUESTER", "SUBMIT FOR APPROVAL");

            Assert.Equal("PENDING", stateMachine.CurrentState);
        }

        [Fact]
        public void Can_transition_a_workflow_to_2nd_level()
        {
            var simpleWorkflowDefinitions = new Dictionary<XStatePosition<string>, string>
            {
                {new XStatePosition<string>("UNKNOWN", "REQUESTER", "SAVE AS DRAFT"), "DRAFT"},
                {new XStatePosition<string>("UNKNOWN", "REQUESTER", "SUBMIT FOR APPROVAL"), "PENDING"},
                {new XStatePosition<string>("DRAFT", "REQUESTER", "SUBMIT FOR APPROVAL"), "PENDING"},
                {new XStatePosition<string>("PENDING", "APPROVER", "APPROVE A REQUEST"), "APPROVE"}
            };

            var stateMachine = new XDynamicStateMachine<string>(simpleWorkflowDefinitions, "UNKNOWN");
            stateMachine.MoveNext("REQUESTER", "SUBMIT FOR APPROVAL");
            stateMachine.MoveNext("APPROVER", "APPROVE A REQUEST");

            Assert.Equal("APPROVE", stateMachine.CurrentState);
        }
        
        [Fact]
        public void Can_transition_a_workflow_with_integer_state()
        {
            var simpleWorkflowDefinitions = new Dictionary<XStatePosition<int>, int>
            {
                {new XStatePosition<int>(1, "REQUESTER", "SAVE AS DRAFT"), 2 },
                {new XStatePosition<int>(2, "REQUESTER", "SUBMIT FOR APPROVAL"), 4},
                {new XStatePosition<int>(4, "APPROVER", "APPROVE A REQUEST"), 8}
            };

            var stateMachine = new XDynamicStateMachine<int>(simpleWorkflowDefinitions, 1);
            stateMachine.MoveNext("REQUESTER", "SAVE AS DRAFT");
            stateMachine.MoveNext("REQUESTER", "SUBMIT FOR APPROVAL");
            stateMachine.MoveNext("APPROVER", "APPROVE A REQUEST");

            Assert.Equal(8, stateMachine.CurrentState);
        }

        [Fact]
        public void Cannot_transition_to_an_undefined_flow_state()
        {
            var simpleWorkflowDefinitions = new Dictionary<XStatePosition<string>, string>
            {
                {new XStatePosition<string>("UNKNOWN", "REQUESTER", "SAVE AS DRAFT"), "DRAFT"},
                {new XStatePosition<string>("UNKNOWN", "REQUESTER", "SUBMIT FOR APPROVAL"), "PENDING"},
                {new XStatePosition<string>("DRAFT", "REQUESTER", "SUBMIT FOR APPROVAL"), "PENDING"},
                {new XStatePosition<string>("PENDING", "APPROVER", "APPROVE A REQUEST"), "APPROVE"}
            };

            var stateMachine = new XDynamicStateMachine<string>(simpleWorkflowDefinitions, "UNKNOWN");
            stateMachine.MoveNext("REQUESTER", "SUBMIT FOR APPROVAL"); // -> PENDING 
            stateMachine.MoveNext("APPROVER", "APPROVE A REQUEST"); // -> APPROVE

            Assert.Throws<ArgumentException>(() =>
            {
                stateMachine.MoveNext("APPROVER", "REJECT A REQUEST"); // undefined action
            });
        }
    }

    public class StringXDynamicStateMachineTests : XDynamicStateMachineTests<string>
    {
    }

    public class IntegerXDynamicStateMachineTests : XDynamicStateMachineTests<int>
    {
    }

    public class DecimalXDynamicStateMachineTests : XDynamicStateMachineTests<decimal>
    {
    }
}

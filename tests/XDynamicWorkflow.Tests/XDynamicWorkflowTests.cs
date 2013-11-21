using System;
using System.Collections.Generic;
using Xunit;

namespace XDynamicWorkflow.Tests
{
    public class XDynamicWorkflowTests
    {
        [Fact]
        public void Cannot_create_workflow_without_definitions()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new XDynamicWorkflow(null, "UNKNOWN"));
            Assert.NotNull(exception);
        }

        [Fact]
        public void Cannot_create_workflow_with_empty_definitions()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new XDynamicWorkflow(new Dictionary<XStatePosition, string>(), "UNKNOWN"));
            Assert.NotNull(exception);
        }

        [Fact]
        public void Cannot_create_workflow_without_initialState()
        {
            var sampleDefinition = new Dictionary<XStatePosition, string>
            {
                {new XStatePosition("UNKNOWN", "REQUESTER", "SAVE AS DRAFT"), "DRAFT"},
                {new XStatePosition("UNKNOWN", "REQUESTER", "SUBMIT FOR APPROVAL"), "PENDING"},
                {new XStatePosition("DRAFT", "REQUESTER", "SUBMIT FOR APPROVAL"), "PENDING"}
            };

            var exception = Assert.Throws<ArgumentNullException>(() =>
                new XDynamicWorkflow(sampleDefinition, null));
            Assert.NotNull(exception);
        }

        [Fact]
        public void Can_create_workflow_with_initialize_state()
        {
            var sampleDefinition = new Dictionary<XStatePosition, string>
            {
                {new XStatePosition("UNKNOWN", "REQUESTER", "SAVE AS DRAFT"), "DRAFT"},
                {new XStatePosition("UNKNOWN", "REQUESTER", "SUBMIT FOR APPROVAL"), "PENDING"},
                {new XStatePosition("DRAFT", "REQUESTER", "SUBMIT FOR APPROVAL"), "PENDING"}
            };

            var workflow = new XDynamicWorkflow(sampleDefinition, "DRAFT");
            Assert.Equal("DRAFT", workflow.CurrentState);
        }

        [Fact]
        public void Can_transition_a_workflow()
        {
            var simpleWorkflowDefinitions = new Dictionary<XStatePosition, string>
            {
                {new XStatePosition("UNKNOWN", "REQUESTER", "SAVE AS DRAFT"), "DRAFT"},
                {new XStatePosition("UNKNOWN", "REQUESTER", "SUBMIT FOR APPROVAL"), "PENDING"},
                {new XStatePosition("DRAFT", "REQUESTER", "SUBMIT FOR APPROVAL"), "PENDING"},
                {new XStatePosition("PENDING", "APPROVER", "APPROVE A REQUEST"), "APPROVE"}
            };

            var stateMachine = new XDynamicWorkflow(simpleWorkflowDefinitions, "UNKNOWN");
            stateMachine.MoveNext("REQUESTER", "SUBMIT FOR APPROVAL");

            Assert.Equal("PENDING", stateMachine.CurrentState);
        }

        [Fact]
        public void Can_transition_a_workflow_to_2nd_level()
        {
            var simpleWorkflowDefinitions = new Dictionary<XStatePosition, string>
            {
                {new XStatePosition("UNKNOWN", "REQUESTER", "SAVE AS DRAFT"), "DRAFT"},
                {new XStatePosition("UNKNOWN", "REQUESTER", "SUBMIT FOR APPROVAL"), "PENDING"},
                {new XStatePosition("DRAFT", "REQUESTER", "SUBMIT FOR APPROVAL"), "PENDING"},
                {new XStatePosition("PENDING", "APPROVER", "APPROVE A REQUEST"), "APPROVE"}
            };

            var stateMachine = new XDynamicWorkflow(simpleWorkflowDefinitions, "UNKNOWN");
            stateMachine.MoveNext("REQUESTER", "SUBMIT FOR APPROVAL");
            stateMachine.MoveNext("APPROVER", "APPROVE A REQUEST");

            Assert.Equal("APPROVE", stateMachine.CurrentState);
        }

        [Fact]
        public void Cannot_transition_to_an_undefined_flow_state()
        {
            var simpleWorkflowDefinitions = new Dictionary<XStatePosition, string>
            {
                {new XStatePosition("UNKNOWN", "REQUESTER", "SAVE AS DRAFT"), "DRAFT"},
                {new XStatePosition("UNKNOWN", "REQUESTER", "SUBMIT FOR APPROVAL"), "PENDING"},
                {new XStatePosition("DRAFT", "REQUESTER", "SUBMIT FOR APPROVAL"), "PENDING"},
                {new XStatePosition("PENDING", "APPROVER", "APPROVE A REQUEST"), "APPROVE"}
            };

            var stateMachine = new XDynamicWorkflow(simpleWorkflowDefinitions, "UNKNOWN");
            stateMachine.MoveNext("REQUESTER", "SUBMIT FOR APPROVAL"); // -> PENDING 
            stateMachine.MoveNext("APPROVER", "APPROVE A REQUEST"); // -> APPROVE

            Assert.Throws<ArgumentException>(() =>
            {
                stateMachine.MoveNext("APPROVER", "REJECT A REQUEST"); // undefined action
            });
        }
    }
}

using System;
using System.Collections.Generic;
using XDynamicWorkflow;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XDynamicWorkflow.Tests
{
    [TestClass]
    public class XDynamicWorkflowTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Cannot_create_workflow_without_definitions()
        {
            var workflow = new XDynamicWorkflow(null, "UNKNOWN");
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Cannot_create_workflow_with_empty_definitions()
        {
            var emptyDefinition = new Dictionary<XStatePosition, string>();
            var workflow = new XDynamicWorkflow(emptyDefinition, "UNKNOWN");
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Cannot_create_workflow_without_initialState()
        {
            var sampleDefinition = new Dictionary<XStatePosition, string>
            {
                {new XStatePosition("UNKNOWN", "REQUESTER", "SAVE AS DRAFT"), "DRAFT"},
                {new XStatePosition("UNKNOWN", "REQUESTER", "SUBMIT FOR APPROVAL"), "PENDING"},
                {new XStatePosition("DRAFT", "REQUESTER", "SUBMIT FOR APPROVAL"), "PENDING"}
            };

            var workflow = new XDynamicWorkflow(sampleDefinition, null);
            Assert.Fail();
        }

        [TestMethod]
        public void Can_create_workflow_with_initialize_state()
        {
            var sampleDefinition = new Dictionary<XStatePosition, string>
            {
                {new XStatePosition("UNKNOWN", "REQUESTER", "SAVE AS DRAFT"), "DRAFT"},
                {new XStatePosition("UNKNOWN", "REQUESTER", "SUBMIT FOR APPROVAL"), "PENDING"},
                {new XStatePosition("DRAFT", "REQUESTER", "SUBMIT FOR APPROVAL"), "PENDING"}
            };

            var workflow = new XDynamicWorkflow(sampleDefinition, "DRAFT");
            Assert.AreEqual("DRAFT", workflow.CurrentState);
        }
    }
}

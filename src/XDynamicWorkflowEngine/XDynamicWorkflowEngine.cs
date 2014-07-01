using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XDynamicWorkflowEngine
{
    public interface IWorkflowObject
    {
        int WorkflowActor { get; set; }
        int WorkflowState { get; set; }
    }

    public class ChangeOrder : IWorkflowObject
    {
        public int WorkflowActor { get; set; }
        public int WorkflowState { get; set; }
    }

    public class XDynamicWorkflowEngine
    {
    }
}

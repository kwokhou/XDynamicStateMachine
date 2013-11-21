using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XDynamicWorkflow
{
    public interface IXDynamicWorkflowDefinition
    {
        Dictionary<XStatePosition, string> Workflows { get; }
        string DefaultState { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ArmEval.Core.Tests
{
    public class ArmClientConfigDeploymentTests : ArmClientConfig
    {
        public ArmClientConfigDeploymentTests() : base()
        {
            ResourceGroup = $"ArmEvalDeploy-{DateTime.Now.Ticks.ToString().Substring(13)}";
        }
    }
}

using ArmEval.Core.AzureClient;
using ArmEval.Core.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArmEval.Core.Tests.AzureClient
{
    public class ArmClientConfigDeploymentTests : ArmClientConfig
    {
        public ArmClientConfigDeploymentTests() : base()
        {
            var suffix = UniqueString.Create(5);
            ResourceGroup = $"ArmEvalDeploy-{suffix}";
        }
    }
}

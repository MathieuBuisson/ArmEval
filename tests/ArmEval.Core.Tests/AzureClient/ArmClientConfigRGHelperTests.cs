using ArmEval.Core.AzureClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArmEval.Core.Tests.AzureClient
{
    public class ArmClientConfigRGHelperTests : ArmClientConfig
    {
        public ArmClientConfigRGHelperTests() : base()
        {
            ResourceGroup = $"ArmEvalRGHelper-{DateTime.Now.Ticks.ToString().Substring(13)}";
        }
    }
}

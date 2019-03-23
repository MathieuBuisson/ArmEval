using ArmEval.Core.AzureClient;
using ArmEval.Core.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArmEval.Core.Tests.AzureClient
{
    public class ArmClientConfigRGHelperTests : ArmClientConfig
    {
        public ArmClientConfigRGHelperTests() : base()
        {
            var suffix = UniqueString.Create(5);
            ResourceGroup = $"ArmEvalRGHelper-{suffix}";
        }
    }
}

using ArmEval.Core.AzureClient;
using ArmEval.Core.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArmEval.Core.Tests.UserInputs
{
    public class ArmClientConfigExpressionTests : ClientConfig
    {
        public ArmClientConfigExpressionTests() : base()
        {
            var suffix = UniqueString.Create(5);
            ResourceGroup = $"ArmEvalExpr-{suffix}";
        }
    }
}

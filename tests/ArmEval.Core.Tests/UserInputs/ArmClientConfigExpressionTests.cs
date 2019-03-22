using ArmEval.Core.AzureClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArmEval.Core.Tests.UserInputs
{
    public class ArmClientConfigExpressionTests : ArmClientConfig
    {
        public ArmClientConfigExpressionTests() : base()
        {
            ResourceGroup = $"ArmEvalExpr-{DateTime.Now.Ticks.ToString().Substring(13)}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ArmEval.Core.Tests
{
    public class ArmClientConfigExpressionTests : ArmClientConfig
    {
        public ArmClientConfigExpressionTests() : base()
        {
            ResourceGroup = $"ArmEvalExpr-{DateTime.Now.Ticks.ToString().Substring(13)}";
        }
    }
}

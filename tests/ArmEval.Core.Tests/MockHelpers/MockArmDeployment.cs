using System;
using System.Collections.Generic;
using System.Text;
using ArmEval.Core.ArmClient;
using Moq;

namespace ArmEval.Core.Tests.MockHelpers
{
    public class MockArmDeployment : Mock<IArmDeployment>
    {
        public MockArmDeployment MockInvoke(string outputType, string outputValue)
        {
            var strBuilder = new StringBuilder("{\r\n  \"expression\": {\r\n    \"type\": \"")
                .Append(outputType)
                .Append("\",\r\n    \"value\": \"")
                .Append(outputValue)
                .Append("\"\r\n  }\r\n}");

            var outputString = strBuilder.ToString();

            Setup(m => m.Invoke()).Returns(outputString);
            return this;
        }
    }
}

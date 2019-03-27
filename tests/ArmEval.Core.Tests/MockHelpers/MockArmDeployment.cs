using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArmEval.Core.ArmClient;
using ArmEval.Core.Extensions;
using Moq;

namespace ArmEval.Core.Tests.MockHelpers
{
    public class MockArmDeployment : Mock<IArmDeployment>
    {
        public MockArmDeployment MockInvoke(string outputType, string outputValue)
        {
            var quotedTypes = new List<string>(){ "string" };
            if (quotedTypes.Contains(outputType))
            {
                outputValue = $"\"{outputValue}\"";
            }

            var strBuilder = new StringBuilder("{\r\n  \"expression\": {\r\n    \"type\": \"")
                .Append(outputType.FirstCharToUpper())
                .Append("\",\r\n    \"value\": ")
                .Append(outputValue)
                .Append("\r\n  }\r\n}");

            var outputString = strBuilder.ToString();

            Setup(m => m.Invoke()).Returns(outputString);
            return this;
        }
    }
}

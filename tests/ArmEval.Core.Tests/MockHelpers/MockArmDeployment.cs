using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArmEval.Core.ArmClient;
using ArmEval.Core.ArmTemplate;
using ArmEval.Core.Extensions;
using Microsoft.Azure.Management.ResourceManager.Models;
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

            var mockDeployment = new Deployment();
            mockDeployment.Properties = new DeploymentProperties
            {
                Mode = DeploymentMode.Incremental,
                Template = new TemplateBuilder().Template,
            };

            SetupProperty(m => m.Deployment, mockDeployment);
            var outputString = strBuilder.ToString();
            Setup(m => m.Invoke()).Returns(outputString);
            return this;
        }
    }
}

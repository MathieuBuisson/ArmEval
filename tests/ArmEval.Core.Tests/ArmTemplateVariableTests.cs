using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ArmEval.Core.Tests
{
    public class ArmTemplateVariableTests
    {
        private readonly string variableName = "testVariable";
        private readonly string variableValue = "testValue";

        [Fact]
        public void Constructor_SetPropertiesAsExpected()
        {
            var actual = new ArmTemplateVariable(variableName, variableValue);
            var expectedReferencePattern = @"variables\([""']{1}testVariable[""']{1}\)";

            Assert.Equal(variableName, actual.Name);
            Assert.Equal(variableValue, actual.Value);
            Assert.Equal(expectedReferencePattern, actual.ReferencePattern.ToString());
        }
    }
}

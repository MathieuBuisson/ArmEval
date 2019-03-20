using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;

namespace ArmEval.Core.Tests
{
    public class ArmTemplateParameterTests
    {
        private readonly string paramName = "testParam";
        private readonly string paramValue = "testValue";

        [Fact]
        public void Constructor_TypeNotInArmValueTypes_ThrowsExpectedException()
        {
            var validValues = Enum.GetNames(typeof(ArmValueTypes));
            
            Action act = () => { new ArmTemplateParameter(paramName, paramValue, "NotAType"); };
            var ex = Record.Exception(act);

            Assert.IsType<ArgumentOutOfRangeException>(ex);
            Assert.All(validValues, (v) => ex.Message.Contains(v));
        }

        [Fact]
        public void Constructor_TypeInArmValueTypes_SetPropertiesAsExpected()
        {
            var actual = new ArmTemplateParameter(paramName, paramValue, "bool");
            var expectedReferencePattern = @"parameters\([""']{1}testParam[""']{1}\)";

            Assert.Equal(paramName, actual.Name);
            Assert.Equal(paramValue, actual.Value);
            Assert.Equal("bool", actual.Type);
            Assert.Equal(expectedReferencePattern, actual.ReferencePattern.ToString());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;

namespace ArmEval.Core.Tests
{
    public class ArmTemplateParameterTests
    {
        [Fact]
        public void TypeSetter_ValueNotInArmValueTypes_ThrowsExpectedException()
        {
            var templateParam = new ArmTemplateParameter();
            var validValues = Enum.GetNames(typeof(ArmValueTypes));
            
            Action act = () => { templateParam.Type = "NotAType"; };
            var ex = Record.Exception(act);

            Assert.IsType<ArgumentOutOfRangeException>(ex);
            Assert.All(validValues, (v) => ex.Message.Contains(v));
        }
    }
}

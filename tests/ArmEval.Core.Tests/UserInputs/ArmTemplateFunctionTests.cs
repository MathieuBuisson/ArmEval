using ArmEval.Core.UserInputs;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ArmEval.Core.Tests.UserInputs
{
    public class ArmTemplateFunctionTests
    {
        [Fact]
        public void Constructor_NullExpression_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ArmTemplateFunction(null));
        }

    }
}

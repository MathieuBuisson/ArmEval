using System;
using Xunit;
using ArmEval.Core.UserInputs;

namespace ArmEval.Core.Tests.UserInputs
{
    public class ExpressionValidationResultTests
    {
        [Fact]
        public void Constructor_SetsSuccessToTrue()
        {
            var result = new ExpressionValidationResult();
            var actual = result.Success;
            Assert.True(actual);
        }

        [Fact]
        public void ExceptionSetter_NotSupportedException_AddsToExceptionProperty()
        {
            var result = new ExpressionValidationResult();
            result.Exception = new NotSupportedException("TestError");
            var actual = result.Exception;

            Assert.IsType<NotSupportedException>(actual);
            Assert.Equal("TestError", actual.Message);
        }
    }
}

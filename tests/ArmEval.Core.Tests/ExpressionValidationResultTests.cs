using System;
using Xunit;

namespace ArmEval.Core.Tests
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
        public void AddException_FormatException_IsAddedToExceptionList()
        {
            var result = new ExpressionValidationResult();
            result.AddException<FormatException>("Error");
            var actual = result.ExceptionList[0];

            Assert.NotNull(actual);
            Assert.IsType<FormatException>(actual);
        }
        [Fact]
        public void AddException_NotSupportedException_IsAddedToExceptionList()
        {
            var result = new ExpressionValidationResult();
            result.AddException<NotSupportedException>("Error");
            var actual = result.ExceptionList[0];

            Assert.NotNull(actual);
            Assert.IsType<NotSupportedException>(actual);
        }
        [Fact]
        public void AddException_FormatException_IsAddedToExceptionListWithExpectMessage()
        {
            var result = new ExpressionValidationResult();
            var expected = "Error Message";
            result.AddException<FormatException>("Error Message");
            var actual = result.ExceptionList[0];

            Assert.Equal(expected, actual.Message);
        }
    }
}

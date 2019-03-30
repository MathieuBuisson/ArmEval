using ArmEval.Core.ArmTemplate;
using ArmEval.Core.UserInputs;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ArmEval.Core.Tests.ArmTemplate
{
    public class TemplateBuilderTests
    {
        [Fact]
        public void Constructor_SetsExpectedTemplateProperty()
        {
            var actual = new TemplateBuilder().Template;

            Assert.IsType<JObject>(actual);
        }

        [Fact]
        public void Constructor_SetsExpectedTemplateWithExpectedElements()
        {
            var actual = new TemplateBuilder().Template;

            Assert.IsType<JObject>(actual);

            Assert.IsType<JValue>(actual["contentVersion"]);
            Assert.IsType<JObject>(actual["parameters"]);
            Assert.IsType<JObject>(actual["variables"]);
            Assert.IsType<JArray>(actual["resources"]);
            Assert.IsType<JObject>(actual["outputs"]);
        }

        [Theory]
        [InlineData(@"[concat('string1', 'String2')]", ArmValueTypes.@string)]
        [InlineData(@"[mod(7, 3)]", ArmValueTypes.@int)]
        [InlineData(@"[mul(6, 3)]", ArmValueTypes.@int)]
        [InlineData(@"[contains(createArray('one', 'two'), 'two')]", ArmValueTypes.@bool)]
        [InlineData(@"[not(equals(1, 10))]", ArmValueTypes.@bool)]
        [InlineData(@"[createArray(1, 2, 3)]", ArmValueTypes.array)]
        public void AddExpression_SetsExpectedTemplateOutputTypeAndValue(string expressionText,
            ArmValueTypes expectedOutputType)
        {
            var expression = new ArmTemplateExpression(expressionText);
            var actual = new TemplateBuilder()
                .AddExpression(expression, expectedOutputType)
                .Template["outputs"]["expression"];

            Assert.IsType<JObject>(actual);
            Assert.Equal(expectedOutputType.ToString(), actual["Type"].ToString());
            Assert.Equal(expressionText, actual["Value"].ToString());
        }
    }
}

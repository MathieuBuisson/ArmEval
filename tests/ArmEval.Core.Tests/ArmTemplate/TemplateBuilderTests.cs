using ArmEval.Core.ArmTemplate;
using ArmEval.Core.Tests.TestData;
using ArmEval.Core.UserInputs;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        [Fact]
        public void AddExpression_NullExpression_ThrowsArgumentNullException()
        {
            var templateBuilder = new TemplateBuilder();
            Action act = () => { templateBuilder.AddExpression(null, ArmValueTypes.@int); };
            var ex = Record.Exception(act);

            Assert.IsType<ArgumentNullException>(ex);
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

        [Fact]
        public void AddVariables_NullInputVariables_ThrowExpectedException()
        {
            var expression = new ArmTemplateExpression(@"[createArray(1, variables('num2'), variables('num3'))]");
            var sut = new TemplateBuilder();
            var expectedErrorMessage = "Enter a value for the following variable/parameter : num2";

            Action act = () => { sut.AddVariables(expression, null); };
            var ex = Record.Exception(act);

            Assert.IsType<ExpressionInputsException>(ex);
            Assert.StartsWith(expectedErrorMessage, ex.Message);
        }
        [Fact]
        public void AddVariables_MissingInputVariables_ThrowExpectedException()
        {
            var expression = new ArmTemplateExpression(@"[createArray(1, variables('num2'), variables('num3'))]");
            var inputVariables = new List<ArmTemplateVariable>(){ new ArmTemplateVariable("NotUsed", 0), new ArmTemplateVariable("num3", 3) };
            var sut = new TemplateBuilder();
            var expectedErrorMessage = "Enter a value for the following variable/parameter : num2";

            Action act = () => { sut.AddVariables(expression, inputVariables); };
            var ex = Record.Exception(act);

            Assert.IsType<ExpressionInputsException>(ex);
            Assert.StartsWith(expectedErrorMessage, ex.Message);
        }

        [Theory]
        [InputVariablesTestData]
        public void AddVariables_ValidInputVariables_SetsExpectedTemplateVariables(
            ArmTemplateExpression expression,
            List<ArmTemplateVariable> inputVariables
        )
        {
            var actual = new TemplateBuilder()
                .AddVariables(expression, inputVariables)
                .Template["variables"];

            Assert.IsType<JObject>(actual);
            foreach (var v in inputVariables)
            {
                if (v.Value.GetType().FullName.Contains("AnonymousType"))
                {
                    foreach (var property in v.Value.GetType().GetProperties())
                    {
                        var expectedValueString = property.GetValue(v.Value).ToString();
                        var actualValueString = actual[v.Name][property.Name].ToString();
                        Assert.Equal(expectedValueString, actualValueString);
                    }
                }
                else
                {
                    Assert.Equal(actual[v.Name], v.Value);
                }
            }
        }

        [Fact]
        public void AddParameters_NullOrEmptyInputParameters_ThrowExpectedException()
        {
            var expression = new ArmTemplateExpression(@"[createArray(1, parameters('num2'), parameters('num3'))]");
            var sut = new TemplateBuilder();
            var expectedErrorMessage = "Enter a value for the following variable/parameter : num2";

            Action act = () => { sut.AddParameters(expression, new List<ArmTemplateParameter>()); };
            var ex = Record.Exception(act);

            Assert.IsType<ExpressionInputsException>(ex);
            Assert.StartsWith(expectedErrorMessage, ex.Message);
        }

        [Fact]
        public void AddParameters_MissingInputParameters_ThrowExpectedException()
        {
            var expression = new ArmTemplateExpression(@"[createArray(1, parameters('num2'), parameters('num3'))]");
            var inputParameters = new List<ArmTemplateParameter>() { new ArmTemplateParameter("num3", 3, "int") };
            var sut = new TemplateBuilder();
            var expectedErrorMessage = "Enter a value for the following variable/parameter : num2";

            Action act = () => { sut.AddParameters(expression, inputParameters); };
            var ex = Record.Exception(act);

            Assert.IsType<ExpressionInputsException>(ex);
            Assert.StartsWith(expectedErrorMessage, ex.Message);
        }

        [Theory]
        [InputParametersTestData]
        public void AddParameters_ValidInputParameters_SetsExpectedTemplateParameters(
            ArmTemplateExpression expression,
            List<ArmTemplateParameter> inputParameters
        )
        {
            var actual = new TemplateBuilder()
                .AddParameters(expression, inputParameters)
                .Template["parameters"];

            Assert.IsType<JObject>(actual);
            foreach (var p in inputParameters)
            {
                if (p.Type == "object")
                {
                    Assert.Equal(p.Type, actual[p.Name]["type"].ToString());
                    foreach (var property in p.Value.GetType().GetProperties())
                    {
                        var actualValueString = actual[p.Name]["defaultValue"][property.Name].ToString();
                        Assert.Equal(property.GetValue(p.Value).ToString(), actualValueString);
                    }
                }
                else if (p.Type == "array")
                {
                    Assert.Equal(p.Type, actual[p.Name]["type"]);
                    var valueArray = (Array)p.Value;
                    foreach (var i in valueArray)
                    {
                        Assert.Contains($"{i.ToString()}", actual[p.Name]["defaultValue"].ToString());
                    }
                }
                else
                {
                    Assert.Equal(p.Type, actual[p.Name]["type"]);
                    Assert.Equal(p.Value.ToString(), actual[p.Name]["defaultValue"].ToString());
                }
            }
        }
    }
}

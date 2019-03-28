using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;
using ArmEval.Core.UserInputs;
using ArmEval.Core.ArmTemplate;
using System.Collections.ObjectModel;

namespace ArmEval.Core.Tests.ArmTemplate
{
    public class InputVariablesTestData : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            // Object properties represent (respectively) : expression text, output type, input variables
            new object[] {@"[variables('vmName')]", ArmValueTypes.@string, new List<ArmTemplateVariable>()
                {
                    new ArmTemplateVariable("vmName", "testVm")
                }
            },
            new object[] {@"[add(variables('number1'), 9)]", ArmValueTypes.@int, new List<ArmTemplateVariable>()
                {
                    new ArmTemplateVariable("number1", 8)
                }
            },
            new object[] {@"[mul(variables('var1'), variables('var2'))]", ArmValueTypes.@int, new List<ArmTemplateVariable>()
                {
                    new ArmTemplateVariable("var1", 3),
                    new ArmTemplateVariable("var2", 7)
                }
            },
            new object[] {@"[variables('obj').Property2]", ArmValueTypes.@object, new List<ArmTemplateVariable>()
                {
                    new ArmTemplateVariable("obj", new {Property1 = "customString", Property2 = true})
                }
            }
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class ArmTemplateTests
    {
        [Fact]
        public void Constructor_InitializesArmTemplateProperties()
        {
            var expectedSchema = @"https://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#";
            var expectedContentVersion = "1.0.0.0";
            var actual = new Template();

            Assert.Equal(expectedSchema, actual.Schema);
            Assert.Equal(expectedContentVersion, actual.ContentVersion);
            Assert.Equal(new Dictionary<string, object>(), actual.Parameters);
            Assert.Equal(new Dictionary<string, object>(), actual.Variables);
            Assert.Equal(new Collection<object>(), actual.Resources);
            Assert.Equal(new Dictionary<string, object>(), actual.Outputs);
        }

        [Fact]
        public void Deserialization_EmptyTemplateFile_ToArmTemplate()
        {
            var testFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData/EmptyTemplate.json");
            var templateContent = File.ReadAllText(testFilePath);
            var actual = JsonConvert.DeserializeObject<Template>(templateContent);

            Assert.IsType<Template>(actual);
            Assert.False(string.IsNullOrWhiteSpace(actual.Schema));
            Assert.False(string.IsNullOrWhiteSpace(actual.ContentVersion));
            Assert.IsAssignableFrom<IDictionary<string, object>>(actual.Parameters);
            Assert.IsAssignableFrom<IDictionary<string, object>>(actual.Variables);
            Assert.IsAssignableFrom<IDictionary<string, object>>(actual.Outputs);
            Assert.IsAssignableFrom<IEnumerable<object>>(actual.Resources);
        }

        [Fact]
        public void Deserialization_FullTemplateFile_ToArmTemplate()
        {
            var testFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData/FullTemplate.json");
            var templateContent = File.ReadAllText(testFilePath);
            var expectedSchema = @"https://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#";
            var expectedContentVersion = "1.0.0.0";
            var actual = JsonConvert.DeserializeObject<Template>(templateContent);

            Assert.IsType<Template>(actual);
            Assert.Equal(expectedSchema, actual.Schema);
            Assert.Equal(expectedContentVersion, actual.ContentVersion);

            Assert.IsAssignableFrom<IDictionary<string, object>>(actual.Parameters);
            Assert.Equal(12, actual.Parameters.Keys.Count);
            Assert.True(actual.Parameters.ContainsKey("clusterName"));
            Assert.True(actual.Parameters.ContainsKey("nodeCount"));

            Assert.IsAssignableFrom<IDictionary<string, object>>(actual.Variables);
            Assert.Equal(5, actual.Variables.Keys.Count);
            Assert.True(actual.Variables.ContainsKey("vnetName"));
            Assert.True(actual.Variables.ContainsKey("subnetName"));

            Assert.IsAssignableFrom<IDictionary<string, object>>(actual.Outputs);
            Assert.Equal(3, actual.Outputs.Keys.Count);
            Assert.True(actual.Outputs.ContainsKey("kubernetesMasterFqdn"));
            Assert.True(actual.Outputs.ContainsKey("aksResourceId"));

            Assert.IsAssignableFrom<IEnumerable<object>>(actual.Resources);
            Assert.Equal(4, actual.Resources.Count());
            Assert.True(actual.Resources.All(r => r is Newtonsoft.Json.Linq.JObject));

        }

        [Theory()]
        [InlineData(@"[concat('string12', 'string34')]", ArmValueTypes.@string, @"{ Type = String, Value = [concat('string12', 'string34')] }")]
        [InlineData(@"[mod(7, 3)]", ArmValueTypes.@int, @"{ Type = Int, Value = [mod(7, 3)] }")]
        [InlineData(@"[not(equals(1, 10))]", ArmValueTypes.@bool, @"{ Type = Bool, Value = [not(equals(1, 10))] }")]
        public void AddExpression_NoVariableOrParameter_AddsExpectedOutput(string text,
            ArmValueTypes outputType,
            string outputString)
        {
            var template = new Template();
            var expression = new ArmTemplateExpression(text);
            template.AddExpression(expression, outputType);
            var actual = template.Outputs["expression"];

            Assert.Equal(outputString, actual.ToString());
        }

        [Theory()]
        [InlineData(@"[variables('vmName')]", "the following variable(s) : vmName")]
        [InlineData(@"[add(variables('number1'), parameters('number2'))]", "the following variable(s) : number1")]
        [InlineData(@"[add(variables('var1'), variables('var2'))]", "the following variable(s) : var1, var2")]
        public void AddExpression_MissingInputVariables_ThrowsArgumentNullException(string text, string expectedMessage)
        {
            var template = new Template();
            var expression = new ArmTemplateExpression(text);
            var inputVariables = new List<ArmTemplateVariable>();
            
            Action act = () => { template.AddExpression(expression, ArmValueTypes.@int, inputVariables); };
            var ex = Record.Exception(act);

            Assert.IsType<ArgumentNullException>(ex);
            Assert.EndsWith(expectedMessage, ex.Message);
        }

        [Theory]
        [ClassData(typeof(InputVariablesTestData))]
        public void AddExpression_InputVariables_OutputsExpectedTypeAndValue(string text,
            ArmValueTypes expectedOutputType,
            List<ArmTemplateVariable> inputVariables)
        {
            var template = new Template();
            var expression = new ArmTemplateExpression(text);
            template.AddExpression(expression, expectedOutputType, inputVariables);
            var actual = template.Variables;

            Assert.All(inputVariables, v => actual.ContainsKey(v.Name));
            inputVariables.ForEach(v => Assert.Equal(v.Value, actual[v.Name]));
        }
    }
}

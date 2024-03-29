﻿using ArmEval.Core.ArmTemplate;
using ArmEval.Core.Tests.MockHelpers;
using ArmEval.Core.Tests.TestData;
using ArmEval.Core.UserInputs;
using ArmEval.Core.Utils;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace ArmEval.Core.Tests.UserInputs
{
    public class ArmTemplateExpressionTests
    {
        private readonly IResourceManagementClient client;
        private readonly string location;

        public ArmTemplateExpressionTests()
        {
            location = "North Europe";
            client = new Mock<IResourceManagementClient>().Object;
        }

        [Theory]
        [InlineData(@"[uniqueString(deployment().name)]")]
        [InlineData(@"[concat('string123', 'string456')]")]
        [InlineData(@"[add(variables('number1'), parameters('number2'))]")]
        public void Constructor_ValidExpressions_SetsText(string text)
        {
            var expression = new ArmTemplateExpression(text);
            var actual = expression.Text;
            Assert.Equal(text, actual);
        }

        [Fact]
        public void Constructor_NullExpression_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ArmTemplateExpression(null));
        }

        [Theory]
        [InlineData(@"[[test]")]
        [InlineData(@"[!test]")]
        [InlineData(@"[]")]
        public void Constructor_InvalidExpressions_ThrowsFormatException(string text)
        {
            Assert.Throws<FormatException>(() => new ArmTemplateExpression(text));
        }

        [Theory]
        [InlineData(@"[resourceId('Microsoft.Storage/storageAccounts','examplestorage')]")]
        [InlineData(@"[reference(test)]")]
        [InlineData(@"[listKeys(parameters('storagename'), '2018-02-01')]")]
        [InlineData(@"[resourceGroup().name]")]
        public void Constructor_UnsupportedFunctions_ThrowsNotSupportedException(string text)
        {
            Assert.Throws<NotSupportedException>(() => new ArmTemplateExpression(text));
        }

        [Theory]
        [InlineData(@"[uniqueString(deployment().name)]", new string[] {}, new string[] {})]
        [InlineData(@"[variables('vmName')]", new string[] {"vmName"}, new string[] {})]
        [InlineData(@"[parameters('testParam')]", new string[] {}, new string[] { "testParam" })]
        [InlineData(@"[add(variables('number1'), parameters('number2'))]", new string[] {"number1"}, new string[] {"number2"})]
        [InlineData(@"[add(variables('number1'), variables('number2'))]", new string[] {"number1", "number2"}, new string[] {})]
        public void Constructor_ValidExpressions_SetsVariablesAndParameters(string text, string[] expectedVariables, string[] expectedParameters)
        {
            var actual = new ArmTemplateExpression(text);

            Assert.Equal(text, actual.Text);
            Assert.Equal(expectedVariables, actual.VariableNames);
            Assert.Equal(expectedParameters, actual.ParameterNames);
        }

        [Fact]
        public void Invoke_NullDeployment_ThrowsArgumentNullException()
        {
            var expression = new ArmTemplateExpression(@"[not(true)]");
            var expectedOutputType = ArmValueTypes.@bool;

            Action act = () => { expression.Invoke(null, expectedOutputType, null, null); };
            var ex = Record.Exception(act);

            Assert.IsType<ArgumentNullException>(ex);
        }

        [Theory]
        [ExpressionTestData]
        public void Invoke_NoVariableOrParameter_ReturnsExpectedOutput(
            string text,
            ArmValueTypes expectedOutputType,
            string outputValue,
            string expectedoutputString
        )
        {
            var expression = new ArmTemplateExpression(text);
            var deployment = new MockArmDeployment()
                .MockInvoke(expectedOutputType.ToString(), outputValue)
                .Object;

            var actual = expression.Invoke(deployment, expectedOutputType, null, null);

            Assert.Equal(expectedoutputString, actual.ToString());
        }

        [Theory]
        [ExpressionWithVariablesTestData]
        public void Invoke_WithVariables_ReturnsExpectedOutput(
            string text,
            ICollection<ArmTemplateVariable> inputVariables,
            ArmValueTypes expectedOutputType,
            string outputValue,
            string expectedoutputString
        )
        {
            var expression = new ArmTemplateExpression(text);
            var deployment = new MockArmDeployment()
                .MockInvoke(expectedOutputType.ToString(), outputValue)
                .Object;

            var actual = expression.Invoke(deployment, expectedOutputType, null, inputVariables);

            Assert.Equal(expectedoutputString, actual.ToString());
        }

        [Theory]
        [ExpressionWithParametersTestData]
        public void Invoke_WithParameters_ReturnsExpectedOutput(
            string text,
            ICollection<ArmTemplateParameter> inputParameters,
            ICollection<ArmTemplateVariable> inputVariables,
            ArmValueTypes expectedOutputType,
            string outputValue,
            string expectedOutputString
        )
        {
            var expression = new ArmTemplateExpression(text);
            var deployment = new MockArmDeployment()
                .MockInvoke(expectedOutputType.ToString(), outputValue)
                .Object;

            var actual = expression.Invoke(deployment, expectedOutputType, inputParameters, inputVariables);

            Assert.Equal(expectedOutputString, actual.ToString());
        }

        [Theory]
        [ExpressionWithMissingInputsTestData]
        public void Invoke_MissingInputs_ReturnsExpectedMissingInputs(
            string text,
            ICollection<ArmTemplateParameter> inputParameters,
            ICollection<ArmTemplateVariable> inputVariables,
            ICollection<MissingInput> expectedMissingInputs
        )
        {
            var expression = new ArmTemplateExpression(text);
            var deployment = new MockArmDeployment()
                .MockInvoke("string", "value")
                .Object;

            var actual = expression.Invoke(deployment, ArmValueTypes.@string, inputParameters, inputVariables);

            Assert.IsType<List<MissingInput>>(actual);

            var missingInputs = actual as List<MissingInput>;
            Assert.Equal(expectedMissingInputs.Count, missingInputs.Count);
            Assert.All(expectedMissingInputs, e =>
            {
                Assert.Contains(missingInputs, a =>
                {
                    return a.Name == e.Name && a.InputType == e.InputType;
                });
            });
        }
    }
}

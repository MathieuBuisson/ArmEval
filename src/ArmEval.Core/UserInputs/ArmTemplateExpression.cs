﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ArmEval.Core.ArmTemplate;
using ArmEval.Core.ArmClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ArmEval.Core.UserInputs
{
    public class ArmTemplateExpression
    {
        private string text;
        public string Text
        {
            get => text;
            set {
                var result = Validate();
                text = result.Success ? value : throw result.Exception;
            }
        }

        public IEnumerable<string> VariableNames { get; private set; }
        public IEnumerable<string> ParameterNames { get; private set; }
        public readonly Regex[] unsupportedFunctionsPatterns = {
            new Regex(@"resourceId\("),
            new Regex(@"reference\("),
            new Regex(@"list.*\("),

            // Not supported by subscription-level deployments (https://docs.microsoft.com/en-us/azure/azure-resource-manager/deploy-to-subscription#use-template-functions)
            new Regex(@"resourceGroup\(")
        };

        public ArmTemplateExpression(string expressionText)
        {
            if (string.IsNullOrEmpty(expressionText))
            {
                throw new ArgumentNullException(nameof(expressionText));
            }
            text = expressionText;
            Text = expressionText;
            VariableNames = parseVariables(Text);
            ParameterNames = parseParameters(Text);
        }

        public ExpressionValidationResult Validate()
        {
            var op = new ExpressionValidationResult();

            var regex = new Regex(@"^\[{1}\w+.*\]$");
            var expressionRegexMatch = regex.IsMatch(text);
            if (!expressionRegexMatch)
            {
                op.Success = false;
                op.Exception = new FormatException($"The specified string is not an ARM template expression: {text}");
                return op;
            }

            // Only self-contained expressions are supported, so they cannot reference resources outside of the expression
            var unsupportedPatternsInText = unsupportedFunctionsPatterns.Where(p => p.IsMatch(text));
            if (unsupportedPatternsInText.Any())
            {
                op.Success = false;
                op.Exception = new NotSupportedException("The expression contains unsupported ARM template function(s)");
            }
            return op;
        }

        public object Invoke(IArmDeployment deployment,
            ArmValueTypes expectedOutputType,
            ICollection<ArmTemplateParameter> inputParameters,
            ICollection<ArmTemplateVariable> inputVariables)
        {
            if (deployment is null)
            {
                throw new ArgumentNullException(nameof(deployment));
            }

            var templateBuilder = new TemplateBuilder()
                .AddExpression(this, expectedOutputType)
                .AddParameters(this, inputParameters)
                .AddVariables(this, inputVariables);

            if (templateBuilder.MissingInputs.Any())
            {
                return templateBuilder.MissingInputs;
            }

            deployment.Deployment.Properties.Template = templateBuilder.Template;

            string deploymentOutputs = deployment.Invoke();
            var expressionOutput = JToken.Parse(deploymentOutputs)["expression"];
            var expressionOutputStr = expressionOutput.ToString();

            var output = JsonConvert.DeserializeObject(expressionOutputStr);
            return output;
        }

        private IEnumerable<string> parseVariables(string text)
        {
            var variableNames = new List<string>();

            var regex = new Regex(@"variables\([""']{1}(?<Variables>\w+)[""']{1}\)");
            var matches = regex.Matches(text);
            if (matches.Count > 0)
            {
                foreach (var match in matches.OfType<Match>())
                {
                    variableNames.Add(match.Groups["Variables"].Value);
                }
            }
            return variableNames;
        }
        private IEnumerable<string> parseParameters(string text)
        {
            var paramNames = new List<string>();

            var regex = new Regex(@"parameters\([""']{1}(?<Parameters>\w+)[""']{1}\)");
            var matches = regex.Matches(text);
            if (matches.Count > 0)
            {
                foreach (var match in matches.OfType<Match>())
                {
                    paramNames.Add(match.Groups["Parameters"].Value);
                }
            }
            return paramNames;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ArmEval.Core
{
    public class ArmTemplateExpression
    {
        private string text;
        public string Text
        {
            get { return text; }
            set {
                var result = Validate();
                text = result.Success ? value : throw result.ExceptionList[0];
            }
        }

        public List<string> VariableNames { get; private set; }
        public List<string> ParameterNames { get; private set; }
        public readonly Regex[] unsupportedFunctionsPatterns = {
            new Regex(@"resourceId\("),
            new Regex(@"reference\("),
            new Regex(@"list.*\(")
        };

        public ArmTemplateExpression(string expressionText)
        {
            if (string.IsNullOrEmpty(expressionText))
            {
                throw new ArgumentNullException("expressionText");
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
                op.AddException<FormatException>($"The specified string is not an ARM template expression: {text}");
            }

            // Only self-contained expressions are supported, so they cannot reference resources outside of the expression
            var unsupportedPatternsInText = unsupportedFunctionsPatterns.Where(p => p.IsMatch(text));
            if (unsupportedPatternsInText.Any())
            {
                op.Success = false;
                op.AddException<NotSupportedException>("The expression text contains unsupported ARM template function(s)");
            }
            return op;
        }

        public IDictionary<string, object> Invoke()
        {
            var outputs = new Dictionary<string, object>();


            return outputs;
        }

        private List<string> parseVariables(string text)
        {
            var variableNames = new List<string>();

            var regex = new Regex(@"variables\([""']{1}(?<Variables>\w+)[""']{1}\)");
            var matches = regex.Matches(text);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    var variableName = match.Groups["Variables"].Value;
                    variableNames.Add(variableName);
                }
            }
            return variableNames;

        }
        private List<string> parseParameters(string text)
        {
            var paramNames = new List<string>();

            var regex = new Regex(@"parameters\([""']{1}(?<Parameters>\w+)[""']{1}\)");
            var matches = regex.Matches(text);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    var paramName = match.Groups["Parameters"].Value;
                    paramNames.Add(paramName);
                }
            }
            return paramNames;
        }
    }
}

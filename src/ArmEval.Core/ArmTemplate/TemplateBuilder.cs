using ArmEval.Core.UserInputs;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArmEval.Core.ArmTemplate
{
    public class TemplateBuilder
    {
        public JObject Template { get; set; }

        public TemplateBuilder()
        {
            var jsonString = @"{
  '$schema': 'https://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#',
  'contentVersion': '1.0.0.0',
  'parameters': {},
  'variables': {},
  'resources': [],
  'outputs': {}
}";
            Template = JObject.Parse(jsonString);
        }

        public TemplateBuilder AddExpression(ArmTemplateExpression expression, ArmValueTypes expectedOutputType)
        {
            if (expression is null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            JObject OutputObj = new JObject(
                new JProperty(
                    "expression", new JObject(
                        new JProperty("Type", expectedOutputType.ToString()),
                        new JProperty("Value", expression.Text)
                    )
                )
            );
            Template["outputs"] = OutputObj;
            return this;
        }

        public TemplateBuilder AddVariables(ArmTemplateExpression expression, ICollection<ArmTemplateVariable> inputVariables)
        {
            // Adding input variables to the template only if the expression references variable(s)
            if (expression.VariableNames.Any())
            {
                CheckForMissingInputs(expression, inputVariables);

                JObject variablesObj = new JObject();
                foreach (var inputVar in inputVariables)
                {
                    variablesObj.Add(new JProperty(inputVar.Name, JToken.FromObject(inputVar.Value)));
                }
                Template["variables"] = variablesObj;
            }
            return this;
        }

        public TemplateBuilder AddParameters(ArmTemplateExpression expression, ICollection<ArmTemplateParameter> inputParameters)
        {
            // Adding input variables to the template only if the expression references parameters(s)
            if (expression.ParameterNames.Any())
            {
                CheckForMissingInputs(expression, inputParameters);

                JObject paramsObj = new JObject();
                foreach (var inputParam in inputParameters)
                {
                    paramsObj.Add(
                        new JProperty(inputParam.Name,
                            new JObject(new JProperty("defaultValue", JToken.FromObject(inputParam.Value))),
                            new JObject(new JProperty("type", JToken.FromObject(inputParam.Type)))
                        )
                    );
                }
            }
            return this;
        }

        private void CheckForMissingInputs<T>(ArmTemplateExpression expression, ICollection<T> inputs) where T : IArmTemplateInput
        {
            var referencesInExpression = typeof(T) == typeof(ArmTemplateVariable)
                ? expression.VariableNames
                : expression.ParameterNames;

            var missingInputs = inputs == null || inputs.Equals(default(T))
                ? referencesInExpression
                : referencesInExpression.Except(inputs.Select(i => i.Name));

            if (missingInputs.Any())
            {
                missingInputs.ToList()
                    .ForEach(missingInput => throw new ExpressionInputsException(missingInput));
            }
        }
    }
}

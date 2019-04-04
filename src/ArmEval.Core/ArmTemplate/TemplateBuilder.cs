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
                CheckExpressionVariablesAreInInput(expression, inputVariables);

                JObject variablesObj = new JObject();
                foreach (var inputVar in inputVariables)
                {
                    variablesObj.Add(new JProperty(inputVar.Name, JToken.FromObject(inputVar.Value)));
                }
                Template["variables"] = variablesObj;
            }
            return this;
        }

        private void CheckExpressionVariablesAreInInput(ArmTemplateExpression expression, ICollection<ArmTemplateVariable> inputVariables)
        {
            var missingVariables = inputVariables is null ?
                expression.VariableNames :
                expression.VariableNames.Except(inputVariables.Select(v => v.Name));

            if (missingVariables.Any())
            {
                var missingString = string.Join(", ", missingVariables);
                throw new ArgumentNullException(nameof(inputVariables), $"Specify a value for the following variable(s) : {missingString}");
            }
        }
    }
}

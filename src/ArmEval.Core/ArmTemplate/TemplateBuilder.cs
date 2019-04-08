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
        public List<MissingInput> MissingInputs { get; set; }

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
            MissingInputs = new List<MissingInput>();
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
                MissingInputs.AddRange(CheckForMissingInputs(expression, inputVariables));
                if (inputVariables == null || inputVariables.Count < 1)
                {
                    return this;
                }

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
                MissingInputs.AddRange(CheckForMissingInputs(expression, inputParameters));
                if (inputParameters == null || inputParameters.Count < 1)
                {
                    return this;
                }

                JObject paramsObj = new JObject();
                foreach (var inputParam in inputParameters)
                {
                    paramsObj.Add(
                        new JProperty(inputParam.Name,
                            new JObject {
                                { "type", JToken.FromObject(inputParam.Type) },
                                { "defaultValue", JToken.FromObject(inputParam.Value) }
                            }
                        )
                    );
                }
                Template["parameters"] = paramsObj;
            }
            return this;
        }

        private ICollection<MissingInput> CheckForMissingInputs<T>(ArmTemplateExpression expression, ICollection<T> inputs) where T : IArmTemplateInput
        {
            var output = new List<MissingInput>();
            var inputType = typeof(T) == typeof(ArmTemplateVariable)
                ? InputTypes.Variable
                : InputTypes.Parameter;

            var referencesInExpression = inputType == InputTypes.Variable
                ? expression.VariableNames
                : expression.ParameterNames;

            var missingInputs = inputs == null || inputs.Equals(default(T))
                ? referencesInExpression
                : referencesInExpression.Except(inputs.Select(i => i.Name));

            if (missingInputs.Any())
            {
                missingInputs.ToList()
                    .ForEach(i => output.Add(new MissingInput(i, inputType)));
            }
            return output;
        }
    }
}

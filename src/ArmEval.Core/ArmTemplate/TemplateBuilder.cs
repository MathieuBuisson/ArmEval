using ArmEval.Core.UserInputs;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
    }
}

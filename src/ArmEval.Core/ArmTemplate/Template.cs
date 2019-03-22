using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using ArmEval.Core.UserInputs;

namespace ArmEval.Core.ArmTemplate
{
    public class Template
    {
        [JsonProperty("$schema")]
        public string Schema { get; set; }

        [JsonProperty("contentVersion")]
        public string ContentVersion { get; set; }

        [JsonProperty("parameters")]
        public IDictionary<string, object> Parameters { get; set; }

        [JsonProperty("variables")]
        public IDictionary<string, object> Variables { get; set; }

        [JsonProperty("resources")]
        public List<object> Resources { get; set; }

        [JsonProperty("outputs")]
        public IDictionary<string, TemplateOutput> Outputs { get; set; }

        public Template()
        {
            Schema = @"https://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#";
            ContentVersion = "1.0.0.0";
            Parameters = new Dictionary<string, object>();
            Variables = new Dictionary<string, object>();
            Resources = new List<object>();
            Outputs = new Dictionary<string, TemplateOutput>();
        }

        public void AddExpression(ArmTemplateExpression expression, ArmValueTypes expectedOutputType)
        {
            var outputName = "expression";
            var outputTypeName = expectedOutputType.ToString();
            var outputObj = new TemplateOutput();
            outputObj.Type = outputTypeName;
            outputObj.Value = expression.Text;
            
            var output = new KeyValuePair<string, TemplateOutput>(outputName, outputObj);
            Outputs.Add(output);
        }

        public void AddExpression(ArmTemplateExpression expression, ArmValueTypes expectedOutputType,
            List<ArmTemplateVariable> inputVariables)
        {
            AddExpression(expression, expectedOutputType);

            if (expression.VariableNames.Any())
            {
                var missingVariables = expression.VariableNames.Except(inputVariables.Select(v => v.Name));
                if (missingVariables.Any())
                {
                    var missingString = String.Join(", ", missingVariables);
                    throw new ArgumentNullException($"Please specify a value for the following variable(s) : {missingString}");
                }

                AddInputVariables(inputVariables);
            }
        }


        public void AddInputVariables(List<ArmTemplateVariable> inputVariables)
        {
            inputVariables.ForEach(v => Variables.Add(v.Name, v.Value));
        }

        public void AddInputParameters(List<ArmTemplateParameter> inputParameters)
        {
            inputParameters.ForEach(p => Parameters.Add(p.Name, p.Value));
        }
    }
}

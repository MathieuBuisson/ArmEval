using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using ArmEval.Core.UserInputs;
using System.Collections.ObjectModel;
using ArmEval.Core.Extensions;

namespace ArmEval.Core.ArmTemplate
{
    public class Template : ITemplate
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
        public IEnumerable<object> Resources { get; set; }

        [JsonProperty("outputs")]
        public IDictionary<string, TemplateOutput> Outputs { get; set; }

        public Template()
        {
            Schema = @"https://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#";
            ContentVersion = "1.0.0.0";
            Parameters = new Dictionary<string, object>();
            Variables = new Dictionary<string, object>();
            Resources = new Collection<object>();
            Outputs = new Dictionary<string, TemplateOutput>();
        }

        public void AddExpression(ArmTemplateExpression expression, ArmValueTypes expectedOutputType)
        {
            var outputName = "expression";
            var outputTypeName = expectedOutputType.ToString().FirstCharToUpper();
            var outputObj = new TemplateOutput();
            outputObj.Type = outputTypeName;
            outputObj.Value = expression.Text;
            
            var output = new KeyValuePair<string, TemplateOutput>(outputName, outputObj);
            Outputs.Add(output);
        }

        public void AddExpression(ArmTemplateExpression expression, ArmValueTypes expectedOutputType,
            ICollection<ArmTemplateVariable> inputVariables)
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

        public void AddInputVariables(ICollection<ArmTemplateVariable> inputVariables)
        {
            foreach (var inputVar in inputVariables)
            {
                Variables.Add(inputVar.Name, inputVar.Value);
            }
        }

        public void AddInputParameters(ICollection<ArmTemplateParameter> inputParameters)
        {
            foreach (var inputParam in inputParameters)
            {
                Parameters.Add(inputParam.Name, inputParam.Value);
            }
        }
    }
}

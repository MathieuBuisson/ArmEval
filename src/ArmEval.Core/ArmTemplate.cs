using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ArmEval.Core
{
    public class ArmTemplate
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
        public IDictionary<string, ArmTemplateOutput> Outputs { get; set; }

        public ArmTemplate()
        {
            Schema = @"https://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#";
            ContentVersion = "1.0.0.0";
            Parameters = new Dictionary<string, object>();
            Variables = new Dictionary<string, object>();
            Resources = new List<object>();
            Outputs = new Dictionary<string, ArmTemplateOutput>();
        }

        public void AddExpression(ArmTemplateExpression expression, ArmValueTypes expectedOutputType)
        {
            var outputName = "expression";
            var outputTypeName = expectedOutputType.ToString();
            var outputObj = new ArmTemplateOutput();
            outputObj.Type = outputTypeName;
            outputObj.Value = expression.Text;
            
            var output = new KeyValuePair<string, ArmTemplateOutput>(outputName, outputObj);
            Outputs.Add(output);
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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ArmEval.Core
{
    public class ArmTemplateBase
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
        public IDictionary<string, object> Outputs { get; set; }

        public ArmTemplateBase()
        {
            Schema = @"https://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#";
            ContentVersion = "1.0.0.0";
            Parameters = new Dictionary<string, object>();
            Variables = new Dictionary<string, object>();
            Resources = new List<object>();
            Outputs = new Dictionary<string, object>();
        }
    }
}

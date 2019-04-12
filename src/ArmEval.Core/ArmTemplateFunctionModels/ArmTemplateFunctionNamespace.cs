
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ArmEval.Core.ArmTemplateFunctionModels
{
    public class FunctionNamespace
    {
        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("members")]
        public IDictionary<string, UserDefinedFunction> Members { get; set; }
    }
}

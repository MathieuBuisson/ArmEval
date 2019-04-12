using Newtonsoft.Json;
using System.Collections.Generic;

namespace ArmEval.Core.ArmTemplateFunctionModels
{
    public class UserDefinedFunction
    {
        [JsonProperty("parameters")]
        public List<UserDefinedFunctionParameter> Parameters { get; set; }

        [JsonProperty("output")]
        public UserDefinedFunctionOutput Output { get; set; }
    }
}

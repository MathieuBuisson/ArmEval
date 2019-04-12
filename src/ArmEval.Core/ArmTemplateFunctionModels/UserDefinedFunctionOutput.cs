using Newtonsoft.Json;

namespace ArmEval.Core.ArmTemplateFunctionModels
{
    public class UserDefinedFunctionOutput
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

    }
}

using ArmEval.Core.ArmClient;
using ArmEval.Core.ArmTemplate;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ArmEval.Core.UserInputs
{
    public class ArmTemplateFunction
    {
        public string Text { get; }
        public string Namespace { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> ParameterNames { get; private set; }

        public readonly Regex[] unsupportedFunctionsPatterns = {
            new Regex(@"resourceId\("),
            new Regex(@"reference\("),
            new Regex(@"list.*\("),

            // Not supported by subscription-level deployments (https://docs.microsoft.com/en-us/azure/azure-resource-manager/deploy-to-subscription#use-template-functions)
            new Regex(@"resourceGroup\(")
        };

        public ArmTemplateFunction(string functionText)
        {
            if (string.IsNullOrEmpty(functionText))
            {
                throw new ArgumentNullException(nameof(functionText));
            }

            // If ArmTemplateFunction.TryParse(functionText)
            // set object properties
        }

        public static bool TryParse(string text, out ArmTemplateFunction function)
        {

            function = null;
            return false;
        }
        //public object Invoke(IArmDeployment deployment,
        //    ICollection<ArmTemplateFunctionParameter> functionParameters)
        //{
        //    if (deployment is null)
        //    {
        //        throw new ArgumentNullException(nameof(deployment));
        //    }

        //    var templateBuilder = new TemplateBuilder()
        //        .AddFunction(this, functionParameters)
        //        .AddFunctionInvocation(this, functionParameters);

        //    if (templateBuilder.MissingInputs.Any())
        //    {
        //        return templateBuilder.MissingInputs;
        //    }

        //    deployment.Deployment.Properties.Template = templateBuilder.Template;

        //    string deploymentOutputs = deployment.Invoke();
        //    var expressionOutput = JToken.Parse(deploymentOutputs)["expression"];
        //    var expressionOutputStr = expressionOutput.ToString();

        //    var output = JsonConvert.DeserializeObject(expressionOutputStr);
        //    return output;
        //}

        private IEnumerable<string> getFunctionParameters(string text)
        {
            var paramNames = new List<string>();

            return paramNames;
        }
    }
}

using System;
using ArmEval.Core.ArmTemplate;
using ArmEval.Core.AzureClient;
using ArmEval.Core.UserInputs;
using ArmEval.Core.Utils;
using Autofac;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Extensions.Configuration;

namespace ArmEval.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var config = scope.Resolve<IConfigurationRoot>();
                var client = scope.Resolve<IResourceManagementClient>();
                client.SubscriptionId = config["SubscriptionId"];

                var expression = new ArmTemplateExpression(@"[concat('string12', 'string56')]");
                var template = new Template();
                template.AddExpression(expression, ArmValueTypes.@string);
                var deployment = new ArmDeployment(client, $"ArmEval{UniqueString.Create(5)}", template, "North Europe");
                var result = expression.Invoke(deployment);

                Console.WriteLine(result.Type);
                Console.WriteLine(result.Value);

                Console.ReadKey();
            }
        }
    }
}

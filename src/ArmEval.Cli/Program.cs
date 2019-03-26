using System;
using ArmEval.Core.ArmTemplate;
using ArmEval.Core.ArmClient;
using ArmEval.Core.UserInputs;
using ArmEval.Core.Extensions;
using Autofac;

namespace ArmEval.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<IApplication>();
                app.Init();

                var expression = new ArmTemplateExpression(@"[concat('strg12', 'strg56')]");
                
                app.Template.AddExpression(expression, ArmValueTypes.@string);
                var deployment = new ArmDeployment(app.Client, app.ResourceGroup, app.Template);
                var result = expression.Invoke(deployment);

                Console.WriteLine(result.Type);
                Console.WriteLine(result.Value);
                app.ResourceGroup.DeleteIfExists(app.Client);
            }
            Console.ReadKey();
        }
    }
}

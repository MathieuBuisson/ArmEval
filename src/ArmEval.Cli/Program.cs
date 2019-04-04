using System;
using ArmEval.Core.ArmTemplate;
using ArmEval.Core.ArmClient;
using ArmEval.Core.UserInputs;
using ArmEval.Core.Extensions;
using Autofac;
using System.Collections.Generic;

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

                var expression = new ArmTemplateExpression(@"[contains(createArray('one', 'two'), variables('twoString')]");
                
                var deployment = new ArmDeployment(app.Client, app.ResourceGroup);
                var inputVariables = new List<ArmTemplateVariable>()
                {
                    new ArmTemplateVariable("twoString", "two")
                };
                var result = expression.Invoke(deployment, ArmValueTypes.@bool, inputVariables);

                Console.WriteLine(result);
                app.ResourceGroup.DeleteIfExists(app.Client);
            }
            Console.ReadKey();
        }
    }
}

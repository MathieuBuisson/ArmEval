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

                var expression = new ArmTemplateExpression(@"[variables('obj').Property1]");
                
                var deployment = new ArmDeployment(app.Client, app.ResourceGroup);
                var inputVariables = new List<ArmTemplateVariable>()
                {
                    new ArmTemplateVariable("obj", new {Property1 = "customString", Property2 = true})
                };
                try
                {
                    var result = expression.Invoke(deployment, ArmValueTypes.@string, inputVariables);
                    Console.WriteLine(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    app.ResourceGroup.DeleteIfExists(app.Client);
                }
            }
            Console.ReadKey();
        }
    }
}

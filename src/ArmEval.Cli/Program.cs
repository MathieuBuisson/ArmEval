﻿using System;
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

                var expression = new ArmTemplateExpression(@"[mul(6, 3)]");
                
                app.Template.AddExpression(expression, ArmValueTypes.@int);
                var deployment = new ArmDeployment(app.Client, app.ResourceGroup);
                var result = expression.Invoke(deployment, ArmValueTypes.@int);

                Console.WriteLine(result);
                app.ResourceGroup.DeleteIfExists(app.Client);
            }
            Console.ReadKey();
        }
    }
}

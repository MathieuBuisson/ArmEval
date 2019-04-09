﻿using System;
using static System.Console;
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

                var expression = new ArmTemplateExpression(@"[parameters('obj').Property1]");
                
                var deployment = new ArmDeployment(app.Client, app.ResourceGroup);
                var inputVariables = new List<ArmTemplateVariable>()
                {
                    new ArmTemplateVariable("num2", 7)
                };
                var inputParams = new List<ArmTemplateParameter>()
                {
                    new ArmTemplateParameter("obj", new {Property1 = "customString", Property2 = true}, "object")
                };

                try
                {
                    var result = expression.Invoke(deployment, ArmValueTypes.@string, null, inputVariables);
                    if (result is List<MissingInput>)
                    {
                        var missingInputs = result as List<MissingInput>;
                        missingInputs.ForEach(m => WriteLine($"Please enter a value for {m.InputType} \"{m.Name}\"."));
                    }
                }
                catch (Exception ex)
                {
                    WriteLine(ex.Message);
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

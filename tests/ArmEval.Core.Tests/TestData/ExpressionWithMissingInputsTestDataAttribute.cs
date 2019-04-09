using ArmEval.Core.ArmTemplate;
using ArmEval.Core.UserInputs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace ArmEval.Core.Tests.TestData
{
    public class ExpressionWithMissingInputsTestDataAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            return _data;
        }

        private readonly List<object[]> _data = new List<object[]>
        {
            // Object properties represent (respectively) : expression text, input parameters, input variables, expected missing inputs
            new object[]
            {
                @"[concat(variables('str1'), variables('str2'))]",
                new List<ArmTemplateParameter>(){ },
                new List<ArmTemplateVariable>(){ new ArmTemplateVariable("str2", "value") },
                new List<MissingInput>(){ new MissingInput("str1", InputTypes.Variable) }
            },
            new object[]
            {
                @"[concat(parameters('str1'), parameters('str2'))]",
                new List<ArmTemplateParameter>(){ new ArmTemplateParameter("str1", "value", "string") },
                null,
                new List<MissingInput>(){ new MissingInput("str2", InputTypes.Parameter) }
            },
            new object[]
            {
                @"[add(variables('num1'), parameters('num2'))]",
                null,
                new List<ArmTemplateVariable>(){ },
                new List<MissingInput>(){ new MissingInput("num1", InputTypes.Variable), new MissingInput("num2", InputTypes.Parameter) }
            },
            new object[]
            {
                @"[add(variables('num1'), variables('num2'))]",
                null,
                null,
                new List<MissingInput>(){ new MissingInput("num1", InputTypes.Variable), new MissingInput("num2", InputTypes.Variable) }
            }
        };
    }
}

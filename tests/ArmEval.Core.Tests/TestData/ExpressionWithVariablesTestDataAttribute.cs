using ArmEval.Core.ArmTemplate;
using ArmEval.Core.UserInputs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit.Sdk;

namespace ArmEval.Core.Tests.TestData
{
    public class ExpressionWithVariablesTestDataAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            return _data;
        }

        private readonly List<object[]> _data = new List<object[]>
        {
            // Object properties represent (respectively) : expression text, input variables, output value type, output value, expected output string
            new object[]
            {
                @"[concat(variables('firstString'), 'string56')]",
                new List<ArmTemplateVariable>(){ new ArmTemplateVariable("firstString", "string12") },
                ArmValueTypes.@string,
                "string12string56",
                $"{{{Environment.NewLine}  \"type\": \"String\",{Environment.NewLine}  \"value\": \"string12string56\"{Environment.NewLine}}}"
            },
            new object[]
            {
                @"[mod(variables('num1'), 3)]",
                new List<ArmTemplateVariable>(){ new ArmTemplateVariable("num1", 7) },
                ArmValueTypes.@int,
                "1",
                $"{{{Environment.NewLine}  \"type\": \"Int\",{Environment.NewLine}  \"value\": 1{Environment.NewLine}}}"
            },
            new object[]
            {
                @"[mul(variables('num1'), variables('num2'))]",
                new List<ArmTemplateVariable>(){ new ArmTemplateVariable("num1", 6), new ArmTemplateVariable("num2", 3) },
                ArmValueTypes.@int,
                "18",
                $"{{{Environment.NewLine}  \"type\": \"Int\",{Environment.NewLine}  \"value\": 18{Environment.NewLine}}}"
            },
            new object[]
            {
                @"[contains(createArray(variables('string1'), variables('string2')), 'two')]",
                new List<ArmTemplateVariable>(){ new ArmTemplateVariable("string1", "one"), new ArmTemplateVariable("string2", "two") },
                ArmValueTypes.@bool,
                "true",
                $"{{{Environment.NewLine}  \"type\": \"Bool\",{Environment.NewLine}  \"value\": true{Environment.NewLine}}}"
            },
            new object[]
            {
                @"[not(equals(1, variables('intTen')))]",
                new List<ArmTemplateVariable>(){ new ArmTemplateVariable("intTen", 10) },
                ArmValueTypes.@bool,
                "true",
                $"{{{Environment.NewLine}  \"type\": \"Bool\",{Environment.NewLine}  \"value\": true{Environment.NewLine}}}"
            },
            new object[]
            {
                @"[createArray(1, variables('num2'), variables('num3'))]",
                new List<ArmTemplateVariable>(){ new ArmTemplateVariable("num2", 2), new ArmTemplateVariable("num3", 3) },
                ArmValueTypes.array,
                @"[
      1,
      2,
      3
    ]",
                $"{{{Environment.NewLine}  \"type\": \"Array\",{Environment.NewLine}  \"value\": [{Environment.NewLine}    1,{Environment.NewLine}    2,{Environment.NewLine}    3{Environment.NewLine}  ]{Environment.NewLine}}}"
            }
        };
    }
}

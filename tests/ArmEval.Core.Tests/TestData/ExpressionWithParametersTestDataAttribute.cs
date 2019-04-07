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
    public class ExpressionWithParametersTestDataAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            return _data;
        }

        private readonly List<object[]> _data = new List<object[]>
        {
            // Object properties represent (respectively) : expression text, input Parameters, input variables, output value type, output value, expected output string
            new object[]
            {
                @"[concat(parameters('firstString'), 'string56')]",
                new List<ArmTemplateParameter>(){ new ArmTemplateParameter("firstString", "string12", "string") },
                new List<ArmTemplateVariable>(){ new ArmTemplateVariable("strVar", "strVar") },
                ArmValueTypes.@string,
                "string12string56",
                $"{{{Environment.NewLine}  \"type\": \"String\",{Environment.NewLine}  \"value\": \"string12string56\"{Environment.NewLine}}}"
            },
            new object[]
            {
                @"[mod(parameters('num1'), 3)]",
                new List<ArmTemplateParameter>(){ new ArmTemplateParameter("num1", 7, "int") },
                new List<ArmTemplateVariable>(){ new ArmTemplateVariable("strVar", "strVar") },
                ArmValueTypes.@int,
                "1",
                $"{{{Environment.NewLine}  \"type\": \"Int\",{Environment.NewLine}  \"value\": 1{Environment.NewLine}}}"
            },
            new object[]
            {
                @"[mul(parameters('num1'), parameters('num2'))]",
                new List<ArmTemplateParameter>(){ new ArmTemplateParameter("num1", 6, "int"), new ArmTemplateParameter("num2", 3, "int") },
                new List<ArmTemplateVariable>(){ new ArmTemplateVariable("strVar", "strVar") },
                ArmValueTypes.@int,
                "18",
                $"{{{Environment.NewLine}  \"type\": \"Int\",{Environment.NewLine}  \"value\": 18{Environment.NewLine}}}"
            },
            new object[]
            {
                @"[contains(createArray(parameters('str1'), variables('str2')), 'two')]",
                new List<ArmTemplateParameter>(){ new ArmTemplateParameter("str1", "one", "string") },
                new List<ArmTemplateVariable>(){ new ArmTemplateVariable("str2", "two") },
                ArmValueTypes.@bool,
                "true",
                $"{{{Environment.NewLine}  \"type\": \"Bool\",{Environment.NewLine}  \"value\": true{Environment.NewLine}}}"
            },
            new object[]
            {
                @"[not(equals(1, parameters('intTen')))]",
                new List<ArmTemplateParameter>(){ new ArmTemplateParameter("intTen", 10, "int") },
                new List<ArmTemplateVariable>(){ new ArmTemplateVariable("intTen", 1) },
                ArmValueTypes.@bool,
                "true",
                $"{{{Environment.NewLine}  \"type\": \"Bool\",{Environment.NewLine}  \"value\": true{Environment.NewLine}}}"
            },
            new object[]
            {
                @"[createArray(1, parameters('num2'), parameters('num3'))]",
                new List<ArmTemplateParameter>(){ new ArmTemplateParameter("num2", 2, "int"), new ArmTemplateParameter("num3", 3, "int") },
                new List<ArmTemplateVariable>(){ new ArmTemplateVariable("strVar", "strVar") },
                ArmValueTypes.array,
                @"[
      1,
      2,
      3
    ]",
                $"{{{Environment.NewLine}  \"type\": \"Array\",{Environment.NewLine}  \"value\": [{Environment.NewLine}    1,{Environment.NewLine}    2,{Environment.NewLine}    3{Environment.NewLine}  ]{Environment.NewLine}}}"
            },
                        new object[]
            {
                @"[mul(parameters('num1'), variables('num2'))]",
                new List<ArmTemplateParameter>(){ new ArmTemplateParameter("num1", 6, "int") },
                new List<ArmTemplateVariable>(){ new ArmTemplateVariable("num2", 4) },
                ArmValueTypes.@int,
                "24",
                $"{{{Environment.NewLine}  \"type\": \"Int\",{Environment.NewLine}  \"value\": 24{Environment.NewLine}}}"
            }
        };
    }
}

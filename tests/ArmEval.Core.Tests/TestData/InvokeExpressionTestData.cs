using ArmEval.Core.ArmTemplate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ArmEval.Core.Tests.TestData
{
    public class InvokeExpressionTestData : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            // Object properties represent (respectively) : expression text, output value type, output value, expected output string
            new object[]
            {
                @"[concat('string12', 'string56')]",
                ArmValueTypes.@string,
                "string12string56",
                $"{{{Environment.NewLine}  \"type\": \"String\",{Environment.NewLine}  \"value\": \"string12string56\"{Environment.NewLine}}}"
            },
            new object[]
            {
                @"[mod(7, 3)]",
                ArmValueTypes.@int,
                "1",
                $"{{{Environment.NewLine}  \"type\": \"Int\",{Environment.NewLine}  \"value\": 1{Environment.NewLine}}}"
            },
            new object[]
            {
                @"[mul(6, 3)]",
                ArmValueTypes.@int,
                "18",
                $"{{{Environment.NewLine}  \"type\": \"Int\",{Environment.NewLine}  \"value\": 18{Environment.NewLine}}}"
            },
            new object[]
            {
                @"[contains(createArray('one', 'two'), 'two')]",
                ArmValueTypes.@bool,
                "true",
                $"{{{Environment.NewLine}  \"type\": \"Bool\",{Environment.NewLine}  \"value\": true{Environment.NewLine}}}"
            },
            new object[]
            {
                @"[not(equals(1, 10))]",
                ArmValueTypes.@bool,
                "true",
                $"{{{Environment.NewLine}  \"type\": \"Bool\",{Environment.NewLine}  \"value\": true{Environment.NewLine}}}"
            },
            new object[]
            {
                @"[createArray(1, 2, 3)]",
                ArmValueTypes.array,
                @"[
      1,
      2,
      3
    ]",
                $"{{{Environment.NewLine}  \"type\": \"Array\",{Environment.NewLine}  \"value\": [{Environment.NewLine}    1,{Environment.NewLine}    2,{Environment.NewLine}    3{Environment.NewLine}  ]{Environment.NewLine}}}"
            }
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

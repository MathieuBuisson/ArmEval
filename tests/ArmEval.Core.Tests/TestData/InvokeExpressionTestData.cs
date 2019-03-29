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
                "{\r\n  \"type\": \"String\",\r\n  \"value\": \"string12string56\"\r\n}"
            },
            new object[]
            {
                @"[mod(7, 3)]",
                ArmValueTypes.@int,
                "1",
                "{\r\n  \"type\": \"Int\",\r\n  \"value\": 1\r\n}"
            },
            new object[]
            {
                @"[mul(6, 3)]",
                ArmValueTypes.@int,
                "18",
                "{\r\n  \"type\": \"Int\",\r\n  \"value\": 18\r\n}"
            },
            new object[]
            {
                @"[contains(createArray('one', 'two'), 'two')]",
                ArmValueTypes.@bool,
                "true",
                "{\r\n  \"type\": \"Bool\",\r\n  \"value\": true\r\n}"
            },
            new object[]
            {
                @"[not(equals(1, 10))]",
                ArmValueTypes.@bool,
                "true",
                "{\r\n  \"type\": \"Bool\",\r\n  \"value\": true\r\n}"
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
                "{\r\n  \"type\": \"Array\",\r\n  \"value\": [\r\n    1,\r\n    2,\r\n    3\r\n  ]\r\n}"
            }
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

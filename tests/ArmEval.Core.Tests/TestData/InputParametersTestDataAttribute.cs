using ArmEval.Core.UserInputs;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace ArmEval.Core.Tests.TestData
{
    public class InputParametersTestDataAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            return _data;
        }

        private readonly List<object[]> _data = new List<object[]>
        {
            // Object properties represent (respectively) : expression, input parameters
            new object[] {new ArmTemplateExpression(@"[parameters('name')]"), new List<ArmTemplateParameter>()
                {
                    new ArmTemplateParameter("name", "testVm", "string")
                }
            },
            new object[] {new ArmTemplateExpression(@"[add(parameters('num1'), 9)]"), new List<ArmTemplateParameter>()
                {
                    new ArmTemplateParameter("num1", 8, "int")
                }
            },
            new object[] {new ArmTemplateExpression(@"[mul(parameters('var1'), parameters('var2'))]"), new List<ArmTemplateParameter>()
                {
                    new ArmTemplateParameter("var1", 3, "int"),
                    new ArmTemplateParameter("var2", 7, "int")
                }
            },
            new object[] {new ArmTemplateExpression(@"[not(parameters('boolTrue'))]"), new List<ArmTemplateParameter>()
                {
                    new ArmTemplateParameter("boolTrue", true, "bool")
                }
            },
            new object[] {new ArmTemplateExpression(@"[take(parameters('testArray'), 2)]"), new List<ArmTemplateParameter>()
                {
                    new ArmTemplateParameter("testArray", new string[]{ "one", "two", "three" }, "array")
                }
            },
            new object[] {new ArmTemplateExpression(@"[parameters('obj').Property2]"), new List<ArmTemplateParameter>()
                {
                    new ArmTemplateParameter("obj", new {Property1 = "customString", Property2 = true}, "object")
                }
            }
        };
    }
}

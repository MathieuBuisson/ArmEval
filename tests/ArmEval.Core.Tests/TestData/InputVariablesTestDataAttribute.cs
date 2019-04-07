using ArmEval.Core.UserInputs;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace ArmEval.Core.Tests.TestData
{
    public class InputVariablesTestDataAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            return _data;
        }

        private readonly List<object[]> _data = new List<object[]>
        {
            // Object properties represent (respectively) : expression, input variables
            new object[] {new ArmTemplateExpression(@"[variables('name')]"), new List<ArmTemplateVariable>()
                {
                    new ArmTemplateVariable("name", "testVm")
                }
            },
            new object[] {new ArmTemplateExpression(@"[add(variables('num1'), 9)]"), new List<ArmTemplateVariable>()
                {
                    new ArmTemplateVariable("num1", 8)
                }
            },
            new object[] {new ArmTemplateExpression(@"[mul(variables('var1'), variables('var2'))]"), new List<ArmTemplateVariable>()
                {
                    new ArmTemplateVariable("var1", 3),
                    new ArmTemplateVariable("var2", 7)
                }
            },
            new object[] {new ArmTemplateExpression(@"[not(variables('boolTrue'))]"), new List<ArmTemplateVariable>()
                {
                    new ArmTemplateVariable("boolTrue", true)
                }
            },
            new object[] {new ArmTemplateExpression(@"[take(variables('testArray'), 2)]"), new List<ArmTemplateVariable>()
                {
                    new ArmTemplateVariable("testArray", new string[]{ "one", "two", "three" })
                }
            },
            new object[] {new ArmTemplateExpression(@"[variables('obj').Property2]"), new List<ArmTemplateVariable>()
                {
                    new ArmTemplateVariable("obj", new {Property1 = "customString", Property2 = true})
                }
            }
        };
    }
}

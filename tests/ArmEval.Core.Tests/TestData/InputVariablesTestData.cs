using ArmEval.Core.ArmTemplate;
using ArmEval.Core.UserInputs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ArmEval.Core.Tests.TestData
{
    public class InputVariablesTestData : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            // Object properties represent (respectively) : expression text, output type, input variables
            new object[] {@"[variables('vmName')]", ArmValueTypes.@string, new List<ArmTemplateVariable>()
                {
                    new ArmTemplateVariable("vmName", "testVm")
                }
            },
            new object[] {@"[add(variables('number1'), 9)]", ArmValueTypes.@int, new List<ArmTemplateVariable>()
                {
                    new ArmTemplateVariable("number1", 8)
                }
            },
            new object[] {@"[mul(variables('var1'), variables('var2'))]", ArmValueTypes.@int, new List<ArmTemplateVariable>()
                {
                    new ArmTemplateVariable("var1", 3),
                    new ArmTemplateVariable("var2", 7)
                }
            },
            new object[] {@"[variables('obj').Property2]", ArmValueTypes.@object, new List<ArmTemplateVariable>()
                {
                    new ArmTemplateVariable("obj", new {Property1 = "customString", Property2 = true})
                }
            }
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

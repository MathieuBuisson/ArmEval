using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ArmEval.Core.UserInputs
{
    public class ArmTemplateVariable
    {
        public string Name { get; }
        public object Value { get; }
        public Regex ReferencePattern { get; }

        public ArmTemplateVariable(string name, object value)
        {
            Name = name;
            Value = value;

            var stringBuilder = new StringBuilder(@"variables\([""']{1}");
            stringBuilder.Append(name).Append(@"[""']{1}\)");
            var patternString = stringBuilder.ToString();
            ReferencePattern = new Regex(patternString);
        }
    }
}

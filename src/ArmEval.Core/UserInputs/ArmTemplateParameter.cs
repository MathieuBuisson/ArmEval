using ArmEval.Core.ArmTemplate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ArmEval.Core.UserInputs
{
    public class ArmTemplateParameter : IArmTemplateInput
    {
        public string Name { get; }
        public object Value { get; }
        public Regex ReferencePattern { get; }

        private readonly string validTypes = string.Join(", ", Enum.GetNames(typeof(ArmValueTypes)));
        private string _type;
        public string Type
        {
            get => _type;
            set => _type = Enum.IsDefined(typeof(ArmValueTypes), value) ?
                value :
                throw new ArgumentOutOfRangeException("Type", $"Valid value are : {validTypes}");
        }

        public ArmTemplateParameter(string name, object value, string type)
        {
            Name = name;
            Value = value;
            Type = type;

            var stringBuilder = new StringBuilder(@"parameters\([""']{1}");
            stringBuilder.Append(name).Append(@"[""']{1}\)");
            var patternString = stringBuilder.ToString();
            ReferencePattern = new Regex(patternString);
        }
    }
}

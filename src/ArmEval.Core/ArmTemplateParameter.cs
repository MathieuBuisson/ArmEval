using System;
using System.Collections.Generic;
using System.Text;

namespace ArmEval.Core
{
    public class ArmTemplateParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }

        private readonly string validTypes = String.Join(", ", Enum.GetNames(typeof(ArmValueTypes)));
        private string _type;
        public string Type
        {
            get => _type;
            set => _type = Enum.IsDefined(typeof(ArmValueTypes), value) ?
                value :
                throw new ArgumentOutOfRangeException("Type", $"Valid value are : {validTypes}");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace ArmEval.Core
{
    public class ArmTemplateOutput
    {
        private string _type;
        public string Type
        {
            get => _type;
            set => _type = value.ToLower();
        }
        public string Value { get; set; }
    }
}

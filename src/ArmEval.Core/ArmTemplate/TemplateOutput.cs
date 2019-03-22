using System;
using System.Collections.Generic;
using System.Text;

namespace ArmEval.Core.ArmTemplate
{
    public class TemplateOutput
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

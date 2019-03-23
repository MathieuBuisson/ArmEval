using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ArmEval.Core.UserInputs
{
    public class ExpressionValidationResult
    {
        public bool Success { get; set; }
        public Exception Exception { get; set; }

        public ExpressionValidationResult()
        {
            Success = true;
        }
    }
}

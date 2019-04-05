using ArmEval.Core.UserInputs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArmEval.Core.ArmTemplate
{
    public class ExpressionInputsException : ArgumentNullException
    {
        public ExpressionInputsException(string inputName)
            : base(nameof(inputName), $"Enter a value for the following variable/parameter : {inputName}")
        {
        }

        public ExpressionInputsException()
        {
        }

        public ExpressionInputsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ArmEval.Core
{
    public class ExpressionValidationResult
    {
        public bool Success { get; set; }
        public List<Exception> ExceptionList { get; private set; }

        public ExpressionValidationResult()
        {
            ExceptionList = new List<Exception>();
            Success = true;
        }

        public void AddException<E>(string message) where E : Exception
        {
            var exception = Activator.CreateInstance(typeof(E), message) as E;
            ExceptionList.Add(exception);
        }
    }
}

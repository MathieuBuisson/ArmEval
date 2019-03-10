using System;
using System.Collections.Generic;
using System.Text;

namespace ArmEval.Core.Tests
{

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class OrderAttribute : Attribute
    {
        public OrderAttribute(int order)
        {
            Order = order;
        }

        public int Order { get; private set; }
    }
}

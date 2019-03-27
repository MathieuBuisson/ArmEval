using System;
using System.Collections.Generic;
using System.Text;

namespace ArmEval.Core.Extensions
{
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
                throw new ArgumentException("There is no first letter");

            char[] a = inputString.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
    }
}

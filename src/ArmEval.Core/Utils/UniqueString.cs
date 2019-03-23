using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ArmEval.Core.Utils
{
    public static class UniqueString
    {
        public static string Create(int length)
        {
            var randomChars = Path.GetRandomFileName().ToCharArray();
            var filteredChars = randomChars
                .Where(c => Char.IsLetterOrDigit(c))
                .Take(length)
                .ToArray();
            return new string(filteredChars);
        }
    }
}

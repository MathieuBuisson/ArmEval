using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ArmEval.Core.ArmTemplate
{
    public static class ArmTypesConverter
    {
        public static List<ArmValueTypes> GetPossibleArmValueTypes()
        {
            var output = Enum.GetValues(typeof(ArmValueTypes))
                .OfType<ArmValueTypes>()
                .ToList();

            return output;
        }

        public static Type ConvertToDotnetType(ArmValueTypes armType)
        {
            Type dotnetType;
            switch (armType)
            {
                case ArmValueTypes.array:
                    dotnetType = typeof(Array);
                    break;
                case ArmValueTypes.@bool:
                    dotnetType = typeof(bool);
                    break;
                case ArmValueTypes.@int:
                    dotnetType = typeof(int);
                    break;
                case ArmValueTypes.@object:
                    dotnetType = typeof(object);
                    break;
                case ArmValueTypes.secureObject:
                    dotnetType = typeof(object);
                    break;
                case ArmValueTypes.securestring:
                    dotnetType = typeof(string);
                    break;
                case ArmValueTypes.@string:
                    dotnetType = typeof(string);
                    break;
                default:
                    throw new NotSupportedException();
            }
            return dotnetType;
        }
    }
}

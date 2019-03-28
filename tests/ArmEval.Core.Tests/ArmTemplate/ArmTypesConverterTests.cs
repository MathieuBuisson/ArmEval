using System;
using System.Collections.Generic;
using System.Linq;
using ArmEval.Core.ArmTemplate;
using Xunit;

namespace ArmEval.Core.Tests.ArmTemplate
{
    public class ArmTypesConverterTests
    {
        [Fact]
        public void GetPossibleArmValueTypes_ReturnsOnlyArmValueTypes()
        {
            var actual = ArmTypesConverter.GetPossibleArmValueTypes();
            actual.ForEach(t => Assert.IsType<ArmValueTypes>(t));
        }

        [Theory]
        [InlineData(ArmValueTypes.array, typeof(Array))]
        [InlineData(ArmValueTypes.@bool, typeof(bool))]
        [InlineData(ArmValueTypes.@int, typeof(int))]
        [InlineData(ArmValueTypes.@object, typeof(object))]
        [InlineData(ArmValueTypes.secureObject, typeof(object))]
        [InlineData(ArmValueTypes.securestring, typeof(string))]
        [InlineData(ArmValueTypes.@string, typeof(string))]
        public void ConvertToDotnetType_ReturnsExpectedDotnetType(ArmValueTypes armType,
            Type dotnetType)
        {
            var actual = ArmTypesConverter.ConvertToDotnetType(armType);
            
            Assert.Equal(dotnetType.ToString(), actual.ToString());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ArmEval.Core.Tests
{
    public class ArmClientConfigTests
    {
        [Fact]
        public void Constructor_SetsAllPropertiesFromEnvironment()
        {
            var actual = new ArmClientConfig();
            Assert.NotNull(actual.TenantId);
            Assert.NotNull(actual.ClientId);
            Assert.NotNull(actual.ClientSecret);
            Assert.NotNull(actual.Subscription);
        }
    }
}

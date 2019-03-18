using Microsoft.Azure.Management.ResourceManager;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ArmEval.Core.Tests
{
    public class ArmClientConfigTests
    {
        [Fact]
        public void Constructor_SetsAllProperties()
        {
            var actual = new ArmClientConfig();
            Assert.NotNull(actual.TenantId);
            Assert.NotNull(actual.ClientId);
            Assert.NotNull(actual.ClientSecret);
            Assert.NotNull(actual.Subscription);
            Assert.Matches(@"^ArmEval\d+$", actual.ResourceGroup);
            Assert.Equal("North Europe", actual.Location);
            Assert.IsAssignableFrom<IResourceManagementClient>(actual.Client);
        }
    }
}

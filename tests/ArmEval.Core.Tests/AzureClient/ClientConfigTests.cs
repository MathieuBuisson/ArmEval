using ArmEval.Core.AzureClient;
using Microsoft.Azure.Management.ResourceManager;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ArmEval.Core.Tests.AzureClient
{
    public class ClientConfigTests
    {
        [Fact]
        public void Constructor_SetsAllProperties()
        {
            var actual = new ClientConfig("testTenant", "TestClientId", "testSecret", "testSubscription");
            Assert.Equal("testTenant", actual.TenantId);
            Assert.Equal("TestClientId", actual.ClientId);
            Assert.Equal("testSecret", actual.ClientSecret);
            Assert.Equal("testSubscription", actual.Subscription);
            Assert.Matches(@"^ArmEval\w{5}$", actual.ResourceGroup);
            Assert.Equal("North Europe", actual.Location);
        }
    }
}

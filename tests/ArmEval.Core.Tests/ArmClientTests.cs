using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ArmEval.Core.Tests
{
    public class ArmClientTests
    {
        [Fact]
        public void Constructor_WithoutArguments_SetsConfigValuesFromConfig()
        {
            var armClient = new ArmClient();
            var actual = armClient.Config;

            Assert.NotNull(actual["TenantId"]);
            Assert.NotNull(actual["ClientId"]);
            Assert.NotNull(actual["ClientSecret"]);
            Assert.NotNull(actual["SubscriptionId"]);
        }

        [Fact]
        public void Constructor_WithoutArguments_SetsPropertiesFromConfig()
        {
            var actual = new ArmClient();

            Assert.NotNull(actual.TenantId);
            Assert.NotNull(actual.ClientId);
            Assert.NotNull(actual.ClientSecret);
            Assert.NotNull(actual.SubscriptionId);
        }

        [Fact]
        public void Constructor_WithArguments_SetsPropertiesFromArgs()
        {
            var tenantId = "testTenantId";
            var clientId = "testClientId";
            var secret = "testSecret";
            var subscription = "testSubscription";
            var actual = new ArmClient(tenantId, clientId, secret, subscription);

            Assert.Equal(tenantId, actual.TenantId);
            Assert.Equal(clientId, actual.ClientId);
            Assert.Equal(secret, actual.ClientSecret);
            Assert.Equal(subscription, actual.SubscriptionId);
        }

        [Fact]
        public void Create_AuthenticatesSuccessfullyWithArm()
        {
            var armClient = new ArmClient();
            using (var resourceManagementClient = armClient.Create())
            {
                Assert.NotNull(resourceManagementClient.Credentials);
            }
        }

        [Fact]
        public void Create_SetsClientSubscriptionToSpecifiedSubscription()
        {
            var armClient = new ArmClient();
            using (var actual = armClient.Create())
            {
                Assert.Equal("d267cdac-1b9b-4ee2-b6c8-7b6eee4e4d89", actual.SubscriptionId);
            }

        }
    }
}

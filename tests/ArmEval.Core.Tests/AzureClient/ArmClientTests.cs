using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using ArmEval.Core.AzureClient;

namespace ArmEval.Core.Tests.AzureClient
{
    public class ArmClientTests : IClassFixture<ArmClientTestConfig>
    {

        private readonly ArmClientTestConfig config;

        public ArmClientTests(ArmClientTestConfig conf)
        {
            config = conf;
        }

        [Fact]
        public void Constructor_WithIArmClientConfig_SetsConfigValuesFromConfig()
        {
            var actual = new ArmClient(config);

            Assert.NotNull(actual.TenantId);
            Assert.NotNull(actual.ClientId);
            Assert.NotNull(actual.ClientSecret);
            Assert.NotNull(actual.SubscriptionId);
        }

        [Fact]
        public void Constructor_WithIArmClientConfig_SetsPropertiesFromConfig()
        {
            var actual = new ArmClient(config);

            Assert.Equal(config.TenantId, actual.TenantId);
            Assert.Equal(config.ClientId, actual.ClientId);
            Assert.Equal(config.ClientSecret, actual.ClientSecret);
            Assert.Equal(config.Subscription, actual.SubscriptionId);
        }

        [Fact]
        public void Constructor_WithArguments_SetsPropertiesFromArgs()
        {
            var actual = new ArmClient(config.TenantId, config.ClientId, config.ClientSecret, config.Subscription);

            Assert.Equal(config.TenantId, actual.TenantId);
            Assert.Equal(config.ClientId, actual.ClientId);
            Assert.Equal(config.ClientSecret, actual.ClientSecret);
            Assert.Equal(config.Subscription, actual.SubscriptionId);
        }

        [Fact]
        public void Create_AuthenticatesSuccessfullyWithArm()
        {
            var config = new ArmClientConfig();
            var armClient = new ArmClient(config);
            using (var resourceManagementClient = armClient.Create())
            {
                Assert.NotNull(resourceManagementClient.Credentials);
            }
        }
    }
}

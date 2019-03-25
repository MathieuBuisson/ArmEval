using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using ArmEval.Core.AzureClient;

namespace ArmEval.Core.Tests.AzureClient
{
    public class ClientTests : IClassFixture<ClientTestConfig>
    {

        private readonly ClientTestConfig config;

        public ClientTests(ClientTestConfig conf)
        {
            config = conf;
        }

        [Fact]
        public void Constructor_WithIArmClientConfig_SetsConfigValuesFromConfig()
        {
            var actual = new Client(config);

            Assert.NotNull(actual.TenantId);
            Assert.NotNull(actual.ClientId);
            Assert.NotNull(actual.ClientSecret);
            Assert.NotNull(actual.SubscriptionId);
        }

        [Fact]
        public void Constructor_WithIArmClientConfig_SetsPropertiesFromConfig()
        {
            var actual = new Client(config);

            Assert.Equal(config.TenantId, actual.TenantId);
            Assert.Equal(config.ClientId, actual.ClientId);
            Assert.Equal(config.ClientSecret, actual.ClientSecret);
            Assert.Equal(config.Subscription, actual.SubscriptionId);
        }

        [Fact]
        public void Constructor_WithArguments_SetsPropertiesFromArgs()
        {
            var actual = new Client(config.TenantId, config.ClientId, config.ClientSecret, config.Subscription);

            Assert.Equal(config.TenantId, actual.TenantId);
            Assert.Equal(config.ClientId, actual.ClientId);
            Assert.Equal(config.ClientSecret, actual.ClientSecret);
            Assert.Equal(config.Subscription, actual.SubscriptionId);
        }

        [Fact]
        public void Create_AuthenticatesSuccessfullyWithArm()
        {
            var config = new ClientConfig();
            var armClient = new Client(config);
            using (var resourceManagementClient = armClient.Create())
            {
                Assert.NotNull(resourceManagementClient.Credentials);
            }
        }
    }
}

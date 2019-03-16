using Microsoft.Azure.Management.ResourceManager;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;

namespace ArmEval.Core.Tests
{
    public class ArmClientTests : IClassFixture<ArmClientTestConfig>
    {

        private readonly ArmClientTestConfig testConfig;

        public ArmClientTests(ArmClientTestConfig config)
        {
            testConfig = config;
        }

        [Fact]
        public void Constructor_WithIArmClientConfig_SetsConfigValuesFromConfig()
        {
            var actual = new ArmClient(testConfig);

            Assert.NotNull(actual.TenantId);
            Assert.NotNull(actual.ClientId);
            Assert.NotNull(actual.ClientSecret);
            Assert.NotNull(actual.SubscriptionId);
        }

        [Fact]
        public void Constructor_WithIArmClientConfig_SetsPropertiesFromConfig()
        {
            var actual = new ArmClient(testConfig);

            Assert.Equal(testConfig.TenantId, actual.TenantId);
            Assert.Equal(testConfig.ClientId, actual.ClientId);
            Assert.Equal(testConfig.ClientSecret, actual.ClientSecret);
            Assert.Equal(testConfig.Subscription, actual.SubscriptionId);
        }

        [Fact]
        public void Constructor_WithArguments_SetsPropertiesFromArgs()
        {
            var actual = new ArmClient(testConfig.TenantId, testConfig.ClientId, testConfig.ClientSecret, testConfig.Subscription);

            Assert.Equal(testConfig.TenantId, actual.TenantId);
            Assert.Equal(testConfig.ClientId, actual.ClientId);
            Assert.Equal(testConfig.ClientSecret, actual.ClientSecret);
            Assert.Equal(testConfig.Subscription, actual.SubscriptionId);
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

using Microsoft.Azure.Management.ResourceManager;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;

namespace ArmEval.Core.Tests
{
    public class ArmClientTests : IClassFixture<TestConfig>
    {

        private TestConfig testConfig;

        public ArmClientTests(TestConfig config)
        {
            testConfig = config;
        }

        [Fact]
        public void Constructor_WithIConfiguration_SetsConfigValuesFromConfig()
        {
            var actual = new ArmClient(testConfig.Config);

            Assert.NotNull(actual.TenantId);
            Assert.NotNull(actual.ClientId);
            Assert.NotNull(actual.ClientSecret);
            Assert.NotNull(actual.SubscriptionId);
        }

        [Fact]
        public void Constructor_WithIConfiguration_SetsPropertiesFromConfig()
        {
            var actual = new ArmClient(testConfig.Config);

            Assert.Equal(testConfig.TenantId, actual.TenantId);
            Assert.Equal(testConfig.ClientId, actual.ClientId);
            Assert.Equal(testConfig.Secret, actual.ClientSecret);
            Assert.Equal(testConfig.Subscription, actual.SubscriptionId);
        }

        [Fact]
        public void Constructor_WithArguments_SetsPropertiesFromArgs()
        {
            var actual = new ArmClient(testConfig.TenantId, testConfig.ClientId, testConfig.Secret, testConfig.Subscription);

            Assert.Equal(testConfig.TenantId, actual.TenantId);
            Assert.Equal(testConfig.ClientId, actual.ClientId);
            Assert.Equal(testConfig.Secret, actual.ClientSecret);
            Assert.Equal(testConfig.Subscription, actual.SubscriptionId);
        }
    }
}

using Microsoft.Azure.Management.ResourceManager;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ArmEval.Core.Tests
{
    [TestCaseOrderer("ArmEval.Core.Tests.NumericOrderer", "ArmEval.Core.Tests")]
    public class ResourceGroupsHelperTests : IClassFixture<ArmClientConfigRGHelperTests>
    {
        private readonly ArmClientConfigRGHelperTests testConfig;

        public ResourceGroupsHelperTests(ArmClientConfigRGHelperTests config)
        {
            testConfig = config;
        }

        [Fact, Order(1)]
        public void Exists_ResourceGroupDoesNotExist_ReturnsFalse()
        {
            var actual = ResourceGroupsHelper.Exists(testConfig.Client, testConfig.ResourceGroup);
            Assert.False(actual);
        }

        [Fact, Order(2)]
        public void CreateIfNotExists_ResourceGroupDoesNotExist_CreatesIt()
        {
            ResourceGroupsHelper.CreateIfNotExists(testConfig.Client, testConfig.ResourceGroup, testConfig.Location);
            Assert.True(ResourceGroupsHelper.Exists(testConfig.Client, testConfig.ResourceGroup));
        }

        [Fact, Order(3)]
        public void CreateIfNotExists_ResourceGroupExists_DoesNotThrow()
        {
            ResourceGroupsHelper.CreateIfNotExists(testConfig.Client, testConfig.ResourceGroup, testConfig.Location);
            Action act = () => {
                ResourceGroupsHelper.CreateIfNotExists(testConfig.Client, testConfig.ResourceGroup, testConfig.Location);
            };
            var ex = Record.Exception(act);

            Assert.Null(ex);
        }

        [Fact, Order(4)]
        public void Exists_ResourceGroupExists_ReturnsTrue()
        {
            var exists = ResourceGroupsHelper.Exists(testConfig.Client, testConfig.ResourceGroup);
            Assert.True(exists);
        }
    }
}

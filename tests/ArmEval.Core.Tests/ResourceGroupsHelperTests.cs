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
        private readonly ArmClientConfigRGHelperTests config;

        public ResourceGroupsHelperTests(ArmClientConfigRGHelperTests conf)
        {
            config = conf;
        }

        [Fact, Order(1)]
        public void Exists_ResourceGroupDoesNotExist_ReturnsFalse()
        {
            var actual = ResourceGroupsHelper.Exists(config.Client, config.ResourceGroup);
            Assert.False(actual);
        }

        [Fact, Order(2)]
        public void CreateIfNotExists_ResourceGroupDoesNotExist_CreatesIt()
        {
            ResourceGroupsHelper.CreateIfNotExists(config.Client, config.ResourceGroup, config.Location);
            Assert.True(ResourceGroupsHelper.Exists(config.Client, config.ResourceGroup));
        }

        [Fact, Order(3)]
        public void CreateIfNotExists_ResourceGroupExists_DoesNotThrow()
        {
            ResourceGroupsHelper.CreateIfNotExists(config.Client, config.ResourceGroup, config.Location);
            Action act = () => {
                ResourceGroupsHelper.CreateIfNotExists(config.Client, config.ResourceGroup, config.Location);
            };
            var ex = Record.Exception(act);

            Assert.Null(ex);
        }

        [Fact, Order(4)]
        public void Exists_ResourceGroupExists_ReturnsTrue()
        {
            var exists = ResourceGroupsHelper.Exists(config.Client, config.ResourceGroup);
            Assert.True(exists);
        }
    }
}

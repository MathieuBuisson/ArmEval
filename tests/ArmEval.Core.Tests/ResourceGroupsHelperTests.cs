using Microsoft.Azure.Management.ResourceManager;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ArmEval.Core.Tests
{
    [TestCaseOrderer("ArmEval.Core.Tests.NumericOrderer", "ArmEval.Core.Tests")]
    public class ResourceGroupsHelperTests : IClassFixture<TestConfig>
    {
        private readonly string resourceGroupName = "ArmEvalTests-rg";
        private readonly string location = "North Europe";
        private TestConfig testConfig;

        public ResourceGroupsHelperTests(TestConfig config)
        {
            testConfig = config;
        }


        [Fact, Order(1)]
        public void Exists_ResourceGroupDoesNotExist_ReturnsFalse()
        {
            using (var client = new ArmClient(testConfig.Real).Create())
            {
                var actual = ResourceGroupsHelper.Exists(client, resourceGroupName);

                Assert.False(actual);
            }
        }

        [Fact, Order(2)]
        public void CreateIfNotExists_ResourceGroupDoesNotExist_CreatesIt()
        {
            using (var client = new ArmClient(testConfig.Real).Create())
            {
                ResourceGroupsHelper.CreateIfNotExists(client, resourceGroupName, location);

                Assert.True(ResourceGroupsHelper.Exists(client, resourceGroupName));
            }
        }

        [Fact, Order(3)]
        public void CreateIfNotExists_ResourceGroupExists_DoesNotThrow()
        {
            using (var client = new ArmClient(testConfig.Real).Create())
            {
                ResourceGroupsHelper.CreateIfNotExists(client, resourceGroupName, location);
                Action act = () => { ResourceGroupsHelper.CreateIfNotExists(client, resourceGroupName, location); };
                var ex = Record.Exception(act);

                Assert.Null(ex);
            }
        }

        [Fact, Order(4)]
        public void DeleteIfExists()
        {
            using (var client = new ArmClient(testConfig.Real).Create())
            {
                var exists = ResourceGroupsHelper.Exists(client, resourceGroupName);
                ResourceGroupsHelper.DeleteIfExists(client, resourceGroupName);
                Assert.True(exists);
            }
        }

        [Fact, Order(5)]
        public void Exists_ResourceGroupDeleted_ReturnsFalse()
        {
            using (var client = new ArmClient(testConfig.Real).Create())
            {
                var actual = ResourceGroupsHelper.Exists(client, resourceGroupName);
                Assert.False(actual);
            }
        }
    }
}

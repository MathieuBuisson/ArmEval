using Microsoft.Azure.Management.ResourceManager;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ArmEval.Core.Tests
{
    public class ResourceGroupsHelperTests
    {
        private readonly string resourceGroupName = "ArmEvalTests-rg";
        private readonly string location = "North Europe";

        [Fact]
        public void CreateIfNotExists_ResourceGroupDoesNotExist_CreatesIt()
        {
            using (var client = new ArmClient().Create())
            {
                ResourceGroupsHelper.CreateIfNotExists(client, resourceGroupName, location);

                Assert.True(client.ResourceGroups.CheckExistence(resourceGroupName));
            }
        }

        [Fact]
        public void CreateIfNotExists_ResourceGroupExists_DoesNotThrow()
        {
            using (var client = new ArmClient().Create())
            {
                ResourceGroupsHelper.CreateIfNotExists(client, resourceGroupName, location);
                Action act = () => { ResourceGroupsHelper.CreateIfNotExists(client, resourceGroupName, location); };
                var ex = Record.Exception(act);

                Assert.Null(ex);
            }
        }

        [Fact]
        public void DeleteIfExists()
        {
            using (var client = new ArmClient().Create())
            {
                var existenceBefore = client.ResourceGroups.CheckExistence(resourceGroupName);
                ResourceGroupsHelper.DeleteIfExists(client, resourceGroupName);
                var existenceAfter = client.ResourceGroups.CheckExistence(resourceGroupName);

                Assert.True(existenceBefore);
                Assert.False(existenceAfter);
            }
        }
    }
}

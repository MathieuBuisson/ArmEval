using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ArmEval.Core.Tests
{
    [TestCaseOrderer("ArmEval.Core.Tests.NumericOrderer", "ArmEval.Core.Tests")]
    public class ArmDeploymentTests : IClassFixture<ArmClientConfig>
    {
        private readonly string resourceGroupName = "ArmEvalDeploy";
        private readonly string location = "North Europe";
        private readonly ArmClientConfig testConfig;
        private readonly ArmTemplate emptyTemplate = new ArmTemplate();

        public ArmDeploymentTests(ArmClientConfig config)
        {
            testConfig = config;
        }


        [Fact, Order(1)]
        public void Constructor_SetsAllProperties()
        {
            using (var client = new ArmClient(testConfig).Create())
            {
                var actual = new ArmDeployment(client, resourceGroupName, emptyTemplate);

                Assert.Same(client, actual.ResourceManagementClient);
                Assert.Same(resourceGroupName, actual.ResourceGroupName);
                Assert.Same(emptyTemplate, actual.Template);
                Assert.StartsWith("armeval-deployment-", actual.DeploymentName);
                Assert.IsType<Deployment>(actual.Deployment);
            }
        }

        [Fact, Order(2)]
        public void Invoke_EmptyTemplate_SucceedsWithEmptyOutputs()
        {
            using (var client = new ArmClient(testConfig).Create())
            {
                var deploy = new ArmDeployment(client, resourceGroupName, emptyTemplate);
                ResourceGroupsHelper.CreateIfNotExists(client, resourceGroupName, location);
                var actual = deploy.Invoke();

                Assert.Equal("Succeeded", actual.Properties.ProvisioningState);
                Assert.Equal("{}", actual.Properties.Outputs.ToString());
            }
        }
    }
}

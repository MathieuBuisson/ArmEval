using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ArmEval.Core.Tests
{
    [TestCaseOrderer("ArmEval.Core.Tests.NumericOrderer", "ArmEval.Core.Tests")]
    public class ArmDeploymentTests : IClassFixture<ArmClientConfigDeploymentTests>
    {
        private readonly ArmClientConfigDeploymentTests testConfig;
        private readonly ArmTemplate emptyTemplate = new ArmTemplate();

        public ArmDeploymentTests(ArmClientConfigDeploymentTests config)
        {
            testConfig = config;
        }

        [Fact, Order(1)]
        public void Constructor_SetsAllProperties()
        {
            var actual = new ArmDeployment(testConfig.Client, testConfig.ResourceGroup, emptyTemplate);

            Assert.Same(testConfig.Client, actual.ResourceManagementClient);
            Assert.Same(testConfig.ResourceGroup, actual.ResourceGroupName);
            Assert.Same(emptyTemplate, actual.Template);
            Assert.Matches(@"^armeval-deployment-\d+$", actual.DeploymentName);
            Assert.IsType<Deployment>(actual.Deployment);
        }

        [Fact, Order(2)]
        public void Invoke_EmptyTemplate_SucceedsWithEmptyOutputs()
        {
            var deploy = new ArmDeployment(testConfig.Client, testConfig.ResourceGroup, emptyTemplate);
            ResourceGroupsHelper.CreateIfNotExists(testConfig.Client, testConfig.ResourceGroup, testConfig.Location);
            var actual = deploy.Invoke();

            Assert.Equal("Succeeded", actual.Properties.ProvisioningState);
            Assert.Equal("{}", actual.Properties.Outputs.ToString());
        }
    }
}

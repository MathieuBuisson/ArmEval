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
        private readonly ArmClientConfigDeploymentTests config;
        private readonly ArmTemplate emptyTemplate = new ArmTemplate();

        public ArmDeploymentTests(ArmClientConfigDeploymentTests conf)
        {
            config = conf;
        }

        [Fact, Order(1)]
        public void Constructor_SetsAllProperties()
        {
            var actual = new ArmDeployment(config.Client, config.ResourceGroup, emptyTemplate, config.Location);

            Assert.Same(config.Client, actual.ResourceManagementClient);
            Assert.Same(config.ResourceGroup, actual.ResourceGroupName);
            Assert.Same(emptyTemplate, actual.Template);
            Assert.Same(config.Location, actual.Location);
            Assert.Matches(@"^armeval-deployment-\d+$", actual.DeploymentName);
            Assert.IsType<Deployment>(actual.Deployment);
        }

        [Fact, Order(2)]
        public void Invoke_EmptyTemplate_SucceedsWithEmptyOutputs()
        {
            var deployment = new ArmDeployment(config.Client, config.ResourceGroup, emptyTemplate, config.Location);
            var actual = deployment.Invoke();

            Assert.Equal("Succeeded", actual.Properties.ProvisioningState);
            Assert.Equal("{}", actual.Properties.Outputs.ToString());
        }
    }
}

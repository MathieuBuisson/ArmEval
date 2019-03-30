using ArmEval.Core.ArmClient;
using ArmEval.Core.ArmTemplate;
using ArmEval.Core.Extensions;
using ArmEval.Core.Utils;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace ArmEval.Core.Tests.ArmClient
{
    public class ArmDeploymentTests
    {
        private readonly IResourceManagementClient client;
        private readonly string location;
        private readonly string rgName;
        private readonly JObject emptyTemplate;
        private readonly ResourceGroup resourceGroup;

        public ArmDeploymentTests()
        {
            emptyTemplate = new TemplateBuilder().Template;
            location = "North Europe";
            rgName = $"ArmEvalDeploy-{UniqueString.Create(5)}";
            resourceGroup = new ResourceGroup(location, name: rgName);

            client = new Mock<IResourceManagementClient>().Object;
        }

        [Fact]
        public void Constructor_SetsAllProperties()
        {
            var actual = new ArmDeployment(client, resourceGroup);

            Assert.Same(client, actual.Client);
            Assert.Same(rgName, actual.ResourceGroup.Name);
            Assert.Same(location, actual.ResourceGroup.Location);
            Assert.Matches(@"^armeval-deployment-\w{5}$", actual.DeploymentName);
            Assert.IsType<Deployment>(actual.Deployment);
            Assert.Equal(emptyTemplate, actual.Deployment.Properties.Template);
            Assert.Equal(DeploymentMode.Incremental, actual.Deployment.Properties.Mode);
        }
    }
}

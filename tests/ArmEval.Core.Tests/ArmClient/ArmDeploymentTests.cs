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
        private readonly JObject emptyTemplate;

        public ArmDeploymentTests()
        {
            emptyTemplate = new TemplateBuilder().Template;
            location = "North Europe";

            client = new Mock<IResourceManagementClient>().Object;
        }

        [Fact]
        public void Constructor_SetsAllProperties()
        {
            var actual = new ArmDeployment(client, location);

            Assert.Same(client, actual.Client);
            Assert.Matches(@"^armeval-deployment-\w{5}$", actual.DeploymentName);
            Assert.IsType<Deployment>(actual.Deployment);
            Assert.Same(location, actual.Deployment.Location);
            Assert.Equal(emptyTemplate, actual.Deployment.Properties.Template);
            Assert.Equal(DeploymentMode.Incremental, actual.Deployment.Properties.Mode);
        }
    }
}

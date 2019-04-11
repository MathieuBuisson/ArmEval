using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using System;
using ArmEval.Core.ArmTemplate;
using ArmEval.Core.Utils;

namespace ArmEval.Core.ArmClient
{
    public class ArmDeployment : IArmDeployment
    {
        public IResourceManagementClient Client { get; set; }
        public string DeploymentName { get; }
        public Deployment Deployment { get; set; }
        public string Location { get; set; }

        public ArmDeployment(IResourceManagementClient resourceManagementClient, string location)
        {
            Client = resourceManagementClient;
            Location = location;
            var template = new TemplateBuilder().Template;

            var suffix = UniqueString.Create();
            DeploymentName = $"armeval-deployment-{suffix}";

            var deploymentProperties = new DeploymentProperties
            {
                Mode = DeploymentMode.Incremental,
                Template = template,
                Parameters = template["parameters"]
            };
            Deployment = new Deployment(deploymentProperties, Location);
        }

        public string Invoke()
        {
            var result = Client
                .Deployments
                .CreateOrUpdateAtSubscriptionScope(DeploymentName, Deployment);

            var outputsString = result.Properties.Outputs.ToString();
            return outputsString;
        }
    }
}

using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using System;
using ArmEval.Core.ArmTemplate;
using ArmEval.Core.Extensions;
using ArmEval.Core.Utils;

namespace ArmEval.Core.ArmClient
{
    public class ArmDeployment : IArmDeployment
    {
        public IResourceManagementClient Client { get; set; }
        public ResourceGroup ResourceGroup { get; set; }
        public string DeploymentName { get; }
        public Deployment Deployment { get; set; }

        public ArmDeployment(IResourceManagementClient resourceManagementClient,
            ResourceGroup resourceGroup)
        {
            Client = resourceManagementClient;
            ResourceGroup = resourceGroup;
            var template = new TemplateBuilder().Template;

            var suffix = UniqueString.Create(5);
            DeploymentName = $"armeval-deployment-{suffix}";

            Deployment = new Deployment();
            Deployment.Properties = new DeploymentProperties
            {
                Mode = DeploymentMode.Incremental,
                Template = template,
                Parameters = template["parameters"]
            };
        }

        public string Invoke()
        {
            ResourceGroup.CreateIfNotExists(Client, ResourceGroup.Location);

            var result = Client
                .Deployments
                .CreateOrUpdate(ResourceGroup.Name, DeploymentName, Deployment);

            var outputsString = result.Properties.Outputs.ToString();
            return outputsString;
        }
    }
}

using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using System;
using System.Collections.Generic;
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
        public ITemplate Template { get; set; }
        public Deployment Deployment { get; set; }

        public ArmDeployment(IResourceManagementClient resourceManagementClient,
            ResourceGroup resourceGroup, ITemplate template)
        {
            Client = resourceManagementClient;
            ResourceGroup = resourceGroup;
            Template = template;

            var suffix = UniqueString.Create(5);
            DeploymentName = $"armeval-deployment-{suffix}";

            Deployment = new Deployment();
            Deployment.Properties = new DeploymentProperties
            {
                Mode = DeploymentMode.Incremental,
                Template = Template,
                Parameters = Template.Parameters
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

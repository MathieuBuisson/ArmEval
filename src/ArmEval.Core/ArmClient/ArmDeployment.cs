using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using System;
using System.Collections.Generic;
using ArmEval.Core.ArmTemplate;
using ArmEval.Core.Extensions;
using ArmEval.Core.Utils;
using Newtonsoft.Json.Linq;

namespace ArmEval.Core.ArmClient
{
    public class ArmDeployment : IArmDeployment
    {
        public IResourceManagementClient Client { get; set; }
        public ResourceGroup ResourceGroup { get; set; }
        public string DeploymentName { get; }
        public JObject Template { get; set; }
        public Deployment Deployment { get; set; }

        public ArmDeployment(IResourceManagementClient resourceManagementClient,
            ResourceGroup resourceGroup)
        {
            Client = resourceManagementClient;
            ResourceGroup = resourceGroup;
            var jsonString = @"{
  '$schema': 'https://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#',
  'contentVersion': '1.0.0.0',
  'parameters': {},
  'variables': {},
  'resources': [],
  'outputs': {}
}";
            Template = JObject.Parse(jsonString);

            var suffix = UniqueString.Create(5);
            DeploymentName = $"armeval-deployment-{suffix}";

            Deployment = new Deployment();
            Deployment.Properties = new DeploymentProperties
            {
                Mode = DeploymentMode.Incremental,
                Template = Template,
                Parameters = Template["parameters"]
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

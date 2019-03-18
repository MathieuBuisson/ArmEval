using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArmEval.Core
{
    public class ArmDeployment
    {
        public IResourceManagementClient ResourceManagementClient { get; set; }
        public string ResourceGroupName { get; set; }
        public string DeploymentName { get; }
        public ArmTemplate Template { get; set; }
        public Deployment Deployment { get; set; }
        public string Location { get; set; }


        public ArmDeployment(IResourceManagementClient resourceManagementClient, string resourceGroupName,
            ArmTemplate template, string location)
        {
            ResourceManagementClient = resourceManagementClient;
            ResourceGroupName = resourceGroupName;
            Template = template;
            Location = location;

            var timeStamp = DateTime.Now.Ticks.ToString().Substring(13);
            DeploymentName = $"armeval-deployment-{timeStamp}";

            Deployment = new Deployment();
            Deployment.Properties = new DeploymentProperties
            {
                Mode = DeploymentMode.Incremental,
                Template = Template,
                Parameters = Template.Parameters
            };

        }

        public DeploymentExtended Invoke()
        {
            ResourceGroupsHelper.CreateIfNotExists(ResourceManagementClient, ResourceGroupName, Location);

            var result = ResourceManagementClient
                .Deployments
                .CreateOrUpdate(ResourceGroupName, DeploymentName, Deployment);
            return result;
        }
    }

}

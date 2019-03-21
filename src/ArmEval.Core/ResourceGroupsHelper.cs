using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArmEval.Core
{
    public static class ResourceGroupsHelper
    {
        public static bool Exists(IResourceManagementClient client, string resourceGroupName)
        {
            return client.ResourceGroups.CheckExistence(resourceGroupName);
        }

        public static void Create(IResourceManagementClient client, string resourceGroupName, string location)
        {
            var resourceGroup = new ResourceGroup();
            resourceGroup.Location = location;
            client.ResourceGroups.CreateOrUpdate(resourceGroupName, resourceGroup);
        }

        public static void CreateIfNotExists(IResourceManagementClient client, string resourceGroupName, string location)
        {
            if (!Exists(client, resourceGroupName))
            {
                Create(client, resourceGroupName, location);
            }
        }

        public static void DeleteIfExists(IResourceManagementClient client, string resourceGroupName)
        {
            if (Exists(client, resourceGroupName))
            {
                client.ResourceGroups.Delete(resourceGroupName);
            }
        }
    }
}

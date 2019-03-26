using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArmEval.Core.Extensions
{
    public static class ResourceGroupExtensions
    {
        public static bool Exists(this ResourceGroup rg, IResourceManagementClient client)
        {
            return client.ResourceGroups.CheckExistence(rg.Name);
        }

        public static void Create(this ResourceGroup rg, IResourceManagementClient client, string location)
        {
            rg.Location = location;
            client.ResourceGroups.CreateOrUpdate(rg.Name, rg);
        }

        public static void CreateIfNotExists(this ResourceGroup rg, IResourceManagementClient client, string location)
        {
            if (!rg.Exists(client))
            {
                rg.Create(client, location);
            }
        }

        public static void DeleteIfExists(this ResourceGroup rg, IResourceManagementClient client)
        {
            if (rg.Exists(client))
            {
                client.ResourceGroups.Delete(rg.Name);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Extensions.Configuration;

namespace ArmEval.Core.AzureClient
{
    public class ArmClientConfig : IArmClientConfig, IDisposable
    {
        public string TenantId { get; }
        public string ClientId { get; }
        public string ClientSecret { get; }
        public string Subscription { get; }
        public string ResourceGroup { get; set; }
        public string Location => "North Europe";
        public IResourceManagementClient Client { get; }

        public ArmClientConfig()
        {
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables("ArmEval_")
                .Build();
            TenantId = config["TenantId"];
            ClientId = config["ClientId"];
            ClientSecret = config["ClientSecret"];
            Subscription = config["SubscriptionId"];
            ResourceGroup = $"ArmEval{DateTime.Now.Ticks.ToString().Substring(13)}";

            Client = new ArmClient(TenantId, ClientId, ClientSecret, Subscription)
                .Create();
        }

        public void Dispose()
        {
            ResourceGroupsHelper.DeleteIfExists(Client, ResourceGroup);
        }
    }
}

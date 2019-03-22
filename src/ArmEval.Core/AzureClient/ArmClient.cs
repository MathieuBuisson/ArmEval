using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Rest.Azure.Authentication;

namespace ArmEval.Core.AzureClient
{
    public class ArmClient
    {
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string SubscriptionId { get; set; }

        public IResourceManagementClient Create()
        {
            var creds = ApplicationTokenProvider.LoginSilentAsync(TenantId, ClientId, ClientSecret).Result;
            var resourceManagementClient = new ResourceManagementClient(creds);
            resourceManagementClient.SubscriptionId = SubscriptionId;
            return resourceManagementClient;
        }

        public ArmClient(string tenantId, string clientId, string clientSecret, string subscriptionId)
        {
            TenantId = tenantId;
            ClientId = clientId;
            ClientSecret = clientSecret;
            SubscriptionId = subscriptionId;
        }

        public ArmClient(IArmClientConfig config)
        {
            TenantId = config.TenantId;
            ClientId = config.ClientId;
            ClientSecret = config.ClientSecret;
            SubscriptionId = config.Subscription;
        }
    }
}

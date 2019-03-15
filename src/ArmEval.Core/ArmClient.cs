using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Rest.Azure.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace ArmEval.Core
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

        public ArmClient(IConfiguration config)
        {
            TenantId = config["TenantId"];
            ClientId = config["ClientId"];
            ClientSecret = config["ClientSecret"];
            SubscriptionId = config["SubscriptionId"];
        }
    }
}

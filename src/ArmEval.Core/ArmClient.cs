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
        public IConfiguration Config { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string SubscriptionId { get; set; }

        public ResourceManagementClient Create()
        {
            var creds = ApplicationTokenProvider.LoginSilentAsync(TenantId, ClientId, ClientSecret).Result;
            var resourceManagementClient = new ResourceManagementClient(creds);
            resourceManagementClient.SubscriptionId = SubscriptionId;
            return resourceManagementClient;
        }

        public ArmClient(string tenantId = null, string clientId = null, string clientSecret = null, string subscriptionId = null)
        {
            Config = new ConfigurationBuilder()
                .AddUserSecrets("72146dd3-c4ba-47e2-81a5-94b367d9c109")
                .Build();
            TenantId = tenantId ?? Config["TenantId"];
            ClientId = clientId ?? Config["ClientId"];
            ClientSecret = clientSecret ?? Config["ClientSecret"];
            SubscriptionId = subscriptionId ?? Config["SubscriptionId"];
        }
    }
}

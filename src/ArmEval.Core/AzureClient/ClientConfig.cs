using System;
using System.Collections.Generic;
using System.Text;
using ArmEval.Core.Utils;
using Microsoft.Extensions.Configuration;

namespace ArmEval.Core.AzureClient
{
    public class ClientConfig : IClientConfig
    {
        public string TenantId { get; }
        public string ClientId { get; }
        public string ClientSecret { get; }
        public string Subscription { get; }

        public ClientConfig(string tenantId, string clientId, string clientSecret, string subscription)
        {
            TenantId = tenantId;
            ClientId = clientId;
            ClientSecret = clientSecret;
            Subscription = subscription;
        }

        // Parameterless constructor for mocking
        public ClientConfig()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ArmEval.Core
{
    public class ArmClientConfig : IArmClientConfig
    {
        public string TenantId { get; }
        public string ClientId { get; }
        public string ClientSecret { get; }
        public string Subscription { get; }

        public ArmClientConfig()
        {
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables("ArmEval_")
                .Build();
            TenantId = config["TenantId"];
            ClientId = config["ClientId"];
            ClientSecret = config["ClientSecret"];
            Subscription = config["SubscriptionId"];
        }
    }
}

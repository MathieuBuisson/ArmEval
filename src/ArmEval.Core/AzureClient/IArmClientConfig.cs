using System;
using System.Collections.Generic;
using System.Text;

namespace ArmEval.Core.AzureClient
{
    public interface IArmClientConfig
    {
        string TenantId { get; }
        string ClientId { get; }
        string ClientSecret { get; }
        string Subscription { get; }
    }
}

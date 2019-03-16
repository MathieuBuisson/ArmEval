using System;
using System.Collections.Generic;
using System.Text;

namespace ArmEval.Core.Tests
{
    public class ArmClientTestConfig : IArmClientConfig
    {
        public string TenantId { get => "testTenantId"; }
        public string ClientId { get => "testClientId"; }
        public string ClientSecret { get => "testSecret"; }
        public string Subscription { get => "testSubscription"; }
    }
}

using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArmEval.Core.Tests
{
    public class TestConfig
    {
        public string TenantId = "testTenantId";
        public string ClientId = "testClientId";
        public string Secret = "testSecret";
        public string Subscription = "testSubscription";
        public IConfiguration Config { get; }
        public IConfiguration Real { get; }

        public TestConfig()
        {
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.SetupGet(c => c["TenantId"]).Returns(TenantId);
            mockConfig.SetupGet(c => c["ClientId"]).Returns(ClientId);
            mockConfig.SetupGet(c => c["ClientSecret"]).Returns(Secret);
            mockConfig.SetupGet(c => c["SubscriptionId"]).Returns(Subscription);
            Config = mockConfig.Object as IConfiguration;

            var realConfig = new Mock<IConfiguration>();
            realConfig.SetupGet(c => c["TenantId"]).Returns("14f3b174-9cdb-4a5e-9177-18c3bccc87cb");
            realConfig.SetupGet(c => c["ClientId"]).Returns("75c1ecd3-190f-42c9-8660-088f69d121ba");
            realConfig.SetupGet(c => c["ClientSecret"]).Returns("feWpRr6/YCxNyh8efMvjWbe5JoOiOw03xR1o9S5CLhY=");
            realConfig.SetupGet(c => c["SubscriptionId"]).Returns("d267cdac-1b9b-4ee2-b6c8-7b6eee4e4d89");
            Real = realConfig.Object as IConfiguration;
        }
    }
}

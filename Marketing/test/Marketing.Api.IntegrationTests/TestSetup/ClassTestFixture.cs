using System;
using System.Net.Http;
using Marketing.Persistence.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Marketing.Api.IntegrationTests.TestSetup
{
    public class ClassTestFixture : IDisposable
    {
        private readonly TestServer _server;

        public ClassTestFixture()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<TestStartup>()) {BaseAddress = new Uri("https://localhost")};

            Context = _server.Host.Services.GetService(typeof(MarketingDbContext)) as MarketingDbContext;
            Client = _server.CreateClient();
        }

        public HttpClient Client { get; }

        public MarketingDbContext Context { get; }

        public void Dispose()
        {
            Context.Dispose();
            Client.Dispose();
            _server.Dispose();
        }
    }
}
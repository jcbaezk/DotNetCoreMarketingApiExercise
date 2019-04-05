using System;
using Marketing.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Marketing.Persistence.IntegrationTests.TestSetup
{
    public class ClassTestFixture : IDisposable
    {
        public MarketingDbContext Context => MarketingDbContext();

        private static MarketingDbContext MarketingDbContext()
        {
            var options = new DbContextOptionsBuilder<MarketingDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;
            var context = new MarketingDbContext(options);

            return context;
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}
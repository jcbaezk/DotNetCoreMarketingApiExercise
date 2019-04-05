using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Marketing.Api.IntegrationTests.TestSetup
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override DbContextOptionsBuilder GetDbContextOptions(DbContextOptionsBuilder options)
        {
            return options.UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging();
        }
    }
}
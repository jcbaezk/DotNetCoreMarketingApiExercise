using Marketing.Persistence.Entities;
using Marketing.Persistence.Entities.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Marketing.Persistence.DbContexts
{
    public class MarketingDbContext : DbContext
    {
        public MarketingDbContext(DbContextOptions<MarketingDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Advertisement> Advertisements { get; set; }

        public DbSet<AdvertisementChannel> AdvertisementChannels { get; set; }

        public DbSet<Channel> Channels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AdvertisementConfiguration());
            modelBuilder.ApplyConfiguration(new ChannelConfiguration());
            modelBuilder.ApplyConfiguration(new AdvertisementChannelConfiguration());
        }
    }
}
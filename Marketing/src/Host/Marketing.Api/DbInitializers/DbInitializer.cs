using System.Collections.Generic;
using System.Linq;
using Marketing.Persistence.DbContexts;
using Marketing.Persistence.Entities;

namespace Marketing.Api.DbInitializers
{
    public static class DbInitializer
    {
        public static void Initialize(MarketingDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Advertisements.Any())
            {
                return;
            }

            var advertisement = new Advertisement
            {
                Name = "New product",
                ClientId = 123
            };
            context.Advertisements.Add(advertisement);
            context.SaveChanges();

            var digitalChannel = new Channel
            {
                Name = "Facebook",
                IsDigital = true
            };
            
            var physicalChannel = new Channel
            {
                Name = "Magazine",
                IsDigital = false
            };
            context.Channels.AddRange( new List<Channel>{ digitalChannel, physicalChannel });
            context.SaveChanges();

            var advertisementChannels = new List<AdvertisementChannel>
            {
                new AdvertisementChannel{ AdvertisementId = advertisement.Id, ChannelId = digitalChannel.Id },
                new AdvertisementChannel{ AdvertisementId = advertisement.Id, ChannelId = physicalChannel.Id }
            };
            context.AdvertisementChannels.AddRange(advertisementChannels);
            context.SaveChanges();
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Marketing.Domain.Domains;

namespace Marketing.Persistence.Mappers
{
    public class AdvertisementMapper : IAdvertisementMapper
    {
        public Advertisement ToDomain(Entities.Advertisement advertisement)
        {
            return new Advertisement
            {
                Id = advertisement.Id,
                Name = advertisement.Name,
                ClientId = advertisement.ClientId,
                Channels = MapChannelsToDomain(advertisement)
            };
        }

        public IEnumerable<Advertisement> ToDomains(IEnumerable<Entities.Advertisement> advertisements)
        {
            return advertisements.Select(ToDomain);
        }

        public Entities.Advertisement ToEntity(AdvertisementEntry advertisementEntry)
        {
            return new Entities.Advertisement
            {
                Id = advertisementEntry.Id,
                Name = advertisementEntry.Name,
                ClientId = advertisementEntry.ClientId,
                AdvertisementChannels = MapAdvertisementChannelsToEntity(advertisementEntry)
            };
        }

        private ICollection<Entities.AdvertisementChannel> MapAdvertisementChannelsToEntity(AdvertisementEntry advertisementEntry)
        {
            var advertisementId = advertisementEntry.Id;
            return advertisementEntry.ChannelIds.Select(x => new Entities.AdvertisementChannel { AdvertisementId = advertisementId, ChannelId = x}).ToList();
        }

        private static IEnumerable<Channel> MapChannelsToDomain(Entities.Advertisement advertisement)
        {
            return advertisement.AdvertisementChannels.Select(x => 
                new Channel { Id = x.Channel.Id, Name = x.Channel.Name, IsDigital = x.Channel.IsDigital });
        }
    }
}
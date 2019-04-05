using System.Collections.Generic;

namespace Marketing.Persistence.Entities
{
    public class Advertisement
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ClientId { get; set; }

        public ICollection<AdvertisementChannel> AdvertisementChannels { get; set; }
    }
}
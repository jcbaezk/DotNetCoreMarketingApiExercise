using System.Collections.Generic;

namespace Marketing.Domain.Domains
{
    public class AdvertisementEntry
    {
        public AdvertisementEntry()
        {
            ChannelIds = new List<int>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int ClientId { get; set; }

        public IEnumerable<int> ChannelIds { get; set; }
    }
}
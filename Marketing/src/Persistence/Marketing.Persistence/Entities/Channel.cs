using System.Collections.Generic;

namespace Marketing.Persistence.Entities
{
    public class Channel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsDigital { get; set; }

        public ICollection<AdvertisementChannel> AdvertisementChannels { get; set; }
    }
}
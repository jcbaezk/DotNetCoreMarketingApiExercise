using System.Collections.Generic;

namespace Marketing.Domain.Domains
{
    public class Advertisement
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ClientId { get; set; }

        public IEnumerable<Channel> Channels { get; set; }
    }
}
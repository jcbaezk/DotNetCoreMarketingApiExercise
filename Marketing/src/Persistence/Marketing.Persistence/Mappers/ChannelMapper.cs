using System.Collections.Generic;
using System.Linq;
using Marketing.Domain.Domains;

namespace Marketing.Persistence.Mappers
{
    public class ChannelMapper : IChannelMapper
    {
        public Channel ToDomain(Entities.Channel channel)
        {
            return new Channel
            {
                Id = channel.Id,
                Name = channel.Name,
                IsDigital = channel.IsDigital
            };
        }

        public IEnumerable<Channel> ToDomains(IEnumerable<Entities.Channel> channels)
        {
            return channels.Select(ToDomain);
        }

        public Entities.Channel ToEntity(Channel channel)
        {
            return new Entities.Channel
            {
                Id = channel.Id,
                Name = channel.Name,
                IsDigital = channel.IsDigital
            };
        }
    }
}
using System.Collections.Generic;
using Marketing.Domain.Domains;

namespace Marketing.Persistence.Mappers
{
    public interface IChannelMapper
    {
        Channel ToDomain(Entities.Channel channel);

        IEnumerable<Channel> ToDomains(IEnumerable<Entities.Channel> channels);

        Entities.Channel ToEntity(Channel channel);
    }
}
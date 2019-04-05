using System.Collections.Generic;
using System.Threading.Tasks;
using Marketing.Domain.Domains;

namespace Marketing.Domain.Repositories
{
    public interface IChannelRepository
    {
        Task<IEnumerable<Channel>> GetAllAsync();

        Task<Channel> GetByIdAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task<Channel> CreateAsync(Channel advertisementEntry);

        Task UpdateAsync(Channel advertisementEntry);

        Task<bool> HasAdvertisementsAsync(int id);

        Task DeleteAsync(int id);

        Task<bool> ChannelsExistAsync(IEnumerable<int> channelIds);
    }
}
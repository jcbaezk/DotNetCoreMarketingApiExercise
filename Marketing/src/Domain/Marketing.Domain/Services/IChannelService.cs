using System.Collections.Generic;
using System.Threading.Tasks;
using Marketing.Domain.Domains;

namespace Marketing.Domain.Services
{
    public interface IChannelService
    {
        Task<IEnumerable<Channel>> GetAllAsync();

        Task<Channel> GetByIdAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task<Channel> CreateAsync(Channel channelEntry);

        Task UpdateAsync(Channel channelEntry);

        Task<bool> HasAdvertisementsAsync(int id);

        Task DeleteAsync(int id);

        Task<bool> ChannelsExistAsync(IEnumerable<int> channelIds);
    }
}
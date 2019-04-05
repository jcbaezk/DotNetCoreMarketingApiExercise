using System.Collections.Generic;
using System.Threading.Tasks;
using Marketing.Domain.Domains;
using Marketing.Domain.Repositories;

namespace Marketing.Domain.Services
{
    public class ChannelService : IChannelService
    {
        private readonly IChannelRepository _channelRepository;

        public ChannelService(IChannelRepository channelRepository)
        {
            _channelRepository = channelRepository;
        }

        public async Task<IEnumerable<Channel>> GetAllAsync()
        {
            return await _channelRepository.GetAllAsync();
        }

        public async Task<Channel> GetByIdAsync(int id)
        {
            return await _channelRepository.GetByIdAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _channelRepository.ExistsAsync(id);
        }
        
        public async Task<Channel> CreateAsync(Channel advertisementEntry)
        {
            return await _channelRepository.CreateAsync(advertisementEntry);
        }

        public async Task UpdateAsync(Channel advertisementEntry)
        {
            await _channelRepository.UpdateAsync(advertisementEntry);
        }

        public async Task<bool> HasAdvertisementsAsync(int id)
        {
            return await _channelRepository.HasAdvertisementsAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _channelRepository.DeleteAsync(id);
        }

        public async Task<bool> ChannelsExistAsync(IEnumerable<int> channelIds)
        {
            return await _channelRepository.ChannelsExistAsync(channelIds);
        }
    }
}
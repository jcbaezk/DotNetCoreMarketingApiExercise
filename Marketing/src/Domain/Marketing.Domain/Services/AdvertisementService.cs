using System.Collections.Generic;
using System.Threading.Tasks;
using Marketing.Domain.Domains;
using Marketing.Domain.Repositories;

namespace Marketing.Domain.Services
{
    public class AdvertisementService : IAdvertisementService
    {
        private readonly IAdvertisementRepository _advertisementRepository;

        public AdvertisementService(IAdvertisementRepository advertisementRepository)
        {
            _advertisementRepository = advertisementRepository;
        }

        public async Task<IEnumerable<Advertisement>> GetAllAsync()
        {
            return await _advertisementRepository.GetAllAsync();
        }

        public async Task<Advertisement> GetByIdAsync(int id)
        {
            return await _advertisementRepository.GetByIdAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _advertisementRepository.ExistsAsync(id);
        }

        public async Task<IEnumerable<Advertisement>> GetByClientIdAsync(int clientId)
        {
            return await _advertisementRepository.GetByClientIdAsync(clientId);
        }

        public async Task<Advertisement> CreateAsync(AdvertisementEntry advertisementEntry)
        {
            return await _advertisementRepository.CreateAsync(advertisementEntry);
        }

        public async Task UpdateAsync(AdvertisementEntry advertisementEntry)
        {
            await _advertisementRepository.UpdateAsync(advertisementEntry);
        }

        public async Task<bool> HasChannelsAsync(int id)
        {
            return await _advertisementRepository.HasChannelsAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _advertisementRepository.DeleteAsync(id);
        }
    }
}
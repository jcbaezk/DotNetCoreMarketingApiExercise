using System.Collections.Generic;
using System.Threading.Tasks;
using Marketing.Domain.Domains;

namespace Marketing.Domain.Services
{
    public interface IAdvertisementService
    {
        Task<IEnumerable<Advertisement>> GetAllAsync();

        Task<Advertisement> GetByIdAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task<IEnumerable<Advertisement>> GetByClientIdAsync(int clientId);

        Task<Advertisement> CreateAsync(AdvertisementEntry advertisementEntry);

        Task UpdateAsync(AdvertisementEntry advertisementEntry);

        Task<bool> HasChannelsAsync(int id);

        Task DeleteAsync(int id);
    }
}
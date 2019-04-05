using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marketing.Domain.Domains;
using Marketing.Domain.Repositories;
using Marketing.Persistence.DbContexts;
using Marketing.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Marketing.Persistence.Repositories
{
    public class AdvertisementRepository : IAdvertisementRepository
    {
        private readonly MarketingDbContext _context;
        private readonly IAdvertisementMapper _mapper;

        public AdvertisementRepository(MarketingDbContext context, IAdvertisementMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Advertisement>> GetAllAsync()
        {
            var entities = await _context.Advertisements
                .Include(x => x.AdvertisementChannels)
                    .ThenInclude(x => x.Channel)
                .ToListAsync();

            return _mapper.ToDomains(entities);
        }

        public async Task<Advertisement> GetByIdAsync(int id)
        {
            var entity = await _context.Advertisements
                .Include(x => x.AdvertisementChannels)
                    .ThenInclude(x => x.Channel)
                .SingleOrDefaultAsync(x => x.Id == id);

            return entity == null ? null : _mapper.ToDomain(entity);
        }

        public async Task<IEnumerable<Advertisement>> GetByClientIdAsync(int clientId)
        {
            var entities = await _context.Advertisements
                .Include(x => x.AdvertisementChannels)
                    .ThenInclude(x => x.Channel)
                .Where(x => x.ClientId == clientId)
                .ToListAsync();

            return _mapper.ToDomains(entities);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Advertisements
                .AsNoTracking()
                .AnyAsync(x => x.Id == id);
        }

        public async Task<Advertisement> CreateAsync(AdvertisementEntry advertisementEntry)
        {
            var entity = _mapper.ToEntity(advertisementEntry);

            await _context.Advertisements.AddAsync(entity);
            await _context.SaveChangesAsync();

            return _mapper.ToDomain(entity);
        }

        public async Task UpdateAsync(AdvertisementEntry advertisementEntry)
        {
            var updateEntity = _mapper.ToEntity(advertisementEntry);

            var advertisement = await _context.Advertisements
                .Include(x => x.AdvertisementChannels)
                    .ThenInclude(x => x.Channel)
                .AsNoTracking()
                .SingleAsync(x => x.Id == updateEntity.Id);

            //var advertisementsToDelete = updateEntity.AdvertisementChannels.Where(x =>
            //    advertisement.AdvertisementChannels.All(c => c.ChannelId != x.ChannelId)).ToList();
            //_context.AdvertisementChannels.RemoveRange(advertisementsToDelete);

            //var advertisementsToAdd = advertisement.AdvertisementChannels.Where(x => 
            //    updateEntity.AdvertisementChannels.All(c => c.ChannelId != x.ChannelId)).ToList();
            //_context.AdvertisementChannels.AddRange(advertisementsToAdd);
            
            advertisement = updateEntity;
            
            _context.Advertisements.Update(advertisement);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasChannelsAsync(int id)
        {
            var advertisement =  await _context.Advertisements
                .Include(x => x.AdvertisementChannels)
                .AsNoTracking()
                .SingleAsync(x => x.Id == id);

            return advertisement.AdvertisementChannels.Any();
        }

        public async Task DeleteAsync(int id)
        {
            var advertisement = await _context.Advertisements
                .SingleAsync(x => x.Id == id);

            _context.Advertisements.Remove(advertisement);
            await _context.SaveChangesAsync();
        }
    }
}
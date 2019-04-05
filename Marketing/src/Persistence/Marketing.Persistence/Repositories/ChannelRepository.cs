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
    public class ChannelRepository : IChannelRepository
    {
        private readonly MarketingDbContext _context;
        private readonly IChannelMapper _mapper;

        public ChannelRepository(MarketingDbContext context, IChannelMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Channel>> GetAllAsync()
        {
            var entities = await _context.Channels
                .ToListAsync();

            return _mapper.ToDomains(entities);
        }

        public async Task<Channel> GetByIdAsync(int id)
        {
            var entity = await _context.Channels
                .SingleOrDefaultAsync(x => x.Id == id);

            return entity == null ? null : _mapper.ToDomain(entity);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Channels
                .AsNoTracking()
                .AnyAsync(x => x.Id == id);
        }

        public async Task<Channel> CreateAsync(Channel channel)
        {
            var entity = _mapper.ToEntity(channel);

            await _context.Channels.AddAsync(entity);
            await _context.SaveChangesAsync();

            return _mapper.ToDomain(entity);
        }

        public async Task UpdateAsync(Channel channel)
        {
            var updateEntity = _mapper.ToEntity(channel);

            var channelToUpdate = await _context.Channels
                .AsNoTracking()
                .SingleAsync(x => x.Id == updateEntity.Id);

            channelToUpdate = updateEntity;

            _context.Channels.Update(channelToUpdate);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasAdvertisementsAsync(int id)
        {
            var channel = await _context.Channels
                .Include(x => x.AdvertisementChannels)
                .AsNoTracking()
                .SingleAsync(x => x.Id == id);

            return channel.AdvertisementChannels.Any();
        }

        public async Task DeleteAsync(int id)
        {
            var channel = await _context.Channels
                .SingleAsync(x => x.Id == id);

            _context.Channels.Remove(channel);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ChannelsExistAsync(IEnumerable<int> channelIds)
        {
            var channelsExist = await _context.Channels.Select(x => x.Id).ToListAsync();
            return new HashSet<int>(channelsExist).IsSupersetOf(channelIds);
        }
    }
}
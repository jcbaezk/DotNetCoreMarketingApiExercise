using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Marketing.Persistence.DbContexts;
using Marketing.Persistence.Entities;
using Marketing.Persistence.IntegrationTests.TestSetup;
using Marketing.Persistence.Mappers;
using Marketing.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Advertisement = Marketing.Persistence.Entities.Advertisement;
using Channel = Marketing.Persistence.Entities.Channel;

namespace Marketing.Persistence.IntegrationTests.Repositories
{
    public class ChannelRepositoryTests : IClassFixture<ClassTestFixture>
    {
        private readonly ChannelRepository _channelRepository;
        private readonly Fixture _fixture;
        private readonly MarketingDbContext _dbContext;

        public ChannelRepositoryTests(ClassTestFixture testFixture)
        {
            _dbContext = testFixture.Context;
            _channelRepository = new ChannelRepository(_dbContext, new ChannelMapper());
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyListGivenNoItemsAreFound()
        {
            var result = await _channelRepository.GetAllAsync();

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfItemsFound()
        {
            var expectedChannel = SetupChannel();

            var result = await _channelRepository.GetAllAsync();

            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().ContainSingle(x => x.Id == expectedChannel.Id);
            var channel = result.Single(x => x.Id == expectedChannel.Id);
            DomainShouldBeEquivalent(channel, expectedChannel);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNullGivenTheIdIsNotFound()
        {
            var nonExistantId = _fixture.Create<int>();

            var result = await _channelRepository.GetByIdAsync(nonExistantId);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnItemGivenTheIdIsFound()
        {
            var expectedChannel = SetupChannel();

            var result = await _channelRepository.GetByIdAsync(expectedChannel.Id);

            result.Should().NotBeNull();
            DomainShouldBeEquivalent(result, expectedChannel);
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnFalseGivenANotMatchingId()
        {
            var nonExistantId = _fixture.Create<int>();

            var result = await _channelRepository.ExistsAsync(nonExistantId);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnTrueGivenAMatchingId()
        {
            var expectedChannel = SetupChannel();

            var result = await _channelRepository.ExistsAsync(expectedChannel.Id);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateTheItemEntry()
        {
            var channel = _fixture.Build<Domain.Domains.Channel>()
                .Create();

            var result = await _channelRepository.CreateAsync(channel);

            result.Should().NotBeNull();
            _dbContext.Channels.Any(x => x.Id == result.Id).Should().BeTrue();
        }

        [Fact(Skip = "Review tracking exception")]
        public async Task UpdateAsync_ShouldUpdateTheItemEntry()
        {
            var originalChannel = SetupChannel();
            var updatedChannel = new Domain.Domains.Channel
            {
                Id = originalChannel.Id,
                IsDigital = _fixture.Create<bool>(),
                Name = $"{originalChannel.Name} - Updated"
            };

            await _channelRepository.UpdateAsync(updatedChannel);

            var currentValue = _dbContext.Channels
                .AsNoTracking()
                .Single(x => x.Id == updatedChannel.Id);
            currentValue.Name.Should().Be(updatedChannel.Name);
            currentValue.IsDigital.Should().Be(updatedChannel.IsDigital);
        }

        [Fact]
        public async Task HasAdvertisementsAsync_ShouldReturnTrueGivenTheChannelHasAdvertisementsAssigned()
        {
            var expectedAdvertisement = SetupAdvertisement();
            var expectedChannel = SetupChannel();
            SetupAdvertisementChannel(expectedAdvertisement.Id, expectedChannel.Id);

            var result = await _channelRepository.HasAdvertisementsAsync(expectedChannel.Id);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task HasAdvertisementsAsync_ShouldReturnFalseGivenTheChannelHasNoAdvertisementsAssigned()
        {
            var expectedChannel = SetupChannel();

            var result = await _channelRepository.HasAdvertisementsAsync(expectedChannel.Id);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteAnExistingChannel()
        {
            var expectedChannel = SetupChannel();

            await _channelRepository.DeleteAsync(expectedChannel.Id);

            _dbContext.Channels.Any(x => x.Id == expectedChannel.Id).Should().BeFalse();
        }

        [Fact]
        public async Task ChannelsExistAsync_ShouldReturnFalseGivenTheIdsOnTheListDoNotExist()
        {
            var list = _fixture.CreateMany<int>();

            var result = await _channelRepository.ChannelsExistAsync(list);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task ChannelsExistAsync_ShouldReturnTrueGivenTheIdsOnTheListExist()
        {
            var expectedChannel = SetupChannel();
            var list = new List<int> {expectedChannel.Id};

            var result = await _channelRepository.ChannelsExistAsync(list);

            result.Should().BeTrue();
        }

        private Channel SetupChannel()
        {
            var channel = _fixture.Build<Channel>()
                .Without(x => x.AdvertisementChannels)
                .Create();

            _dbContext.Channels.Add(channel);
            _dbContext.SaveChanges();

            return channel;
        }

        private Advertisement SetupAdvertisement()
        {
            var advertisement = _fixture.Build<Advertisement>()
                .Without(x => x.AdvertisementChannels)
                .Create();
            _dbContext.Advertisements.Add(advertisement);
            _dbContext.SaveChanges();

            return advertisement;
        }

        private AdvertisementChannel SetupAdvertisementChannel(int advertisementId, int channelId)
        {
            var advertisementChannel = new AdvertisementChannel
            {
                AdvertisementId = advertisementId,
                ChannelId = channelId
            };

            _dbContext.AdvertisementChannels.Add(advertisementChannel);
            _dbContext.SaveChanges();

            return advertisementChannel;
        }

        private static void DomainShouldBeEquivalent(Domain.Domains.Channel channel, Channel expectedChannel)
        {
            channel.Should().BeEquivalentTo(
                expectedChannel,
                options => options.Excluding(x => x.AdvertisementChannels));
        }
    }
}
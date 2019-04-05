using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Marketing.Domain.Domains;
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
    public class AdvertisementRepositoryTests : IClassFixture<ClassTestFixture>
    {
        private readonly AdvertisementRepository _advertisementRepository;
        private readonly Fixture _fixture;
        private readonly MarketingDbContext _dbContext;

        public AdvertisementRepositoryTests(ClassTestFixture testFixture) 
        {
            _dbContext = testFixture.Context;
            _advertisementRepository = new AdvertisementRepository(_dbContext, new AdvertisementMapper());
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyListGivenNoItemsAreFound()
        {
            var result = await _advertisementRepository.GetAllAsync();

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfItemsFound()
        {
            var expectedAdvertisement = SetupAdvertisement();
            var expectedChannel = SetupChannel();
            SetupAdvertisementChannel(expectedAdvertisement.Id, expectedChannel.Id);

            var result = await _advertisementRepository.GetAllAsync();

            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().ContainSingle(x => x.Id == expectedAdvertisement.Id);
            var advertisement = result.Single(x => x.Id == expectedAdvertisement.Id);
            DomainShouldBeEquivalent(advertisement, expectedAdvertisement, expectedChannel);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNullGivenTheIdIsNotFound()
        {
            var nonExistantId = _fixture.Create<int>();

            var result = await _advertisementRepository.GetByIdAsync(nonExistantId);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnItemGivenTheIdIsFound()
        {
            var expectedAdvertisement = SetupAdvertisement();
            var expectedChannel = SetupChannel();
            SetupAdvertisementChannel(expectedAdvertisement.Id, expectedChannel.Id);

            var result = await _advertisementRepository.GetByIdAsync(expectedAdvertisement.Id);

            result.Should().NotBeNull();
            DomainShouldBeEquivalent(result, expectedAdvertisement, expectedChannel);
        }

        [Fact]
        public async Task GetByClientIdAsync_ShouldReturnNullGivenTheClientIdIsNotFound()
        {
            var nonExistantClientId = _fixture.Create<int>();

            var result = await _advertisementRepository.GetByClientIdAsync(nonExistantClientId);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetByClientIdAsync_ShouldReturnItemGivenTheClientIdIsFound()
        {
            var expectedAdvertisement = SetupAdvertisement();
            var expectedChannel = SetupChannel();
            SetupAdvertisementChannel(expectedAdvertisement.Id, expectedChannel.Id);

            var result = await _advertisementRepository.GetByClientIdAsync(expectedAdvertisement.ClientId);

            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().ContainSingle(x => x.Id == expectedAdvertisement.Id);
            var advertisement = result.Single(x => x.Id == expectedAdvertisement.Id);
            DomainShouldBeEquivalent(advertisement, expectedAdvertisement, expectedChannel);
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnFalseGivenANotMatchingId()
        {
            var nonExistantId = _fixture.Create<int>();

            var result = await _advertisementRepository.ExistsAsync(nonExistantId);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnTrueGivenAMatchingId()
        {
            var expectedAdvertisement = SetupAdvertisement();
            
            var result = await _advertisementRepository.ExistsAsync(expectedAdvertisement.Id);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateTheItemEntry()
        {
            var expectedChannel = SetupChannel();
            var advertisementEntry = _fixture.Build<AdvertisementEntry>()
                .With(x => x.ChannelIds, new List<int> { expectedChannel.Id })
                .Create();
            
            var result = await _advertisementRepository.CreateAsync(advertisementEntry);

            result.Should().NotBeNull();
            _dbContext.Advertisements.Any(x => x.Id == result.Id).Should().BeTrue();
        }

        [Fact(Skip = "Review tracking exception")]
        public async Task UpdateAsync_ShouldUpdateTheItemEntry()
        {
            var originalAdvertisement = SetupAdvertisement();
            var updatedAdvertisement = new AdvertisementEntry
            {
                Id = originalAdvertisement.Id,
                ClientId = _fixture.Create<int>(),
                Name = $"{originalAdvertisement.Name} - Updated"
            };
            
            await _advertisementRepository.UpdateAsync(updatedAdvertisement);

            var currentValue = _dbContext.Advertisements
                .AsNoTracking()
                .Single(x => x.Id == updatedAdvertisement.Id);
            currentValue.Name.Should().Be(updatedAdvertisement.Name);
            currentValue.ClientId.Should().Be(updatedAdvertisement.ClientId);
        }

        [Fact]
        public async Task HasChannelsAsync_ShouldReturnTrueGivenTheAdverisementHasChannelsAssigned()
        {
            var expectedAdvertisement = SetupAdvertisement();
            var expectedChannel = SetupChannel();
            SetupAdvertisementChannel(expectedAdvertisement.Id, expectedChannel.Id);

            var result = await _advertisementRepository.HasChannelsAsync(expectedAdvertisement.Id);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task HasChannelsAsync_ShouldReturnFalseGivenTheAdverisementHasNoChannelsAssigned()
        {
            var expectedAdvertisement = SetupAdvertisement();
            
            var result = await _advertisementRepository.HasChannelsAsync(expectedAdvertisement.Id);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteAnExistingAdvertisement()
        {
            var expectedAdvertisement = SetupAdvertisement();

            await _advertisementRepository.DeleteAsync(expectedAdvertisement.Id);

            _dbContext.Advertisements.Any(x => x.Id == expectedAdvertisement.Id).Should().BeFalse();
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

        private static void DomainShouldBeEquivalent(Domain.Domains.Advertisement advertisement, Advertisement expectedAdvertisement, Channel expectedChannel)
        {
            advertisement.Should().BeEquivalentTo(
                expectedAdvertisement,
                options => options.Excluding(x => x.AdvertisementChannels));
            advertisement.Channels.First().Should().BeEquivalentTo(
                expectedChannel,
                options => options.Excluding(x => x.AdvertisementChannels));
        }
    }
}
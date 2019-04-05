using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using Marketing.Persistence.Entities;
using Marketing.Persistence.Mappers;
using Xunit;

namespace Marketing.Persistence.UnitTests.Mappers
{
    public class AdvertisementMapperTests
    {
        private readonly AdvertisementMapper _mapper;
        private readonly Fixture _fixture;

        public AdvertisementMapperTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _mapper = new AdvertisementMapper();
        }

        [Fact]
        public void ToDomain_ShouldMapMainEntityDetailsToDomain()
        {
            var entity = _fixture.Build<Advertisement>()
                .With(x => x.AdvertisementChannels, new List<AdvertisementChannel>())
                .Create();

            var domain = _mapper.ToDomain(entity);

            domain.Should().NotBeNull();
            DetailsShouldBeEquivalent(domain, entity);
        }

        [Fact]
        public void ToDomain_ShouldMapEntityNestedObjectsToDomain()
        {
            var advertisementChannels = _fixture.Build<AdvertisementChannel>()
                .Without(x => x.Advertisement)
                .CreateMany()
                .ToList();
            var entity = _fixture.Build<Advertisement>()
                .With(x => x.AdvertisementChannels, advertisementChannels)
                .Create();

            var domain = _mapper.ToDomain(entity);

            ChannelsShouldBeEquivalent(domain, entity);
        }

        [Fact]
        public void ToDomains_ShouldMapMainEntityDetailsToDomains()
        {
            var entities = _fixture.Build<Advertisement>()
                .With(x => x.AdvertisementChannels, new List<AdvertisementChannel>())
                .CreateMany();

            var domains = _mapper.ToDomains(entities);

            domains.Should().NotBeNullOrEmpty();
            domains.Should().HaveCount(entities.Count());
            domains.Should().BeEquivalentTo(
                entities,
                options => options.Excluding(x => x.AdvertisementChannels));
        }

        [Fact]
        public void ToDomains_ShouldMapEntityNestedObjectsToDomains()
        {
            var advertisementChannels = _fixture.Build<AdvertisementChannel>()
                .Without(x => x.Advertisement)
                .CreateMany()
                .ToList();
            var entities = _fixture.Build<Advertisement>()
                .With(x => x.AdvertisementChannels, advertisementChannels)
                .CreateMany();

            var domains = _mapper.ToDomains(entities);

            foreach (var entity in entities)
            {
                var domain = domains.Single(x => x.Id == entity.Id);
                ChannelsShouldBeEquivalent(domain, entity);
            }
        }

        [Fact]
        public void ToEntity_ShouldMapMainDomainDetailsToEntity()
        {
            var domain = _fixture.Build<Domain.Domains.AdvertisementEntry>()
                .Create();

            var entity = _mapper.ToEntity(domain);

            domain.Should().NotBeNull();
            DetailsShouldBeEquivalent(domain, entity);
        }

        private static void DetailsShouldBeEquivalent(Domain.Domains.Advertisement domain, Advertisement entity)
        {
            domain.Id.Should().Be(entity.Id);
            domain.Name.Should().Be(entity.Name);
            domain.ClientId.Should().Be(entity.ClientId);
        }

        private static void DetailsShouldBeEquivalent(Domain.Domains.AdvertisementEntry domain, Advertisement entity)
        {
            domain.Id.Should().Be(entity.Id);
            domain.Name.Should().Be(entity.Name);
            domain.ClientId.Should().Be(entity.ClientId);
        }

        private static void ChannelsShouldBeEquivalent(Domain.Domains.Advertisement domain, Advertisement entity)
        {
            var expectedChannels = entity.AdvertisementChannels.Select(x => x.Channel);
            var mappedChannels = domain.Channels;
            mappedChannels.Should().NotBeNullOrEmpty();
            mappedChannels.Should().HaveCount(expectedChannels.Count());
            mappedChannels.Should().BeEquivalentTo(
                expectedChannels,
                options => options.Excluding(x => x.AdvertisementChannels));
        }
    }
}
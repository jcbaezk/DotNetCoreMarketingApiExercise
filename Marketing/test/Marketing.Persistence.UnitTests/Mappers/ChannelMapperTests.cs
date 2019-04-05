using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using Marketing.Persistence.Entities;
using Marketing.Persistence.Mappers;
using Xunit;

namespace Marketing.Persistence.UnitTests.Mappers
{
    public class ChannelMapperTests
    {
        private readonly ChannelMapper _mapper;
        private readonly Fixture _fixture;

        public ChannelMapperTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _mapper = new ChannelMapper();
        }

        [Fact]
        public void ToDomain_ShouldMapMainEntityDetailsToDomain()
        {
            var entity = _fixture.Build<Channel>()
                .With(x => x.AdvertisementChannels, new List<AdvertisementChannel>())
                .Create();

            var domain = _mapper.ToDomain(entity);

            domain.Should().NotBeNull();
            DetailsShouldBeEquivalent(domain, entity);
        }
        
        [Fact]
        public void ToDomains_ShouldMapMainEntityDetailsToDomains()
        {
            var entities = _fixture.Build<Channel>()
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
        public void ToEntity_ShouldMapMainDomainDetailsToEntity()
        {
            var domain = _fixture.Build<Domain.Domains.Channel>()
                .Create();

            var entity = _mapper.ToEntity(domain);

            domain.Should().NotBeNull();
            DetailsShouldBeEquivalent(domain, entity);
        }

        private static void DetailsShouldBeEquivalent(Domain.Domains.Channel domain, Channel entity)
        {
            domain.Id.Should().Be(entity.Id);
            domain.Name.Should().Be(entity.Name);
            domain.IsDigital.Should().Be(entity.IsDigital);
        }
    }
}
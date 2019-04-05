using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Marketing.Api.IntegrationTests.TestSetup;
using Marketing.Domain.Domains;
using Xunit;

namespace Marketing.Api.IntegrationTests.Controllers
{
    public class ChannelControllerTests : ApiTest
    {
        private readonly Fixture _fixture;

        public ChannelControllerTests(ClassTestFixture testFixture)
            : base(testFixture)
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetAllAsync_ShouldGetAllItems()
        {
            var result = await GetAsync("/api/v1/channel/all");

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldTryToGetById()
        {
            var id = _fixture.Create<int>();

            var result = await GetAsync($"/api/v1/channel/{id}");

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateItem()
        {
            var channel = _fixture.Create<Channel>();

            var result = await PostAsync("/api/v1/channel/", channel);

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task UpdateAsync_ShouldTryToUpdateItem()
        {
            var channel = _fixture.Create<Channel>();

            var result = await PutAsync("/api/v1/channel/", channel);

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteAsync_ShouldTryToDeleteItem()
        {
            var id = _fixture.Create<int>();

            var result = await DeleteAsync($"/api/v1/channel/{id}");

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
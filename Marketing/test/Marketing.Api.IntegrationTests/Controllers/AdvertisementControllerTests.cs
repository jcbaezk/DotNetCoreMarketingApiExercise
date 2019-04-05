using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Marketing.Api.IntegrationTests.TestSetup;
using Marketing.Domain.Domains;
using Xunit;

namespace Marketing.Api.IntegrationTests.Controllers
{
    public class AdvertisementControllerTests : ApiTest
    {
        private readonly Fixture _fixture;

        public AdvertisementControllerTests(ClassTestFixture testFixture) 
            : base(testFixture)
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetAllAsync_ShouldGetAllItems()
        {
            var result = await GetAsync("/api/v1/Advertisement/all");

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldTryToGetById()
        {
            var id = _fixture.Create<int>();

            var result = await GetAsync($"/api/v1/Advertisement/{id}");

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetByClientIdAsync_ShouldGetByClientId()
        {
            var id = _fixture.Create<int>();

            var result = await GetAsync($"/api/v1/Advertisement/client/{id}");

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateItem()
        {
            var advertisement = _fixture.Create<AdvertisementEntry>();

            var result = await PostAsync("/api/v1/Advertisement/", advertisement);

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateAsync_ShouldTryToUpdateItem()
        {
            var advertisement = _fixture.Create<AdvertisementEntry>();

            var result = await PutAsync("/api/v1/Advertisement/", advertisement);

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeleteAsync_ShouldTryToDeleteItem()
        {
            var id = _fixture.Create<int>();

            var result = await DeleteAsync($"/api/v1/Advertisement/{id}");

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
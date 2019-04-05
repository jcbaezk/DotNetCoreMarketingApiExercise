using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Marketing.Api.Controllers;
using Marketing.Domain.Domains;
using Marketing.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Marketing.Api.UnitTests.Controllers
{
    public class AdvertisementControllerTest
    {
        private readonly AdvertisementController _controller;
        private readonly Mock<IAdvertisementService> _advertisementService;
        private readonly Mock<IChannelService> _channelService;

        public AdvertisementControllerTest()
        {
            _advertisementService = new Mock<IAdvertisementService>();
            _channelService = new Mock<IChannelService>();
            _controller = new AdvertisementController(_advertisementService.Object, _channelService.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNotFoundResponseGivenTheIdDoesNotMatchAnAdvertisement()
        {
            var nonExistentId = new Fixture().Create<int>();
            _advertisementService.Setup(x => x.GetByIdAsync(nonExistentId)).ReturnsAsync((Advertisement) null);

            var result = await _controller.GetByIdAsync(nonExistentId);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnOkResponseGivenTheIdMatchesAnAdvertisement()
        {
            var matchingId = new Fixture().Create<int>();
            _advertisementService.Setup(x => x.GetByIdAsync(matchingId)).ReturnsAsync(new Advertisement());

            var result = await _controller.GetByIdAsync(matchingId);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnBadRequestResponseGivenTheInputIsNotValid()
        {
            var advertisementEntry = new Fixture().Create<AdvertisementEntry>();
            _controller.ModelState.AddModelError("test", "test");

            var result = await _controller.CreateAsync(advertisementEntry);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnBadRequestResponseGivenTheChannelIdsAreNotValid()
        {
            var advertisementEntry = new Fixture().Create<AdvertisementEntry>();
            _channelService.Setup(x => x.ChannelsExistAsync(It.IsAny<IEnumerable<int>>())).ReturnsAsync(false);
            
            var result = await _controller.CreateAsync(advertisementEntry);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedResponseGivenAValidItem()
        {
            var advertisementEntry = new Fixture().Create<AdvertisementEntry>();
            _channelService.Setup(x => x.ChannelsExistAsync(It.IsAny<IEnumerable<int>>())).ReturnsAsync(true);
            _advertisementService.Setup(x => x.CreateAsync(advertisementEntry)).ReturnsAsync(new Advertisement());

            var result = await _controller.CreateAsync(advertisementEntry);

            result.Should().BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnBadRequestResponseGivenTheInputIsNotValid()
        {
            var advertisementEntry = new Fixture().Create<AdvertisementEntry>();
            _controller.ModelState.AddModelError("test", "test");

            var result = await _controller.UpdateAsync(advertisementEntry);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnBadRequestResponseGivenTheChannelIdsAreNotValid()
        {
            var advertisementEntry = new Fixture().Create<AdvertisementEntry>();
            _channelService.Setup(x => x.ChannelsExistAsync(It.IsAny<IEnumerable<int>>())).ReturnsAsync(false);

            var result = await _controller.UpdateAsync(advertisementEntry);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNotFoundResponseGivenTheIdForTheEntryDoesNotExist()
        {
            var advertisementEntry = new Fixture().Create<AdvertisementEntry>();
            _channelService.Setup(x => x.ChannelsExistAsync(It.IsAny<IEnumerable<int>>())).ReturnsAsync(true);
            _advertisementService.Setup(x => x.ExistsAsync(advertisementEntry.Id)).ReturnsAsync(false);

            var result = await _controller.UpdateAsync(advertisementEntry);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNoContentResponseGivenTheIdExistsAndTheEntryWasUpdated()
        {
            var advertisementEntry = new Fixture().Create<AdvertisementEntry>();
            _channelService.Setup(x => x.ChannelsExistAsync(It.IsAny<IEnumerable<int>>())).ReturnsAsync(true);
            _advertisementService.Setup(x => x.ExistsAsync(advertisementEntry.Id)).ReturnsAsync(true);

            var result = await _controller.UpdateAsync(advertisementEntry);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnNotFoundResponseGivenTheIdDoesNotMatchAnAdvertisement()
        {
            var nonExistentId = new Fixture().Create<int>();
            _advertisementService.Setup(x => x.ExistsAsync(nonExistentId)).ReturnsAsync(false);

            var result = await _controller.DeleteAsync(nonExistentId);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnBadRequestResponseGivenTheAdvertisementHasChannelsAndCannotBeDeleted()
        {
            var id = new Fixture().Create<int>();
            _advertisementService.Setup(x => x.ExistsAsync(id)).ReturnsAsync(true);
            _advertisementService.Setup(x => x.HasChannelsAsync(id)).ReturnsAsync(true);

            var result = await _controller.DeleteAsync(id);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnAcceptedResponseGivenTheAdvertisementCanBeDeleted()
        {
            var id = new Fixture().Create<int>();
            _advertisementService.Setup(x => x.ExistsAsync(id)).ReturnsAsync(true);
            _advertisementService.Setup(x => x.HasChannelsAsync(id)).ReturnsAsync(false);

            var result = await _controller.DeleteAsync(id);

            result.Should().BeOfType<AcceptedResult>();
        }
    }
}
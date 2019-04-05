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
    public class ChannelControllerTest
    {
        private readonly ChannelController _controller;
        private readonly Mock<IChannelService> _channelService;

        public ChannelControllerTest()
        {
            _channelService = new Mock<IChannelService>();
            _controller = new ChannelController(_channelService.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNotFoundResponseGivenTheIdDoesNotMatchAChannel()
        {
            var nonExistentId = new Fixture().Create<int>();
            _channelService.Setup(x => x.ExistsAsync(nonExistentId)).ReturnsAsync(false);

            var result = await _controller.GetByIdAsync(nonExistentId);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnOkResponseGivenTheIdMatchesAnAdvertisement()
        {
            var matchingId = new Fixture().Create<int>();
            _channelService.Setup(x => x.ExistsAsync(matchingId)).ReturnsAsync(true);

            var result = await _controller.GetByIdAsync(matchingId);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnBadRequestResponseGivenTheInputIsNotValid()
        {
            var channel = new Fixture().Create<Channel>();
            _controller.ModelState.AddModelError("test", "test");

            var result = await _controller.CreateAsync(channel);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedResponseGivenAValidItem()
        {
            var channel = new Fixture().Create<Channel>();
            _channelService.Setup(x => x.CreateAsync(channel)).ReturnsAsync(channel);

            var result = await _controller.CreateAsync(channel);

            result.Should().BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnBadRequestResponseGivenTheInputIsNotValid()
        {
            var channel = new Fixture().Create<Channel>();
            _controller.ModelState.AddModelError("test", "test");

            var result = await _controller.UpdateAsync(channel);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNotFoundResponseGivenTheIdForTheEntryDoesNotExist()
        {
            var channel = new Fixture().Create<Channel>();
            _channelService.Setup(x => x.ExistsAsync(channel.Id)).ReturnsAsync(false);

            var result = await _controller.UpdateAsync(channel);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNoContentResponseGivenTheIdExistsAndTheEntryWasUpdated()
        {
            var channel = new Fixture().Create<Channel>();
            _channelService.Setup(x => x.ExistsAsync(channel.Id)).ReturnsAsync(true);

            var result = await _controller.UpdateAsync(channel);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnNotFoundResponseGivenTheIdDoesNotMatchAChannel()
        {
            var nonExistentId = new Fixture().Create<int>();
            _channelService.Setup(x => x.ExistsAsync(nonExistentId)).ReturnsAsync(false);

            var result = await _controller.DeleteAsync(nonExistentId);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnBadRequestResponseGivenTheChannelHasAdvertisementsAndCannotBeDeleted()
        {
            var id = new Fixture().Create<int>();
            _channelService.Setup(x => x.ExistsAsync(id)).ReturnsAsync(true);
            _channelService.Setup(x => x.HasAdvertisementsAsync(id)).ReturnsAsync(true);

            var result = await _controller.DeleteAsync(id);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnAcceptedResponseGivenTheChannelCanBeDeleted()
        {
            var id = new Fixture().Create<int>();
            _channelService.Setup(x => x.ExistsAsync(id)).ReturnsAsync(true);
            _channelService.Setup(x => x.HasAdvertisementsAsync(id)).ReturnsAsync(false);

            var result = await _controller.DeleteAsync(id);

            result.Should().BeOfType<AcceptedResult>();
        }
    }
}
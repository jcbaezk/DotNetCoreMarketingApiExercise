using System.Collections.Generic;
using System.Threading.Tasks;
using Marketing.Domain.Domains;
using Marketing.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Marketing.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ChannelController : Controller
    {
        private readonly IChannelService _channelService;

        public ChannelController(IChannelService channelService)
        {
            _channelService = channelService;
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Channel>))]
        public async Task<IActionResult> GetAllAsync()
        {
            var advertisements = await _channelService.GetAllAsync();

            return Ok(advertisements);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Channel))]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var exists = await _channelService.ExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            var advertisement = await _channelService.GetByIdAsync(id);

            return Ok(advertisement);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAsync([FromBody]Channel channelEntry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var channel = await _channelService.CreateAsync(channelEntry);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = channel.Id }, null);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateAsync([FromBody]Channel channelEntry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exists = await _channelService.ExistsAsync(channelEntry.Id);
            if (!exists)
            {
                return NotFound();
            }

            await _channelService.UpdateAsync(channelEntry);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var exists = await _channelService.ExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            var hasAdvertisements = await _channelService.HasAdvertisementsAsync(id);
            if (hasAdvertisements)
            {
                return BadRequest();
            }

            await _channelService.DeleteAsync(id);

            return Accepted();
        }
    }
}

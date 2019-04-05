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
    public class AdvertisementController : Controller
    {
        private readonly IAdvertisementService _advertisementService;
        private readonly IChannelService _channelService;

        public AdvertisementController(IAdvertisementService advertisementService, IChannelService channelService)
        {
            _advertisementService = advertisementService;
            _channelService = channelService;
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Advertisement>))]
        public async Task<IActionResult> GetAllAsync()
        {
            var advertisements = await _advertisementService.GetAllAsync();

            return Ok(advertisements);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Advertisement))]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var advertisement = await _advertisementService.GetByIdAsync(id);

            if (advertisement == null)
            {
                return NotFound();
            }

            return Ok(advertisement);
        }

        [HttpGet("client/{clientId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Advertisement>))]
        public async Task<IActionResult> GetByClientIdAsync(int clientId)
        {
            var advertisements = await _advertisementService.GetByClientIdAsync(clientId);

            return Ok(advertisements);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAsync([FromBody]AdvertisementEntry advertisementEntry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _channelService.ChannelsExistAsync(advertisementEntry.ChannelIds))
            {
                return BadRequest();
            }

            var advertisement = await _advertisementService.CreateAsync(advertisementEntry);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = advertisement.Id }, null);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateAsync([FromBody]AdvertisementEntry advertisementEntry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _channelService.ChannelsExistAsync(advertisementEntry.ChannelIds))
            {
                return BadRequest();
            }

            var exists = await _advertisementService.ExistsAsync(advertisementEntry.Id);
            if (!exists)
            {
                return NotFound();
            }

            await _advertisementService.UpdateAsync(advertisementEntry);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var exists = await _advertisementService.ExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            var hasChannels = await _advertisementService.HasChannelsAsync(id);
            if (hasChannels)
            {
                return BadRequest();
            }

            await _advertisementService.DeleteAsync(id);

            return Accepted();
        }
    }
}

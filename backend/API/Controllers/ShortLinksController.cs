using System.Security.Claims;
using Core.Dtos;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/links")]
    [ApiController]
    [Authorize]
    public class ShortLinksController(IShortLinkService linkService) : ControllerBase
    {
        private string? GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;    
        }

        [HttpPost]
        public async Task<ActionResult<LinkDto>> CreateShortLink(CreateLinkDto createLinkDto)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var link = await linkService.CreateShortLinkAsync(createLinkDto, userId);
            return CreatedAtAction(nameof(GetShortLink), new {id = link.Id}, link);
        }

        [HttpGet]
        public async Task<ActionResult<IList<LinkDto>>> GetShortLinkList()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var shortLinks = await linkService.GetListOfShortLinksAsync(userId);
            return Ok(shortLinks);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LinkDto?>> GetShortLink(int id)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var link = await linkService.GetShortLinkAsync(id, userId);
            if (link == null) return NotFound();

            return Ok(link);
        }

        [HttpGet("{id:int}/analytics")]
        public async Task<ActionResult<LinkAnalyticsDto>> GetLinkAnalytics(int id)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var analytics = await linkService.GetLinkAnalyticsAsync(id, userId);
            if (analytics == null) return NotFound();

            return analytics;
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateShortLink(int id, UpdateLinkDto updateLinkDto)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var updateAttempt = await linkService.UpdateLinkAsync(id, updateLinkDto, userId);
            if (updateAttempt == null) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteShortLink(int id)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var deleteAttempt = await linkService.DeleteShortLinkAsync(id, userId);
            if (!deleteAttempt) return NotFound();

            return NoContent();
        }
    }
}

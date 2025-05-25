using Microsoft.AspNetCore.Mvc;
using FinancePlus.Models;
using FinancePlus.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace FinancePlus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortDir = "asc")
        {
            var result = await _service.GetPagedAsync(page, pageSize, search, sortBy, sortDir);
            return Ok(result);
        }



        [HttpGet("{uuid}")]
        public async Task<IActionResult> GetByUuid(Guid uuid)
        {
            var user = await _service.GetByUuidAsync(uuid);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            var created = await _service.CreateAsync(user);
            return CreatedAtAction(nameof(GetByUuid), new { uuid = created.Uuid }, created);
        }

        [HttpPut("{uuid}")]
        public async Task<IActionResult> Update(Guid uuid, User user)
        {
            var updated = await _service.UpdateAsync(uuid, user);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{uuid}")]
        public async Task<IActionResult> Delete(Guid uuid)
        {
            var success = await _service.DeleteAsync(uuid);
            return success ? NoContent() : NotFound();
        }
    }
}

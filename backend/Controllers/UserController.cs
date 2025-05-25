using Microsoft.AspNetCore.Mvc;
using FinancePlus.Models;
using FinancePlus.Services.Interfaces;

namespace FinancePlus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _service.GetAllAsync();
            return Ok(users);
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

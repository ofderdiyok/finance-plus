using FinancePlus.Models;
using FinancePlus.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinancePlus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _service;

        public TransactionController(ITransactionService service)
        {
            _service = service;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortDir = "desc")
        {
            var result = await _service.GetPagedAsync(page, pageSize, search, sortBy, sortDir);
            return Ok(result);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyTransactions(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortDir = "desc",
            [FromQuery] Guid? categoryUuid = null)
        {
            var userUuid = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _service.GetPagedByUserAsync(userUuid, page, pageSize, search, sortBy, sortDir, categoryUuid);
            return Ok(result);
        }


        [HttpPost("me")]
        public async Task<IActionResult> Create(Transaction transaction)
        {
            var userUuid = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var created = await _service.CreateAsync(userUuid, transaction);
            return CreatedAtAction(nameof(GetMyTransactions), new { uuid = created.Uuid }, created);
        }

        [HttpPut("me/{uuid}")]
        public async Task<IActionResult> Update(Guid uuid, Transaction transaction)
        {
            var userUuid = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var updated = await _service.UpdateAsync(userUuid, uuid, transaction);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("me/{uuid}")]
        public async Task<IActionResult> Delete(Guid uuid)
        {
            var userUuid = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var success = await _service.DeleteAsync(userUuid, uuid);
            return success ? NoContent() : NotFound();
        }
    }
}

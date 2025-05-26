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
    public class TransferController : ControllerBase
    {
        private readonly ITransferService _service;

        public TransferController(ITransferService service)
        {
            _service = service;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyTransfers(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortDir = "desc")
        {
            var senderUuid =  Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _service.GetPagedBySenderAsync(senderUuid, page, pageSize, search, sortBy, sortDir);
            return Ok(result);
        }

        [HttpPost("me")]
        public async Task<IActionResult> Create(Transfer transfer)
        {
            var senderUuid = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var created = await _service.CreateAsync(senderUuid, transfer);
            return CreatedAtAction(nameof(GetMyTransfers), new { uuid = created.Uuid }, created);
        }

        [HttpPut("me/{uuid}")]
        public async Task<IActionResult> Update(Guid uuid, Transfer transfer)
        {
            var senderUuid = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var existing = await _service.GetByUuidAsync(uuid);

            if (existing == null || existing.senderUuid != senderUuid)
                return NotFound();

            existing.Amount = transfer.Amount;
            existing.Description = transfer.Description;
            existing.ReceiverUuid = transfer.ReceiverUuid;
            existing.TransferType = transfer.TransferType;
            existing.UpdatedAt = DateTime.UtcNow;

            //track eder saveler.
            await _service.SaveAsync(); 

            return Ok(existing);
        }


        [HttpDelete("me/{uuid}")]
        public async Task<IActionResult> Delete(Guid uuid)
        {
            var senderUuid = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var success = await _service.DeleteAsync(senderUuid, uuid);
            return success ? NoContent() : NotFound();
        }
    }
}
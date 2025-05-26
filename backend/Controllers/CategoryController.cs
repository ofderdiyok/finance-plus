using FinancePlus.Models;
using FinancePlus.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinancePlus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 100,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortDir = "asc")
        {
            var result = await _service.GetPagedAsync(page, pageSize, search, sortBy, sortDir);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            var created = await _service.CreateAsync(category);
            return CreatedAtAction(nameof(GetAll), new { uuid = created.Uuid }, created);
        }

        [HttpPut("{uuid}")]
        public async Task<IActionResult> Update(Guid uuid, Category category)
        {
            var updated = await _service.UpdateAsync(uuid, category);
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

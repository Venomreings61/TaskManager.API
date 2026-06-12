using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Models.DTOs;
using TaskManager.API.Services;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly TaskService _taskService;

        public TaskController(TaskService taskService)
        {
            _taskService = taskService;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirst(
                ClaimTypes.NameIdentifier)!.Value);

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _taskService
                .GetAllAsync(GetUserId());
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _taskService
                .GetByIdAsync(id, GetUserId());
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskDTO dto)
        {
            var task = await _taskService
                .CreateAsync(dto, GetUserId());
            return CreatedAtAction(
                nameof(GetById),
                new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id, UpdateTaskDTO dto)
        {
            var task = await _taskService
                .UpdateAsync(id, dto, GetUserId());
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _taskService
                .DeleteAsync(id, GetUserId());
            if (!result) return NotFound();
            return NoContent();
        }
    }
}

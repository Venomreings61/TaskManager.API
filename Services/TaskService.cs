using Microsoft.EntityFrameworkCore;
using TaskManager.API.Data;
using TaskManager.API.Models;
using TaskManager.API.Models.DTOs;

namespace TaskManager.API.Services
{
    public class TaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaskResponseDTO>> GetAllAsync(int userId)
        {
            return await _context.Tasks
                .Where(t => t.UserId == userId)
                .Select(t => new TaskResponseDTO
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    CreatedAt = t.CreatedAt,
                    CompletedAt = t.CompletedAt
                })
                .ToListAsync();
        }

        public async Task<TaskResponseDTO?> GetByIdAsync(int id, int userId)
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null) return null;

            return new TaskResponseDTO
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt,
                CompletedAt = task.CompletedAt
            };
        }

        public async Task<TaskResponseDTO> CreateAsync(
            CreateTaskDTO dto, int userId)
        {
            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                UserId = userId
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return new TaskResponseDTO
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt
            };
        }

        public async Task<TaskResponseDTO?> UpdateAsync(
            int id, UpdateTaskDTO dto, int userId)
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null) return null;

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.IsCompleted = dto.IsCompleted;
            task.CompletedAt = dto.IsCompleted ? DateTime.UtcNow : null;

            await _context.SaveChangesAsync();

            return new TaskResponseDTO
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt,
                CompletedAt = task.CompletedAt
            };
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

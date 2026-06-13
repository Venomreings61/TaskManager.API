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

        public async Task<PagedResponseDTO<TaskResponseDTO>> GetAllAsync(
            int userId, bool? isCompleted, int page, int pageSize)
        {
            var query = _context.Tasks.Where(t => t.UserId == userId);

            // Filtering
            if (isCompleted.HasValue)
                query = query.Where(t => t.IsCompleted == isCompleted.Value);

            var totalCount = await query.CountAsync();

            // Pagination
            var items = await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => MapToDto(t))
                .ToListAsync();

            return new PagedResponseDTO<TaskResponseDTO>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

        public async Task<TaskResponseDTO?> GetByIdAsync(int id, int userId)
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            return task == null ? null : MapToDto(task);
        }

        public async Task<TaskResponseDTO> CreateAsync(
            CreateTaskDTO dto, int userId)
        {
            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                UserId = userId
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return MapToDto(task);
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
            task.Priority = dto.Priority;
            task.CompletedAt = dto.IsCompleted ? DateTime.UtcNow : null;

            await _context.SaveChangesAsync();

            return MapToDto(task);
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

        private static TaskResponseDTO MapToDto(TaskItem t) => new()
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            IsCompleted = t.IsCompleted,
            Priority = t.Priority,
            CreatedAt = t.CreatedAt,
            CompletedAt = t.CompletedAt
        };
    }
}

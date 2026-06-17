using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskAPI.Data;
using TaskAPI.Models;
using TaskStatus = TaskAPI.Models.TaskStatus;

namespace TaskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Requires JWT token
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
        {
            var userId = GetUserId();
            var tasks = await _context.Tasks
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return Ok(tasks);
        }

        // GET: api/tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTask(int id)
        {
            var userId = GetUserId();
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null)
                return NotFound(new { message = "Task not found" });

            return Ok(task);
        }

        // POST: api/tasks
        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateTask([FromBody] CreateTaskRequest request)
        {
            var userId = GetUserId();

            var task = new TaskItem
            {
                Title = request.Title,
                Description = request.Description,
                Status = request.Status,
                Priority = request.Priority,
                DueDate = request.DueDate,
                UserId = userId
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        // PUT: api/tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskRequest request)
        {
            var userId = GetUserId();
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null)
                return NotFound(new { message = "Task not found" });

            task.Title = request.Title;
            task.Description = request.Description;
            task.Status = request.Status;
            task.Priority = request.Priority;
            task.DueDate = request.DueDate;
            task.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(task);
        }

        // DELETE: api/tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var userId = GetUserId();
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null)
                return NotFound(new { message = "Task not found" });

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Task deleted successfully" });
        }

        // Helper method to get the current user's ID from JWT
        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User ID not found in token");

            return int.Parse(userIdClaim.Value);
        }
    }

    // Request DTOs
    public class CreateTaskRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskStatus Status { get; set; } = TaskStatus.ToDo;
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public DateTime? DueDate { get; set; }
    }

    public class UpdateTaskRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
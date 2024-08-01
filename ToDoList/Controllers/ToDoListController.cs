using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Data;
using ToDoList.Model;
using ToDoList.EmailService;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoListController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly Email _emailService; // Corrected Email service interface

        public ToDoListController(ApplicationDbContext context, Email emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // Endpoint to add a new task
        [HttpPost]
        public async Task<IActionResult> AddTask([FromQuery] string taskName, [FromQuery] string taskDescription,
            [FromQuery] string taskStatus, [FromQuery] string taskType)
        {
            var task = new AddTaskModel
            {
                Task_ID = Guid.NewGuid(),
                Task_Name = taskName,
                Task_Description = taskDescription,
                Task_Status = taskStatus,
                Task_Type = taskType,
                Task_StatusDescription = "Task is created at " + DateTime.Now.ToLocalTime().ToString("yyyy-MMM-dd hh:mm")
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return Ok("Task added successfully");
        }

        // Endpoint to get all tasks
        [HttpGet("tasks")]
        public async Task<IActionResult> GetTasks()
        {
            var tasks = await _context.Tasks.ToListAsync();
            return Ok(tasks);
        }

        // Endpoint to send email with tasks
        [HttpGet("send-email")]
        public async Task<IActionResult> SendEmail([FromQuery] string email)
        {
            var tasks = await _context.Tasks.ToListAsync();
            var htmlContent = "<html><body><h1>Tasks List</h1><table border='1'><tr><th>ID</th><th>Name</th><th>Description</th><th>Status</th><th>Type</th><th>Status Description</th></tr>";

            foreach (var task in tasks)
            {
                htmlContent += $"<tr><td>{task.Task_ID}</td><td>{task.Task_Name}</td><td>{task.Task_Description}</td><td>{task.Task_Status}</td><td>{task.Task_Type}</td><td>{task.Task_StatusDescription}</td></tr>";
            }

            htmlContent += "</table></body></html>";

            try
            {
                await _emailService.SendEmailAsync(
                    email,
                    "Task List",
                    htmlContent
                );
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return StatusCode(500, "Error sending email");
            }

            return Ok("Email sent successfully");
        }


        // Endpoint to delete a task by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound("Task not found");
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return Ok("Task deleted successfully");
        }

        // Endpoint to update a task by ID
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] AddTaskModel updatedTask)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound("Task not found");
            }

            task.Task_Name = updatedTask.Task_Name ?? task.Task_Name;
            task.Task_Description = updatedTask.Task_Description ?? task.Task_Description;
            task.Task_Status = updatedTask.Task_Status ?? task.Task_Status;
            task.Task_Type = updatedTask.Task_Type ?? task.Task_Type;
            task.Task_StatusDescription = updatedTask.Task_StatusDescription != null
                ? updatedTask.Task_StatusDescription + " Updated at " + DateTime.Now.ToLocalTime().ToString("yyyy-MMM-dd hh:mm")
                : task.Task_StatusDescription;

            await _context.SaveChangesAsync();

            return Ok("Task updated successfully");
        }
    }
}

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ToDoList.Data;
using ToDoList.Model;

/*
    ASP.NET (Active Server Pages) is a framework used for creating web applications.
    ASP.NET Core is a cross-platform version of ASP.NET that runs on Windows, macOS, and Linux.
    .NET is a versatile framework for building a variety of applications, including web, console, desktop, and mobile applications.
    .NET supports multiple languages such as C#, F#, VB.NET, and J#.

    In this example, we are creating a REST API for a ToDo application with the following features:
    1. Add Task
    2. Delete Task
    3. Update Task
    4. Track Task Status via Email
    5. Get Task Notifications if a Date/Time is mentioned in the Task Description
    6. Generate Monthly Reports to Monitor Productivity
*/

namespace ToDoList.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoListController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ToDoListController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Endpoint to add a new task
        [HttpPost]
        public async Task<IActionResult> AddTask([FromQuery] string taskName, [FromQuery] string taskDescription,
            [FromQuery] string taskStatus, [FromQuery] string taskType)
        {
            // Generate a new unique identifier for the task
            Guid newGuid = Guid.NewGuid();

            // Create a new task model instance with the provided details
            var task = new AddTaskModel
            {
                Task_ID = newGuid,
                Task_Name = taskName,
                Task_Description = taskDescription,
                Task_Status = taskStatus,
                Task_Type = taskType,
                Task_StatusDescription = "Task is created at " + DateTime.Now.ToString("yyyy-MMM-dd :hh:mm") // Default description for new tasks
            };

            // Add the new task to the database
            _context.Tasks.Add(task);

            // Save changes asynchronously
            await _context.SaveChangesAsync();

            // Return a response indicating that the data was successfully submitted
            return Ok("Data is submitted");
        }
        [HttpGet]
        public IActionResult GetTask()
        {
            // Retrieve all tasks from the database
            var tasks = _context.Tasks.ToList();

            // Return the tasks as an HTTP response
            return Ok(tasks);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            // Find the task by ID
            var task = await _context.Tasks.FindAsync(id);

            // If the task does not exist, return a 404 Not Found response
            if (task == null)
            {
                return NotFound("Task not found");
            }

            // Remove the task from the database
            _context.Tasks.Remove(task);

            // Save changes asynchronously
            await _context.SaveChangesAsync();

            // Return a response indicating that the task was successfully deleted
            return Ok("Task deleted successfully");
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] AddTaskModel updatedTask)
        {
            // Find the task by ID
            var task = await _context.Tasks.FindAsync(id);

            // If the task does not exist, return a 404 Not Found response
            if (task == null)
            {
                return NotFound("Task not found");
            }

            // Update the task with the provided details
            if (!string.IsNullOrWhiteSpace(updatedTask.Task_Name))
            {
                task.Task_Name = updatedTask.Task_Name;
            }
            if (!string.IsNullOrWhiteSpace(updatedTask.Task_Description))
            {
                task.Task_Description = updatedTask.Task_Description;
            }
            if (!string.IsNullOrWhiteSpace(updatedTask.Task_Status))
            {
                task.Task_Status = updatedTask.Task_Status;
            }
            if (!string.IsNullOrWhiteSpace(updatedTask.Task_Type))
            {
                task.Task_Type = updatedTask.Task_Type;
            }
            if (!string.IsNullOrWhiteSpace(updatedTask.Task_StatusDescription))
            {
                task.Task_StatusDescription = updatedTask.Task_StatusDescription  + DateTime.Now.ToString("yyyy-MMM-dd :hh:mm");
            }

            // Save changes asynchronously
            await _context.SaveChangesAsync();

            // Return a response indicating that the task was successfully updated
            return Ok("Task updated successfully");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using ToDoList.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Adds the services required for the application to work, such as controllers and API documentation.
builder.Services.AddControllers();  // Adds services for controllers, enabling API endpoints.

builder.Services.AddEndpointsApiExplorer();  // Adds services to generate API documentation for Swagger/OpenAPI.
builder.Services.AddSwaggerGen();  // Adds and configures Swagger generator for API documentation.


// Configure Entity Framework and SQL Server
// Retrieves the connection string from the configuration and configures Entity Framework to use SQL Server.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Build the application.
var app = builder.Build();

// Configure the HTTP request pipeline.
// Middleware components are added to the request processing pipeline here.
if (app.Environment.IsDevelopment())
{
    // In development mode, use Swagger to generate and display API documentation.
    app.UseSwagger();
    app.UseSwaggerUI();  // Provides a user interface for interacting with the API.
}

// Use HTTPS for secure communication.
app.UseHttpsRedirection();

// Adds authorization middleware to the pipeline, which is used for handling authorization policies.
app.UseAuthorization();

// Maps controller routes to the application. This enables the application to route incoming HTTP requests to the appropriate controllers.
app.MapControllers();

// Runs the application, starting the web server and listening for incoming requests.
app.Run();

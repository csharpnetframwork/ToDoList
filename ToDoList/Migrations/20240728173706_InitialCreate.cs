using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Task_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Task_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Task_Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Task_Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Task_Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Task_StatusDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Task_ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}

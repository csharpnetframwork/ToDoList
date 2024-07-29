using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Model
{
    public class AddTaskModel
    {
        [Key]
        public Guid Task_ID { get; set; }
        public string ? Task_Name { get; set; }
        public string ? Task_Description { get; set; }
        public string ? Task_Status { get; set; }
        public string ? Task_Type { get; set; }
        public string? Task_StatusDescription { get; set; } 


    }
}

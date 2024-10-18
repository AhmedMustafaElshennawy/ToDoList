using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Core.Models
{
    public class task
    {
        public int Id { get; set; }
        public required string Description { get; set; }
        public required string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public ToDolist ToDolist { get; set; }
        public int ToDolistId { get; set; }
    }
}

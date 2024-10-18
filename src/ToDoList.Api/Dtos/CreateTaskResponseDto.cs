using ToDoList.Core.Models;

namespace ToDoList.Api.Dtos
{
    public class CreateTaskResponseDto
    {
        public required int Id { get; set; }
        public required string Description { get; set; }
        public required string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public required int toDoListId { get; set; }
    }
}
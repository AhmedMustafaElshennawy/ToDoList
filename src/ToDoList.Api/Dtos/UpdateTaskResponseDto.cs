namespace ToDoList.Api.Dtos
{
    public class UpdateTaskResponseDto
    {
        public required int TaskId { get; set; }
        public required string Description { get; set; }
        public required string Name { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

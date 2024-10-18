namespace ToDoList.Api.Dtos
{
    public class GetAllTasksResponseDto
    {
        public  int Id { get; set; }
        public  string Description { get; set; }
        public  string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public  int toDoListId { get; set; }
    }
}

namespace ToDoList.Api.Dtos
{
    public class RegisterResponseDto
    {
        public required string Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Token { get; set; }
        public required int UserToDoListId { get; set; }
    }
}

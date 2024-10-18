namespace ToDoList.Api.Dtos
{
    public class LoginResponseDto
    {
        public required string Id { get; set; }
        public required string Token { get; set; }
        public required string UserName { get;set; }
        public required int UserToDoListId { get;set; }
    }
}

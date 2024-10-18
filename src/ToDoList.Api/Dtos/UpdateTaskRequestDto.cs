﻿namespace ToDoList.Api.Dtos
{
    public class UpdateTaskRequestDto
    {
        public required string Description { get; set; }
        public required string Name { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

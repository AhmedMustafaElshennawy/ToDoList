using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Models;

namespace ToDoList.Core.Services
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(ApplicationUser user);
        public Task<string> GenerateToken(ApplicationUser user, int toDoListId);
    }
}
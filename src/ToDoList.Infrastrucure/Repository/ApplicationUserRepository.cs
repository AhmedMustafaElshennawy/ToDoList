using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Models;
using ToDoList.Core.Repository;
using ToDoList.Infrastrucure.Context;

namespace ToDoList.Infrastrucure.Repository
{
    public class ApplicationUserRepository : BaseRepository<ApplicationUser>,IApplicationUserRepository
    {
        private readonly ApplicationDbContext _context;
        public ApplicationUserRepository(ApplicationDbContext context) :base(context)
        {
            _context = context;
        }

        public async Task<ApplicationUser> GetById(string id)
        {
            var user = await _context.Users.FindAsync(id);
            return user;
        }
    }
}

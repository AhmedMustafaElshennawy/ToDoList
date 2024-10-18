using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Models;
using ToDoList.Core.Repository;
using ToDoList.Core.UnitOfWork;
using ToDoList.Infrastrucure.Context;
using ToDoList.Infrastrucure.Repository;

namespace ToDoList.Infrastrucure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IBaseRepository<task> Tasks { get; set; }
        public IBaseRepository<ToDolist> ToDolist { get; set; }
        public IApplicationUserRepository Users { get; set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            Tasks = new BaseRepository<task>(context);
            ToDolist = new BaseRepository<ToDolist>(context);
            Users = new ApplicationUserRepository(context);
            _context = context;
        }
        public int Complete() => _context.SaveChanges();
        public Task<int> CompleteAsync() => _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }
}

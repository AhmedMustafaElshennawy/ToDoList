using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Models;
using ToDoList.Core.Repository;

namespace ToDoList.Core.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public IBaseRepository<task> Tasks { get; }
        public IBaseRepository<ToDolist> ToDolist { get; }
        public IApplicationUserRepository Users { get; }
        Task<int> CompleteAsync();
        int Complete();
    }
}

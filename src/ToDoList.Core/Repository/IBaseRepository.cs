using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Core.Repository
{
    public interface IBaseRepository<T>where T : class
    {
        Task<T> GetEntityByIdAsync(int id); 
        Task<T> CreateEntityAsync(T Entity);
        Task DeleteEntityAsync(T Entity);
        Task<T> UpdateEntityAsync(T Entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> FindAnyAsync(Expression<Func<T, bool>> Match, string[] Includes = null);
        Task<T> FindByLamdaAsync(Expression<Func<T, bool>> match);
        IQueryable<T> Entities();

    }
}

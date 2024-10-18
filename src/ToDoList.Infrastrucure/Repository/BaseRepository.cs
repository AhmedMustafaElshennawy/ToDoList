using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ToDoList.Core.Repository;
using ToDoList.Infrastrucure.Context;

namespace ToDoList.Infrastrucure.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<T> CreateEntityAsync(T Entity)
        {
            await _context.Set<T>().AddAsync(Entity);
            return Entity;
        }

        public Task DeleteEntityAsync(T Entity)
        {
            _context.Set<T>().Remove(Entity);
            return Task.CompletedTask;
        }

        public IQueryable<T> Entities() => _context.Set<T>();

        public async Task<T> FindAnyAsync(Expression<Func<T, bool>> Match, string[] Includes = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (Includes!= null)
            {
                foreach (var include in Includes)
                {
                    query = query.Include(include);
                }
            }
            var result = await query.SingleOrDefaultAsync(Match);
            return result;
        }

        public async Task<T> FindByLamdaAsync(Expression<Func<T, bool>> match)
        {
            var result = await _context.Set<T>().SingleOrDefaultAsync(match);
            return result;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var result = await _context.Set<T>().ToListAsync();
            return result;
        }

        public async Task<T> GetEntityByIdAsync(int id)
        {
            var result = await _context.Set<T>().FindAsync(id);
            return result;
        }

        public Task<T> UpdateEntityAsync(T Entity)
        {
            _context.Set<T>().Update(Entity);
            return Task.FromResult(Entity);
        }
    }
}
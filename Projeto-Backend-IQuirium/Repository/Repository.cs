using Microsoft.EntityFrameworkCore;
using Projeto_Backend_IQuirium.Interfaces;
using System.Linq.Expressions;

namespace Projeto_Backend_IQuirium.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext Context;
        protected readonly DbSet<T> DbSet;

        public Repository(DbContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
        }

        public virtual void Update(T entity)
        {
            DbSet.Update(entity);
        }

        public virtual void Delete(T entity)
        {
            DbSet.Remove(entity);
        }
    }
}
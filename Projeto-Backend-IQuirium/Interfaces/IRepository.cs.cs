using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Projeto_Backend_IQuirium.Interfaces;

namespace Projeto_Backend_IQuirium.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}


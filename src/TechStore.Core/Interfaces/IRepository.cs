using System.Linq.Expressions;

namespace TechStore.Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        //Métodos de leitura
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        //Métodos de escrita
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool>SaveChangesAsync();

        //Métodos específicos
        Task<bool>ExistsAsync(int id);
    }
}

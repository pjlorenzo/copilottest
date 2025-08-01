using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    Task<PagedList<T>> GetAllAsync(PaginationParameters parameters);
    Task<T?> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}

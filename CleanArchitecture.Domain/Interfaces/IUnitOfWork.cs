namespace CleanArchitecture.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<TEntity> Repository<TEntity>() where TEntity : class;
    IProductRepository ProductRepository { get; }
    Task<int> SaveChangesAsync();
}

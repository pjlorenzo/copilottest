using CleanArchitecture.Domain.Entities;

using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Application.Interfaces;

public interface IProductService
{
    Task<PagedList<Product>> GetAllProductsAsync(PaginationParameters parameters);
    Task<Product?> GetProductByIdAsync(int id);
    Task<Product> CreateProductAsync(Product product);
    Task UpdateProductAsync(Product product);
    Task DeleteProductAsync(int id);
}

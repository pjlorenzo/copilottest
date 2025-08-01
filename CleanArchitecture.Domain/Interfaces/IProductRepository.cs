using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Dtos;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Domain.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<PagedList<ProductDto>> GetAllWithCategoryAsync(PaginationParameters parameters);
}

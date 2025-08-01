using CleanArchitecture.Domain.Dtos;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Data;

public class ProductRepository : Repository<Product>, IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<PagedList<ProductDto>> GetAllWithCategoryAsync(PaginationParameters parameters)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .OrderBy(p => p.Name)  // Default ordering by product name
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                CategoryDescription = p.Category.Description
            });

        var count = await query.CountAsync();
        var items = await query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        return new PagedList<ProductDto>(items, count, parameters.PageNumber, parameters.PageSize);
    }
}

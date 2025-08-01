using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;

namespace CleanArchitecture.Application.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Retrieves all products, paged.
    /// </summary>
    /// <param name="parameters">The pagination parameters.</param>
    /// <returns>A paged list of products.</returns>
    public async Task<PagedList<Product>> GetAllProductsAsync(PaginationParameters parameters)
    {
        return await _unitOfWork.Repository<Product>().GetAllAsync(parameters);
    }

    /// <summary>
    /// Retrieves a product by its id.
    /// </summary>
    /// <param name="id">The id of the product to retrieve.</param>
    /// <returns>The product with the given id, or <c>null</c> if no such product exists.</returns>
    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _unitOfWork.Repository<Product>().GetByIdAsync(id);
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        await _unitOfWork.Repository<Product>().AddAsync(product);
        await _unitOfWork.SaveChangesAsync();
        return product;
    }

    public async Task UpdateProductAsync(Product product)
    {
        await _unitOfWork.Repository<Product>().UpdateAsync(product);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
        if (product != null)
        {
            await _unitOfWork.Repository<Product>().DeleteAsync(product);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

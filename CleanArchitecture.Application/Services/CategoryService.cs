using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;

namespace CleanArchitecture.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedList<Category>> GetAllCategoriesAsync(PaginationParameters parameters)
    {
        return await _unitOfWork.Repository<Category>().GetAllAsync(parameters);
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        return await _unitOfWork.Repository<Category>().GetByIdAsync(id);
    }

    public async Task<Category> CreateCategoryAsync(Category category)
    {
        await _unitOfWork.Repository<Category>().AddAsync(category);
        await _unitOfWork.SaveChangesAsync();
        return category;
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        await _unitOfWork.Repository<Category>().UpdateAsync(category);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
        if (category != null)
        {
            await _unitOfWork.Repository<Category>().DeleteAsync(category);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

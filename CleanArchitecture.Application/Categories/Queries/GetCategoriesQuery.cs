using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Categories.Queries;

public record GetCategoriesQuery(PaginationParameters Parameters) : IRequest<PagedList<Category>>;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, PagedList<Category>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCategoriesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedList<Category>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository<Category>().GetAllAsync(request.Parameters);
    }
}

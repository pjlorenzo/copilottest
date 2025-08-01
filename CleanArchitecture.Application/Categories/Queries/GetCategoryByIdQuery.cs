using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Categories.Queries;

public record GetCategoryByIdQuery(int Id) : IRequest<Category?>;

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Category?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Category?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository<Category>().GetByIdAsync(request.Id);
    }
}

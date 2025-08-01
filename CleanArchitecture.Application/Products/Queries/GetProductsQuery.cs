using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Dtos;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Products.Queries;

public record GetProductsQuery(PaginationParameters Parameters) : IRequest<PagedList<ProductDto>>;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PagedList<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProductsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedList<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.ProductRepository.GetAllWithCategoryAsync(request.Parameters);
    }
}

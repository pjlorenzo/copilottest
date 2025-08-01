using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Products.Queries;

public record GetProductByIdQuery(int Id) : IRequest<Product?>;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProductByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Product?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Repository<Product>().GetByIdAsync(request.Id);
    }
}

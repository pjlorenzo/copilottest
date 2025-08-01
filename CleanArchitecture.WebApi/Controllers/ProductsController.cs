using CleanArchitecture.Application.Products.Commands;
using CleanArchitecture.Application.Products.Queries;
using CleanArchitecture.Domain.Dtos;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.WebApi.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CleanArchitecture.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [EnableRateLimiting(RateLimitingConstants.GetPolicy)]
    public async Task<ActionResult<PagedList<ProductDto>>> GetProducts([FromQuery] PaginationParameters parameters)
    {
        var query = new GetProductsQuery(parameters);
        var products = await _mediator.Send(query);
        return Ok(products);
    }

    [HttpGet("{id}")]
    [EnableRateLimiting(RateLimitingConstants.GetPolicy)]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var query = new GetProductByIdQuery(id);
        var product = await _mediator.Send(query);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    [EnableRateLimiting(RateLimitingConstants.WritePolicy)]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] CreateProductCommand command)
    {
        var product = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    [EnableRateLimiting(RateLimitingConstants.WritePolicy)]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [EnableRateLimiting(RateLimitingConstants.WritePolicy)]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await _mediator.Send(new DeleteProductCommand(id));
        return NoContent();
    }
}

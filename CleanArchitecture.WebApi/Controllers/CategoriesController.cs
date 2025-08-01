using CleanArchitecture.Application.Categories.Commands;
using CleanArchitecture.Application.Categories.Queries;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.WebApi.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CleanArchitecture.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [EnableRateLimiting(RateLimitingConstants.GetPolicy)]
    public async Task<ActionResult<PagedList<Category>>> GetCategories([FromQuery] PaginationParameters parameters)
    {
        var query = new GetCategoriesQuery(parameters);
        var categories = await _mediator.Send(query);
        return Ok(categories);
    }

    [HttpGet("{id}")]
    [EnableRateLimiting(RateLimitingConstants.GetPolicy)]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        var query = new GetCategoryByIdQuery(id);
        var category = await _mediator.Send(query);
        if (category == null)
        {
            return NotFound();
        }
        return Ok(category);
    }

    [HttpPost]
    [EnableRateLimiting(RateLimitingConstants.WritePolicy)]
    public async Task<ActionResult<Category>> CreateCategory([FromBody] CreateCategoryCommand command)
    {
        var category = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
    }

    [HttpPut("{id}")]
    [EnableRateLimiting(RateLimitingConstants.WritePolicy)]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryCommand command)
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
    public async Task<IActionResult> DeleteCategory(int id)
    {
        await _mediator.Send(new DeleteCategoryCommand(id));
        return NoContent();
    }
}

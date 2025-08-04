using CleanArchitecture.Application.Categories.Commands;
using CleanArchitecture.Application.Categories.Queries;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.WebApi.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Caching.Memory;

namespace CleanArchitecture.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMemoryCache _cache;

    public CategoriesController(IMediator mediator, IMemoryCache cache)
    {
        _mediator = mediator;
        _cache = cache;
    }

    [HttpGet]
    [EnableRateLimiting(RateLimitingConstants.GetPolicy)]

    public async Task<ActionResult<PagedList<Category>>> GetCategories([FromQuery] PaginationParameters parameters)
    {
        // Build a cache key based on pagination parameters
        var cacheKey = $"categories_page_{parameters.PageNumber}_size_{parameters.PageSize}";
        if (!_cache.TryGetValue(cacheKey, out PagedList<Category>? categories))
        {
            var query = new GetCategoriesQuery(parameters);
            categories = await _mediator.Send(query);

            // Cache for 60 seconds
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
            _cache.Set(cacheKey, categories, cacheEntryOptions);
        }
        return Ok(categories);
    }

    [HttpGet("{id}")]
    [EnableRateLimiting(RateLimitingConstants.GetPolicy)]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        var cacheKey = $"category_{id}";
        if (!_cache.TryGetValue(cacheKey, out Category? category))
        {
            var query = new GetCategoryByIdQuery(id);
            category = await _mediator.Send(query);
            if (category == null)
            {
                return NotFound();
            }
            // Cache for 60 seconds
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
            _cache.Set(cacheKey, category, cacheEntryOptions);
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

        // Invalidate category cache for this id and all paged lists
        _cache.Remove($"category_{id}");
        RemoveCategoriesPagedCache();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [EnableRateLimiting(RateLimitingConstants.WritePolicy)]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        await _mediator.Send(new DeleteCategoryCommand(id));

        // Invalidate category cache for this id and all paged lists
        _cache.Remove($"category_{id}");
        RemoveCategoriesPagedCache();

        return NoContent();
    }
    /// <summary>
    /// Removes all cached paged categories lists.
    /// </summary>
    private void RemoveCategoriesPagedCache()
    {
        // IMemoryCache does not provide a way to enumerate keys, so this is a simple pattern for small apps.
        // For production, consider using a distributed cache with key prefix support.
        // Here, we use a known set of page numbers and sizes for demonstration.
        for (int page = 1; page <= 10; page++)
        {
            for (int size = 1; size <= 50; size++)
            {
                var key = $"categories_page_{page}_size_{size}";
                _cache.Remove(key);
            }
        }
    }
}

using DistributedCaching.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Entities;
using Shared.Services.Interfaces;

namespace DistributedCaching.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly IDistributedCache _distributedCache;

    public PostsController(IPostService postService, IDistributedCache distributedCache)
    {
        _postService = postService;
        _distributedCache = distributedCache;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(string id)
    {
        var cacheKey = $"Post-{id}";
        if (!_distributedCache.TryGetValue<Post>(cacheKey, out Post? post))
        {
            post = await _postService.GetAsync(id);

            if(post is null) return NotFound();

            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(10))
                .SetSlidingExpiration(TimeSpan.FromSeconds(5));

            await _distributedCache.SetAsync<Post>(cacheKey, post, options);
        }

        return Ok(post);
    }
}

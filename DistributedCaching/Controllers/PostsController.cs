using DistributedCaching.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Models;
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
        if (_distributedCache.TryGetValue<Post>(cacheKey, out Post? post))
        {
            post = await _postService.GetAsync(id);

            if(post is null) return NotFound();

            await _distributedCache.SetAsync<Post>(cacheKey, post, new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(10),
                SlidingExpiration = TimeSpan.FromSeconds(5),
            });
        }

        return Ok(post);
    }
}

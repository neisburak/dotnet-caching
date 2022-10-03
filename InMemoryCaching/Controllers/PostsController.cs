using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Shared.Models;
using Shared.Services.Interfaces;

namespace InMemoryCaching.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<PostsController> _logger;

    public PostsController(IPostService postService, IMemoryCache memoryCache, ILogger<PostsController> logger)
    {
        _postService = postService;
        _memoryCache = memoryCache;
        _logger = logger;
    }

    [HttpGet("get/{key}")]
    public IActionResult Get(string key)
    {
        var value = _memoryCache.Get<string>(key);

        if(value is not null) return Ok(value);
        return NotFound();
    }

    [HttpPost("set/{key}")]
    public IActionResult Set(string key, string value)
    {
        return Ok(_memoryCache.Set(key, value));
    }

    [HttpDelete("delete/{key}")]
    public IActionResult Delete(string key)
    {
        _memoryCache.Remove(key);
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(string id)
    {
        var cacheKey = $"Post-{id}";
        if (!_memoryCache.TryGetValue<Post>(cacheKey, out Post? post))
        {
            post = await _postService.GetAsync(id);

            if (post is null) return NotFound();

            _memoryCache.Set<Post>(cacheKey, post, new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(10),
                SlidingExpiration = TimeSpan.FromSeconds(5),
                Priority = CacheItemPriority.High,
                PostEvictionCallbacks = 
                {
                    {
                        new PostEvictionCallbackRegistration
                        {
                            EvictionCallback = (key, value, reason, state) => 
                                _logger.LogInformation($"Cache with key: {key} is evicted.")
                        } 
                    }
                }
            });
        }

        return Ok(post);
    }
}

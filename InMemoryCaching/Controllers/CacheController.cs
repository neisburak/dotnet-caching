using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCaching.Controllers;

[ApiController]
[Route("[controller]")]
public class CacheController : ControllerBase
{
    private readonly IMemoryCache _memoryCache;

    public CacheController(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    [HttpGet("{key}")]
    public IActionResult Get(string key)
    {
        return Ok(_memoryCache.Get<string>(key));
    }

    [HttpGet("{key}/try")]
    public IActionResult TryGet(string key)
    {
        if (_memoryCache.TryGetValue<string>(key, out string value))
        {
            return Ok(value);
        }
        return NotFound();
    }

    [HttpGet("{key}/create/{value}")]
    public IActionResult GetOrCreate(string key, string value)
    {
        var val = _memoryCache.GetOrCreate<string>(key, factory =>
        {
            factory.AbsoluteExpiration = DateTime.Now.AddSeconds(10);
            factory.SlidingExpiration = TimeSpan.FromSeconds(5);
            factory.Priority = CacheItemPriority.High;

            return value;
        });
        return Ok(val);
    }

    [HttpPost("{key}")]
    public IActionResult Set(string key, string value)
    {
        if (value is null) return BadRequest();

        return Ok(_memoryCache.Set<string>(key, value));
    }

    [HttpDelete("{key}")]
    public IActionResult Delete(string key)
    {
        _memoryCache.Remove(key);
        return Ok();
    }
}

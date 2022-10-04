using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using DistributedCaching.Extensions;

namespace InMemoryCaching.Controllers;

[ApiController]
[Route("[controller]")]
public class CacheController : ControllerBase
{
    private readonly IDistributedCache _distributedCache;

    public CacheController(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    [HttpGet("{key}")]
    public IActionResult Get(string key)
    {
        return Ok(_distributedCache.GetStringAsync(key));
    }

    [HttpGet("{key}/try")]
    public IActionResult TryGet(string key)
    {
        if (_distributedCache.TryGetValue<string>(key, out string? value))
        {
            return Ok(value);
        }
        return NotFound();
    }

    [HttpPost("{key}")]
    public IActionResult Set(string key, string value)
    {
        if (value is null) return BadRequest();

        return Ok(_distributedCache.SetAsync<string>(key, value));
    }

    [HttpDelete("{key}")]
    public IActionResult Delete(string key)
    {
        _distributedCache.Remove(key);
        return Ok();
    }
}

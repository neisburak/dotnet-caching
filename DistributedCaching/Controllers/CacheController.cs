using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

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
    public async Task<IActionResult> GetAsync(string key)
    {
        return Ok(await _distributedCache.GetStringAsync(key));
    }

    [HttpPost("{key}")]
    public async Task<IActionResult> SetAsync(string key, string value)
    {
        if (value is null) return BadRequest();

        await _distributedCache.SetStringAsync(key, value);

        return Ok();
    }

    [HttpDelete("{key}")]
    public IActionResult Delete(string key)
    {
        _distributedCache.Remove(key);
        return Ok();
    }

    [HttpGet("file")]
    public async Task<IActionResult> File()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "image.jpg");
        var byteImage = await System.IO.File.ReadAllBytesAsync(path);
        await _distributedCache.SetAsync("image", byteImage);

        var bytes = await _distributedCache.GetAsync("image");
        return File(bytes, "image/jpg");
    }
}

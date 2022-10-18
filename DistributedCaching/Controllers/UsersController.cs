using Microsoft.AspNetCore.Mvc;
using Shared.Services.Interfaces;

namespace DistributedCaching.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(string id)
    {
        return Ok(await _userService.GetAsync(id));
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        return Ok(await _userService.GetAsync());
    }
}
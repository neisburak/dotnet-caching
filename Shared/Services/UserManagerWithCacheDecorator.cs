using System.Text.Json;
using Shared.Entities;
using Shared.Services.Interfaces;
using Shared.Utilities;
using StackExchange.Redis;

namespace Shared.Services;

public class UserManagerWithCacheDecorator : IUserService
{
    private const string CACHE_KEY = "usersCache";

    private readonly IUserService _userService;
    private readonly RedisService _redisService;
    private readonly IDatabase _redisDatabase;

    public UserManagerWithCacheDecorator(IUserService userService, RedisService redisService)
    {
        _userService = userService;
        _redisService = redisService;
        _redisDatabase = _redisService.GetDatabase();
    }

    public async Task<User?> GetAsync(string id)
    {
        if (await _redisDatabase.KeyExistsAsync(CACHE_KEY) && await _redisDatabase.HashExistsAsync(CACHE_KEY, id))
        {
            return await GetCacheAsync(id);
        }

        var user = await _userService.GetAsync(id);
        if (user is not null) await SetCacheAsync(user);
        return user;
    }

    public async Task<IEnumerable<User>?> GetAsync()
    {
        if (await _redisDatabase.KeyExistsAsync(CACHE_KEY))
        {
            return await GetCacheAsync();
        }

        var users = await _userService.GetAsync();
        if (users is not null) await SetCacheAsync(users);
        return users;
    }

    #region Helper Methods
    private async Task SetCacheAsync(User user)
    {
        await _redisDatabase.HashSetAsync(CACHE_KEY, user.Id, JsonSerializer.Serialize(user));
    }
    private async Task SetCacheAsync(IEnumerable<User> users)
    {
        foreach (var user in users)
        {
            await _redisDatabase.HashSetAsync(CACHE_KEY, user.Id, JsonSerializer.Serialize(user));
        }
    }
    private async Task<User?> GetCacheAsync(string id)
    {
        var cache = await _redisDatabase.HashGetAsync(CACHE_KEY, id);
        if (cache.HasValue)
        {
            return JsonSerializer.Deserialize<User>(cache!);
        }
        return null;
    }
    private async Task<IEnumerable<User>?> GetCacheAsync()
    {
        var users = new List<User>();
        var cachedUsers = await _redisDatabase.HashGetAllAsync(CACHE_KEY);
        foreach (var item in cachedUsers.ToList())
        {
            var user = JsonSerializer.Deserialize<User>(item.Value!);
            if (user is not null) users.Add(user);
        }
        return users;
    }
    #endregion
}
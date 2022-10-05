using DistributedCaching.Services;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace DistributedCaching.Controllers;

[ApiController]
[Route("[controller]")]
public class RedisController : ControllerBase
{
    private readonly RedisService _redisService;
    private readonly IDatabase _redisDatabase;

    public RedisController(RedisService redisService)
    {
        _redisService = redisService;
        _redisDatabase = _redisService.GetDatabase();
    }

    [HttpGet("String")]
    public IActionResult GetString()
    {
        _redisDatabase.StringSet("name", "Jessica");
        _redisDatabase.StringSet("visitors", 100);

        var name = _redisDatabase.StringGet("name");
        var nameRange = _redisDatabase.StringGetRange("name", 0, 2);
        var length = _redisDatabase.StringLength("name");

        var incrementValue = _redisDatabase.StringIncrement("visitors", 10);
        var decrementValue = _redisDatabase.StringDecrement("visitors");

        return Ok(new
        {
            Name = name.ToString(),
            NameRange = nameRange.ToString(),
            Length = length,
            IncrementValue = incrementValue,
            DecrementValue = decrementValue
        });
    }

    [HttpGet("LinkedList")]
    public IActionResult GetLinkedList()
    {
        _redisDatabase.ListRightPush("names_list", "Jessica");
        _redisDatabase.ListLeftPush("names_list", "Joey");
        _redisDatabase.ListLeftPush("names_list", "Jennifer");

        var names = new List<string>();
        if (_redisDatabase.KeyExists("names_list"))
        {
            _redisDatabase.ListRange("names_list").ToList().ForEach(f => names.Add(f));
        }

        _redisDatabase.ListRemove("names_list", "Jennifer");
        _redisDatabase.ListRightPop("names_list");

        return Ok(names);
    }

    [HttpGet("Set")]
    public IActionResult GetSet()
    {
        if (_redisDatabase.KeyExists("names_set"))
        {
            _redisDatabase.KeyExpire("names_set", DateTime.Now.AddSeconds(30));
        }

        _redisDatabase.SetAdd("names_set", "Olivia");
        _redisDatabase.SetAdd("names_set", "Amelia");

        var names = new HashSet<string>();
        if (_redisDatabase.KeyExists("names_set"))
        {
            _redisDatabase.SetMembers("names_set").ToList().ForEach(f => names.Add(f));
        }

        _redisDatabase.SetRemove("names_set", "Amelia");
        var poppedItem = _redisDatabase.SetPop("names_set");

        return Ok(names);
    }

    [HttpGet("SortedSet")]
    public IActionResult GetSortedSet()
    {
        if (_redisDatabase.KeyExists("names_sortedset"))
        {
            _redisDatabase.KeyExpire("names_sortedset", DateTime.Now.AddSeconds(30));
        }

        _redisDatabase.SortedSetAdd("names_sortedset", "Ava", 2);
        _redisDatabase.SortedSetAdd("names_sortedset", "Charlotte", 1);

        var names = new HashSet<string>();
        var namesReserved = new HashSet<string>();
        if (_redisDatabase.KeyExists("names_sortedset"))
        {
            _redisDatabase.SortedSetScan("names_sortedset").ToList().ForEach(f => names.Add(f.Element));
            _redisDatabase.SortedSetRangeByRank("names_sortedset", order: Order.Descending).ToList().ForEach(f => namesReserved.Add(f));
        }
        _redisDatabase.SortedSetRemove("names_sortedset", "Ava");

        return Ok(new
        {
            Names = names,
            Reserved = namesReserved
        });
    }

    [HttpGet("Hash")]
    public IActionResult GetHash()
    {
        _redisDatabase.HashSet("hash_set", new HashEntry[] { new("names_hash", "Sophia"), new("names_hash", "Isabella") });

        var names = new Dictionary<string, string>();
        if (_redisDatabase.KeyExists("hash_set"))
        {
            _redisDatabase.HashGetAll("hash_set").ToList().ForEach(f => names.Add(f.Name, f.Value));
        }

        var hash = _redisDatabase.HashGet("hash_set", "names_hash");
        _redisDatabase.HashDelete("hash_set", "names_hash");

        return Ok(names);
    }
}

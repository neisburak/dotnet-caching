using Shared.Extensions;
using Shared.Entities;
using Shared.Services.Interfaces;

namespace Shared.Services;

public class UserManager : IUserService
{
    private const string _url = "https://jsonplaceholder.typicode.com/users/";

    public async Task<User?> GetAsync(string id) => await _url.GetAsync<User>(id);

    public async Task<IEnumerable<User>?> GetAsync() => await _url.GetAsync<IEnumerable<User>>();
}
using Shared.Extensions;
using Shared.Entities;
using Shared.Services.Interfaces;

namespace Shared.Services;

public class PostManager : IPostService
{
    private const string _url = "https://jsonplaceholder.typicode.com/posts/";

    public async Task<Post?> GetAsync(string id) => await _url.GetAsync<Post>(id);

    public async Task<IEnumerable<Post>?> GetAsync() => await _url.GetAsync<IEnumerable<Post>>();
}
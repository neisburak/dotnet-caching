using Shared.Entities;

namespace Shared.Services.Interfaces;

public interface IPostService
{
    Task<Post?> GetAsync(string id);
    Task<IEnumerable<Post>?> GetAsync();
}
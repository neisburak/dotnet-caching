using Shared.Entities;

namespace Shared.Services.Interfaces;

public interface IUserService
{
    Task<User?> GetAsync(string id);
    Task<IEnumerable<User>?> GetAsync();
}
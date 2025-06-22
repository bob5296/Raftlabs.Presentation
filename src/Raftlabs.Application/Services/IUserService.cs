using Raftlabs.Core;
using Raftlabs.Library.Interfaces;

namespace Raftlabs.Application.Services;

public interface IUserService
{
    Task<User> GetUserByIdAsync(int userId, CancellationToken cancellationToken);

    Task<IEnumerable<User>> GetAllUsersAsync(int pageNumber, CancellationToken cancellationToken);
}

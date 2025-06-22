using System.Threading;
using System.Threading.Tasks;

namespace Raftlabs.Application.Services;

public interface IJsonService
{
    Task<T?> DeserializeAsync<T>(HttpContent content, CancellationToken cancellationToken);
}

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using Raftlabs.Library.Exceptions;

namespace Raftlabs.Infra.UserApiClient.Client;

public class UserApi(IApiClient client) : IUserApiClient
{
    public async Task<HttpResponseMessage> GetAllUsersAsync(int pageNumber, CancellationToken cancellationToken)
    {
        var route = $"api/users?page={pageNumber}";
        try
        {
            return await client.GetAsync("UserApi", route, cancellationToken);
        }
        catch (GatewayTimeoutException ex)
        {
            throw new GatewayTimeoutException($"Request timed out while retrieving users for page {pageNumber}.", ex);
        }
    }

    public async Task<HttpResponseMessage> GetUserByIdAsync(int id, CancellationToken cancellationToken)
    {
        var route = $"api/users/{id}";
        try
        {
            return await client.GetAsync("UserApi", route, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            throw new GatewayTimeoutException($"Request timed out while retrieving user with ID {id}.", ex);
        }
    }
}

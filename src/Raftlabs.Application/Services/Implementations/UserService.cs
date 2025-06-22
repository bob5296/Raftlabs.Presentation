using System.Net;
using Application.Abstractions;
using Application.Response;
using AutoMapper;
using Raftlabs.Core;
using Raftlabs.Library.Caching.Services;
using Raftlabs.Library.Exceptions;
using Raftlabs.Library.Interfaces;

namespace Raftlabs.Application.Services.Implementations;

public partial class UserService : IUserService, IApplicationService
{
    private readonly IUserApiClient _httpClient;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;
    private readonly IJsonService _jsonService;

    public UserService(IUserApiClient httpClient, IMapper mapper, ICacheService cacheService, IJsonService jsonService)
    {
        _httpClient = httpClient;
        _mapper = mapper;
        _cacheService = cacheService;
        _jsonService = jsonService;
    }

    public async Task<User> GetUserByIdAsync(int userId, CancellationToken cancellationToken)
    {
        string cacheKey = $"user:{userId}";

        return await _cacheService.GetOrSetAsync(
            key: cacheKey,
            factory: async () =>
            {
                var response = await _httpClient.GetUserByIdAsync(userId, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                        throw new NotFoundException($"User with ID {userId} was not found.");

                    response.EnsureSuccessStatusCode();
                }

                var userResponse = await _jsonService.DeserializeAsync<UserResponse>(response.Content, cancellationToken);

                if (userResponse?.Data is null)
                    throw new NotFoundException($"User with ID {userId} was not found or response was invalid.");

                return _mapper.Map<User>(userResponse.Data);
            },
            absoluteExpiration: TimeSpan.FromMinutes(10),
            cancellationToken: cancellationToken
        );
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync(int pageNumber, CancellationToken cancellationToken)
    {
        string cacheKey = $"users:page:{pageNumber}";

        return await _cacheService.GetOrSetAsync(
            key: cacheKey,
            factory: async () =>
            {
                var response = await _httpClient.GetAllUsersAsync(pageNumber, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                        throw new NotFoundException($"No users found for page {pageNumber}.");

                    response.EnsureSuccessStatusCode();
                }

                var usersResponse = await _jsonService.DeserializeAsync<UsersResponse>(response.Content, cancellationToken);
                var users = usersResponse?.Data;

                if (users is null || !users.Any())
                    throw new NotFoundException($"No users found for page {pageNumber}.");

                return _mapper.Map<IEnumerable<User>>(users);
            },
            absoluteExpiration: TimeSpan.FromMinutes(10),
            cancellationToken: cancellationToken
        );
    }
}

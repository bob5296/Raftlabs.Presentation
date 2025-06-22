using System.Net;
using Application.Abstractions;
using Application.Response;
using AutoMapper;
using Moq;
using Raftlabs.Application.Services;
using Raftlabs.Application.Services.Implementations;
using Raftlabs.Core;
using Raftlabs.Library.Caching.Services;
using Raftlabs.Library.Exceptions;

namespace Raftlabs.Application.Tests;
public class UserServiceTests
{
    private readonly Mock<IUserApiClient> _mockApiClient = new();
    private readonly Mock<IMapper> _mockMapper = new();
    private readonly Mock<ICacheService> _mockCacheService = new();
    private readonly Mock<IJsonService> _mockJsonService = new();

    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userService = new UserService(
            _mockApiClient.Object,
            _mockMapper.Object,
            _mockCacheService.Object,
            _mockJsonService.Object);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        var userId = 1;
        var user = new User { Id = userId, First_Name = "John" };
        var userResponse = new UserResponse { Data = user };
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{}")
        };

        _mockApiClient.Setup(x => x.GetUserByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(httpResponse);

        _mockJsonService.Setup(x => x.DeserializeAsync<UserResponse>(httpResponse.Content, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userResponse);

        _mockMapper.Setup(x => x.Map<User>(user)).Returns(user);

        _mockCacheService.Setup(x => x.GetOrSetAsync(
            It.IsAny<string>(),
            It.IsAny<Func<Task<User>>>(),
            It.IsAny<TimeSpan>(),
            It.IsAny<CancellationToken>()
        )).Returns((string key, Func<Task<User>> factory, TimeSpan _, CancellationToken ct) => factory());

        var result = await _userService.GetUserByIdAsync(userId, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldThrowNotFoundException_WhenUserIsNotFound()
    {
        var userId = 999;
        var httpResponse = new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            Content = new StringContent("{}")
        };

        _mockApiClient.Setup(x => x.GetUserByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(httpResponse);

        _mockJsonService.Setup(x => x.DeserializeAsync<UserResponse>(httpResponse.Content, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserResponse { Data = new User() });

        _mockCacheService.Setup(x => x.GetOrSetAsync(
            It.IsAny<string>(),
            It.IsAny<Func<Task<User>>>(),
            It.IsAny<TimeSpan>(),
            It.IsAny<CancellationToken>()
        )).Returns((string key, Func<Task<User>> factory, TimeSpan _, CancellationToken ct) => factory());

        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _userService.GetUserByIdAsync(userId, CancellationToken.None));
    }

    [Fact]
    public async Task GetAllUsersAsync_ShouldThrowNotFoundException_WhenUsersEmpty()
    {
        var page = 1;
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{}")
        };

        _mockApiClient.Setup(x => x.GetAllUsersAsync(page, It.IsAny<CancellationToken>()))
            .ReturnsAsync(httpResponse);

        _mockJsonService.Setup(x => x.DeserializeAsync<UsersResponse>(httpResponse.Content, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UsersResponse { Data = new List<User>() });

        _mockCacheService.Setup(x => x.GetOrSetAsync(
            It.IsAny<string>(),
            It.IsAny<Func<Task<IEnumerable<User>>>>(),
            It.IsAny<TimeSpan>(),
            It.IsAny<CancellationToken>()
        )).Returns((string key, Func<Task<IEnumerable<User>>> factory, TimeSpan _, CancellationToken ct) => factory());

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _userService.GetAllUsersAsync(page, CancellationToken.None));
    }
}

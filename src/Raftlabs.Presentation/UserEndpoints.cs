using Raftlabs.Application.Services;

namespace Raftlabs.Presentation;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        // add cqrs or medator
        var group = app.MapGroup("/users");

        group.MapGet("/{id:int}", async (
            int id,
            IUserService userService,
            CancellationToken cancellationToken) =>
        {
            var user = await userService.GetUserByIdAsync(id, cancellationToken);
            return Results.Ok(user); // NotFound handled in middleware
        });

        group.MapGet("", async (
            [AsParameters] UserQuery query,
            IUserService userService,
            CancellationToken cancellationToken) =>
        {
            var users = await userService.GetAllUsersAsync(query.PageNumber, cancellationToken);
            return Results.Ok(users);
        });

        return app;
    }

    public record UserQuery(int PageNumber);
}

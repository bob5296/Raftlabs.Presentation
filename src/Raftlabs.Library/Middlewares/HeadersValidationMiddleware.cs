using Microsoft.AspNetCore.Http;
using Raftlabs.Library.Services;

namespace Raftlabs.Library.Middlewares;

public class HeadersValidationMiddleware
{
    private readonly RequestDelegate _next;

    public HeadersValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, IWorkContext workContext)
    {
        if (workContext == null)
        {
            throw new ArgumentNullException(nameof(workContext));
        }

        workContext.ValidateItems();
        await _next(httpContext);
    }
}
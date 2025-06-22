using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Raftlabs.Library.Exceptions;
using Raftlabs.Library.Models;

namespace Raftlabs.Library.Middlewares;

public class UnhandledExceptionsMiddleware
{
    private readonly RequestDelegate _next;

    public UnhandledExceptionsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception e)
        {
            if (await ErrorMiddleware(httpContext, e))
            {
                return;
            }

            throw;
        }
    }

    internal static async Task<bool> ErrorMiddleware(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        if (await TryHandleException<GatewayTimeoutException>(context, exception, HttpStatusCode.BadRequest))
        {
            return true;
        }

        if (await TryHandleException<NotFoundException>(context, exception, HttpStatusCode.Conflict))
        {
            return true;
        }

        var exceptionId = Guid.NewGuid();

        var logger = context.RequestServices.GetService<ILogger<UnhandledExceptionsMiddleware>>();
        logger.LogError(exception, $"An unhandled exception has occurred - {exceptionId}: {exception.Message}.");

        await context.Response.WriteAsync(new ApiException
        {
            ExceptionId = exceptionId,
            Message = "Internal Server Error"
        }.ToJson());

        return true;
    }

    private static async Task<bool> TryHandleException<TException>(HttpContext context, Exception e, HttpStatusCode statusCode)
        where TException : RaftlabsException
    {
        return await TryHandleException<TException>(context, e, statusCode, e => new AppResponse(false, e.Message).ToJson());
    }

    private static async Task<bool> TryHandleException<TException>(HttpContext context, Exception e, HttpStatusCode statusCode, Func<TException, string> responseBuilder)
        where TException : RaftlabsException
    {
        var exception = FindException<TException>(e);
        if (exception == null)
        {
            return false;
        }

        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsync(responseBuilder(exception));

        var logger = context.RequestServices.GetService<ILogger<UnhandledExceptionsMiddleware>>();
        logger.LogInformation(exception.Message);

        return true;
    }

    private static TException FindException<TException>(Exception e)
        where TException : class
    {
        if (e is TException exception)
        {
            return exception;
        }

        return e.InnerException != null
            ? FindException<TException>(e.InnerException)
            : null;
    }
}

using Microsoft.AspNetCore.Http;
using Raftlabs.Library.Exceptions;
using Raftlabs.Library.Interfaces;

namespace Raftlabs.Library.Services.Implementations;

public class WorkContext(IHttpContextAccessor httpContextAccessor) : IWorkContext, IApplicationService
{
    internal const string API_KEY_HEADER = "x-api-key";

    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string GetUserEmail()
    {
        return GetHeader(API_KEY_HEADER);
    }

    public void ValidateItems()
    {
        if (ContainsUserEmail())
        {
            GetUserEmail();
        }
    }

    public bool ContainsUserEmail()
    {
        return HasHeaderValue(API_KEY_HEADER);
    }

    private bool HasHeaderValue(string key)
    {
        return _httpContextAccessor.HttpContext != null && !string.IsNullOrWhiteSpace(GetHeaderString(key));
    }

    private string GetHeader(string key)
    {
        var header = GetHeaderString(key);
        if (string.IsNullOrWhiteSpace(header))
        {
            throw new MissingDataException($"Header {key} not found in request. Make sure that correct headers are send as a part of request.");
        }

        return header;
    }

    private string? GetHeaderString(string key)
    {
        return _httpContextAccessor.HttpContext != null
               && _httpContextAccessor.HttpContext.Request.Headers.TryGetValue(key, out var value)
            ? value
            : (string)null;
    }
}
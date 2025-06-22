using System.Text.Json;
using Microsoft.Extensions.Logging;
using Raftlabs.Library.Exceptions;
using Raftlabs.Library.Interfaces;

namespace Raftlabs.Application.Services.Implementations;

public class JsonService : IJsonService, IApplicationService
{
    private readonly ILogger<JsonService> _logger;

    public JsonService(ILogger<JsonService> logger)
    {
        _logger = logger;
    }

    public async Task<T?> DeserializeAsync<T>(HttpContent content, CancellationToken cancellationToken)
    {
        try
        {
            var json = await content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize JSON to type {Type}", typeof(T).Name);
            throw new DeserializationException($"Deserialization failed for type {typeof(T).Name}.", ex);
        }
    }
}

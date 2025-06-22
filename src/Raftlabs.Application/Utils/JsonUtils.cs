using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Utils;

public static class JsonUtils
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static async Task<T?> DeserializeAsync<T>(HttpContent content, CancellationToken cancellationToken)
    {
        var json = await content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<T>(json, _options);
    }
}

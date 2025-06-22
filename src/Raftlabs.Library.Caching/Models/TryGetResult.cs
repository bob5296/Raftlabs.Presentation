namespace Raftlabs.Library.Caching.Models;

public record TryGetResult<T>(bool Found, T? Value);
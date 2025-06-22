using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Raftlabs.Library.Caching.Enums;
using Raftlabs.Library.Caching.Models;
using Raftlabs.Library.Caching.Services.Implementations;
using Raftlabs.Library.Extensions;
using Raftlabs.Library.Interfaces;

namespace Raftlabs.Library.Caching;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCaching(this IServiceCollection services)
    {
        services.AddLibraryServices(typeof(CacheService).Assembly);

        var serviceProvider = services.BuildServiceProvider();
        var cacheOptions = serviceProvider.GetRequiredService<IOptions<CacheOptions>>().Value;
        var memoryCacheExpirationScanFrequencyInMinutes = cacheOptions.MemoryCacheExpirationScanFrequencyInMinutes;
        services.AddMemoryCache(options => options.ExpirationScanFrequency = TimeSpan.FromMinutes(memoryCacheExpirationScanFrequencyInMinutes));

        if (cacheOptions.CacheType == CacheType.Distributed.ToString())
        {
            AddRedisCacheServices(services, serviceProvider);
        }
        else
        {
            services.AddDistributedMemoryCache(options => options.ExpirationScanFrequency = TimeSpan.FromMinutes(memoryCacheExpirationScanFrequencyInMinutes));
        }

        return services;
    }

    private static void AddRedisCacheServices(IServiceCollection services, ServiceProvider serviceProvider)
    {
        var redisConfiguration = serviceProvider.GetRequiredService<IOptions<RedisConfiguration>>().Value;
        services.AddStackExchangeRedisCache(s =>
        {
            s.ConfigurationOptions = redisConfiguration.ConfigurationOptions;
            s.InstanceName = string.Empty;
        });
        services.AddSingleton(redisConfiguration.ConfigurationOptions);
    }

    public static IServiceCollection AddCacheServices(this IServiceCollection services, Assembly assembly)
    {
        var serviceTypes = assembly.GetTypes()
            .Where(t => typeof(IApplicationService).IsAssignableFrom(t)
                        && t.IsClass
                        && !t.IsAbstract)
            .ToList();

        foreach (var implType in serviceTypes)
        {
            var serviceInterfaces = implType.GetInterfaces()
                .Where(i => i != typeof(IApplicationService));

            foreach (var serviceInterface in serviceInterfaces)
            {
                services.AddScoped(serviceInterface, implType);
            }
        }

        return services;
    }

}

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Raftlabs.Application.Mappings;
using Raftlabs.Application.Services.Implementations;
using Raftlabs.Library.Interfaces;

namespace Raftlabs.Application.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(UserMappingProfile));
        services.AddApplicationServices(typeof(UserService).Assembly);
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services, Assembly assembly)
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

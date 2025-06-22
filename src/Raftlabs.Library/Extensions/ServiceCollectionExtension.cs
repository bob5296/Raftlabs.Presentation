using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Raftlabs.Library.Interfaces;
using Raftlabs.Library.Services;
using Raftlabs.Library.Services.Implementations;

namespace Raftlabs.Library.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtension
{
    public static IServiceCollection AddLibrary(this IServiceCollection services)
    {
        services.AddScoped<IWorkContext, WorkContext>();
        services.AddLibraryServices(typeof(WorkContext).Assembly);

        return services;
    }

    public static IServiceCollection AddLibraryServices(this IServiceCollection services, Assembly assembly)
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

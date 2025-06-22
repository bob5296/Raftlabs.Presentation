using Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Raftlabs.Infra.Client.Clients;
using Raftlabs.Infra.Client.Models;
using Raftlabs.Infra.UserApiClient.Client;

namespace Raftlabs.Infra.Client.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApiClient(this IServiceCollection services)
    {
        services.AddScoped<IUserApiClient, UserApi>();
        services.AddScoped<IApiClient, ApiClient>();

        var serviceProvider = services.BuildServiceProvider();
        var apiClientConfig = serviceProvider.GetRequiredService<IOptions<ApiClientOptions>>().Value;

        services.AddHttpClient(apiClientConfig.Client.Key, client =>
        {
            client.BaseAddress = new Uri(apiClientConfig.Client.Url);
            foreach (var header in apiClientConfig.Client.DefaultHeaders)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        })
        .AddPolicyHandler(HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                    .WaitAndRetryAsync(apiClientConfig.RetryAttempts,
                        retryAttempt =>
                            TimeSpan.FromSeconds(Math.Pow(apiClientConfig.ExponentialBackoffMultipler, retryAttempt))));

        return services;
    }
}

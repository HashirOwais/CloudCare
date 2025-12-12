using CloudCare.Web.Handlers;
using CloudCare.Web.Services; // Adjust to your namespace
using Microsoft.Extensions.DependencyInjection; // Needed for IServiceCollection

namespace CloudCare.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IHttpClientBuilder AddApiClient<TService>(
        this IServiceCollection services, 
        string baseUrl) where TService : class
    {
        return services.AddHttpClient<TService>(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            })
            .AddHttpMessageHandler<CloudCareApiHandler>();
    }
}
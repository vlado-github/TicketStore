using Microsoft.Extensions.DependencyInjection;
using Caravan.Domain.SocialEventFeature.Queries;

namespace Caravan.Domain.DependencyInjection;

public static class DomainDependencies
{
    public static void AddDomain(this IServiceCollection services)
    {
        services.AddScoped<ISocialEventQuery, SocialEventQuery>();
    } 
}
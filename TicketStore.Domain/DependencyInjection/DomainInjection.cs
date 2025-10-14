using Microsoft.Extensions.DependencyInjection;
using TicketStore.Domain.SocialEventFeature.Queries;

namespace TicketStore.Domain.DependencyInjection;

public static class DomainInjection
{
    public static void AddDomain(this IServiceCollection services)
    {
        services.AddScoped<ISocialEventQuery, SocialEventQuery>();
    } 
}
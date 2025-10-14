using Marten.Events.Projections;

namespace TicketStore.Domain.SocialEventFeature.Schema.Projections;

public static class Registry
{
    public static void AddSocialEventProjections(this ProjectionOptions options)
    {
        //options.Add<>(ProjectionLifecycle.Inline);
    }
}
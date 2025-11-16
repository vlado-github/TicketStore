using JasperFx.Events.Projections;
using Marten.Events.Projections;

namespace Caravan.Domain.SocialEventFeature.Schema.Projections;

public static class Registry
{
    public static void AddSocialEventProjections(this ProjectionOptions options)
    {
        options.Add<SocialEventProfile>(ProjectionLifecycle.Inline);
    }
}
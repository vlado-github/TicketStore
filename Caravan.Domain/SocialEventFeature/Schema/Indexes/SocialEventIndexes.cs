using Marten;
using Caravan.Domain.SocialEventFeature.Schema.Aggregates;
using Caravan.Domain.SocialEventFeature.Schema.Projections;

namespace Caravan.Domain.SocialEventFeature.Schema.Indexes;

public static class SocialEventIndexes
{
    public static void AddSocialEventIndexes(this StoreOptions options)
    {
        options.Schema.For<SocialEvent>().Identity(x => x.Id);
        options.Schema.For<SocialEventProfileDetails>().Identity(x => x.Id);
    }
}
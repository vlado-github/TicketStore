using Marten;
using TicketStore.Domain.SocialEventFeature.Schema.Aggregates;
using TicketStore.Domain.SocialEventFeature.Schema.Projections;

namespace TicketStore.Domain.SocialEventFeature.Schema.Indexes;

public static class SocialEventIndexes
{
    public static void AddSocialEventIndexes(this StoreOptions options)
    {
        options.Schema.For<SocialEvent>().Identity(x => x.Id);
        options.Schema.For<SocialEventProfileDetails>().Identity(x => x.Id);
    }
}
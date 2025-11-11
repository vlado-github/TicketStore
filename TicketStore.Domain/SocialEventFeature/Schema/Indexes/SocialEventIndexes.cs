using Marten;
using TicketStore.Domain.SocialEventFeature.Schema.Aggregates;

namespace TicketStore.Domain.SocialEventFeature.Schema.Indexes;

public static class SocialEventIndexes
{
    public static void AddSocialEventIndexes(this StoreOptions options)
    {
        options.Schema.For<SocialEvent>().Identity(x => x.Id);
        //options.Schema.For<SocialEvent>().Duplicate(x => x.Title);
        //options.Schema.For<SocialEvent>().Duplicate(x => x.Type);
    }
}
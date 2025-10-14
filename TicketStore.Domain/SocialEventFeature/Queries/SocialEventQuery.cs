using Marten;

namespace TicketStore.Domain.SocialEventFeature.Queries;

public partial class SocialEventQuery : ISocialEventQuery
{
    private readonly IQuerySession _session;

    public SocialEventQuery(IQuerySession session)
    {
        _session = session;
    }
}


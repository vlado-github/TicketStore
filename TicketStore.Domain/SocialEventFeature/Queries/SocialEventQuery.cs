using Marten;

namespace TicketStore.Domain.SocialEventFeature.Queries;

public partial class SocialEventQuery : ISocialEventQuery
{
    private readonly IQuerySession _querySession;

    public SocialEventQuery(IQuerySession querySession)
    {
        _querySession = querySession;
    }
}


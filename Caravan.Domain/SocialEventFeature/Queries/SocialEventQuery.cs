using Caravan.Domain.SocialEventFeature.Queries;
using Marten;

namespace Caravan.Domain.SocialEventFeature.Queries;

public partial class SocialEventQuery : ISocialEventQuery
{
    private readonly IQuerySession _querySession;

    public SocialEventQuery(IQuerySession querySession)
    {
        _querySession = querySession;
    }
}


using Marten.Pagination;
using TicketStore.Domain.SocialEventFeature.Schema.Aggregates;
using TicketStore.Domain.SocialEventFeature.Schema.Projections;

namespace TicketStore.Domain.SocialEventFeature.Queries;

public partial class SocialEventQuery
{
    public async Task<IPagedList<SocialEventProfile>> List(int pageNumber = 1, int pageSize = 10)
    {
        return await _querySession
            .Query<SocialEventProfile>()
            .ToPagedListAsync(pageNumber, pageSize);
    }
}
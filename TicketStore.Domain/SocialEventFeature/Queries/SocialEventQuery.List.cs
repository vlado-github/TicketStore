using Marten.Pagination;
using TicketStore.Domain.SocialEventFeature.Schema.Aggregates;

namespace TicketStore.Domain.SocialEventFeature.Queries;

public partial class SocialEventQuery
{
    public async Task<IPagedList<SocialEvent>> List(int pageNumber = 1, int pageSize = 10)
    {
        return await _session
            .Query<SocialEvent>()
            .ToPagedListAsync(pageNumber, pageSize);
    }
}
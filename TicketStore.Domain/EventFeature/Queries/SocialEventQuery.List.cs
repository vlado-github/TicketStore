using Marten.Pagination;
using TicketStore.DAL.Events;

namespace TicketStore.Domain.EventFeature.Queries;

public partial class SocialEventQuery
{
    public async Task<IPagedList<SocialEvent>> List(int pageNumber = 1, int pageSize = 10)
    {
        return await _session
            .Query<SocialEvent>()
            .ToPagedListAsync(pageNumber, pageSize);
    }
}
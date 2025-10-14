using Marten.Pagination;
using TicketStore.Domain.SocialEventFeature.Schema.Documents;

namespace TicketStore.Domain.SocialEventFeature.Queries;

public interface ISocialEventQuery
{
    Task<SocialEvent> GetById(Guid streamId);
    Task<IPagedList<SocialEvent>> List(int pageNumber = 0, int pageSize = 10);
}
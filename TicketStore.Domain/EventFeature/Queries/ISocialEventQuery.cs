using Marten.Pagination;
using TicketStore.DAL.Events;
using TicketStore.DAL.Projections;

namespace TicketStore.Domain.EventFeature.Queries;

public interface ISocialEventQuery
{
    Task<SocialEvent> GetById(Guid streamId);
    Task<IPagedList<SocialEvent>> List(int pageNumber = 0, int pageSize = 10);
}
using Marten.Pagination;
using TicketStore.Domain.Base;
using TicketStore.Domain.Shared.Enums;
using TicketStore.Domain.SocialEventFeature.Schema.Aggregates;
using TicketStore.Domain.SocialEventFeature.Schema.Projections;

namespace TicketStore.Domain.SocialEventFeature.Queries;

public interface ISocialEventQuery
{
    Task<SocialEvent> GetById(Guid streamId);
    Task<PagedResult<SocialEventProfileDetails>> List(EventStatus eventStatus, int pageNumber = 0, int pageSize = 10);
}
using Marten.Pagination;
using Caravan.Domain.Base;
using Caravan.Domain.Shared.Enums;
using Caravan.Domain.SocialEventFeature.Schema.Aggregates;
using Caravan.Domain.SocialEventFeature.Schema.Projections;

namespace Caravan.Domain.SocialEventFeature.Queries;

public interface ISocialEventQuery
{
    Task<SocialEvent> GetById(Guid streamId);
    Task<PagedResult<SocialEventProfileDetails>> List(EventStatus eventStatus, int pageNumber = 0, int pageSize = 10);
}
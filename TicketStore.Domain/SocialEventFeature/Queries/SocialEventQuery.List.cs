using Marten.Pagination;
using TicketStore.Domain.Base;
using TicketStore.Domain.Shared.Enums;
using TicketStore.Domain.SocialEventFeature.Schema.Aggregates;
using TicketStore.Domain.SocialEventFeature.Schema.Projections;

namespace TicketStore.Domain.SocialEventFeature.Queries;

public partial class SocialEventQuery
{
    public async Task<PagedResult<SocialEventProfileDetails>> List(EventStatus eventStatus, int pageNumber = 1, int pageSize = 10)
    {
        var result = await _querySession
            .Query<SocialEventProfileDetails>()
            .Where(x => x.Status == eventStatus)
            .ToPagedListAsync(pageNumber, pageSize);

        return new PagedResult<SocialEventProfileDetails>
        {
            Items = result.ToList(),
            Count = result.Count,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            PageCount = result.PageCount,
            TotalItemCount = result.TotalItemCount,
            HasNextPage = result.HasNextPage,
            HasPreviousPage = result.HasPreviousPage,
            IsFirstPage = result.IsFirstPage,
            IsLastPage = result.IsLastPage,
            FirstItemOnPage = result.FirstItemOnPage,
            LastItemOnPage = result.LastItemOnPage
        };
    }
}
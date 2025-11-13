using TicketStore.Domain.Shared.Exceptions;
using TicketStore.Domain.SocialEventFeature.Schema.Aggregates;

namespace TicketStore.Domain.SocialEventFeature.Queries;

public partial class SocialEventQuery
{
    public async Task<SocialEvent> GetById(Guid id)
    {
        var socialEvent = await _querySession.LoadAsync<SocialEvent>(id);

        if (socialEvent == null)
        {
            throw new RecordNotFoundException(id);
        }

        return socialEvent;
    }
}
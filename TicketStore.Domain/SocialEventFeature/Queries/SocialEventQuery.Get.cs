using TicketStore.Domain.SocialEventFeature.Schema.Aggregates;
using TicketStore.Shared.Exceptions;

namespace TicketStore.Domain.SocialEventFeature.Queries;

public partial class SocialEventQuery
{
    public async Task<SocialEvent> GetById(Guid id)
    {
        var socialEvent = await _session.LoadAsync<SocialEvent>(id);

        if (socialEvent == null)
        {
            throw new RecordNotFoundException(id);
        }

        return socialEvent;
    }
}
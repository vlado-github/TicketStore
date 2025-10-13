using TicketStore.DAL.Events;
using TicketStore.Shared.Exceptions;

namespace TicketStore.Domain.EventFeature.Queries;

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
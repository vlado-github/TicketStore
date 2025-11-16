using Caravan.Domain.Shared.Exceptions;
using Caravan.Domain.SocialEventFeature.Schema.Aggregates;

namespace Caravan.Domain.SocialEventFeature.Queries;

public partial class SocialEventQuery
{
    public async Task<SocialEvent> GetById(Guid id)
    {
        var socialEvent = await _querySession.Events.AggregateStreamAsync<SocialEvent>(id);

        if (socialEvent == null)
        {
            throw new RecordNotFoundException(id);
        }

        return socialEvent;
    }
}
using Caravan.Domain.Base;

namespace Caravan.Domain.SocialEventFeature.Events;

public record SocialEventArchived : EventBase
{
    public Guid Id { get; init; }
}
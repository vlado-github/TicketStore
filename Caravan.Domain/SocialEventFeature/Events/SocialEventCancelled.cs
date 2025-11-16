using Caravan.Domain.Base;

namespace Caravan.Domain.SocialEventFeature.Events;

public record SocialEventCancelled : EventBase
{
    public Guid Id { get; init; }
}
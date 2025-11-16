using Caravan.Domain.Base;

namespace Caravan.Domain.SocialEventFeature.Events;

public record SocialEventPublished : EventBase
{
    public Guid Id { get; init; }
}
using Caravan.Domain.Base;

namespace Caravan.Domain.SocialEventFeature.Events;

public record SocialEventRescheduled : EventBase
{
    public Guid Id { get; init; }
    public DateTimeOffset StartTime { get; init; }
    public DateTimeOffset? EndTime { get; init; } = null;
}
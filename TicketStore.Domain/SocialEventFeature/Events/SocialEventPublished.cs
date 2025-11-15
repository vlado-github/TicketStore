using TicketStore.Domain.Base;

namespace TicketStore.Domain.SocialEventFeature.Events;

public record SocialEventPublished : EventBase
{
    public Guid Id { get; init; }
}
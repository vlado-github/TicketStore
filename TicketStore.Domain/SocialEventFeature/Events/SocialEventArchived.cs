using TicketStore.Domain.Base;

namespace TicketStore.Domain.SocialEventFeature.Events;

public record SocialEventArchived : EventBase
{
    public Guid Id { get; init; }
}
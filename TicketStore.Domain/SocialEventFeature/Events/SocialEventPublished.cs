using TicketStore.Shared.Enums;

namespace TicketStore.Domain.SocialEventFeature.Events;

public record SocialEventPublished
{
    public Guid Id { get; init; }
}
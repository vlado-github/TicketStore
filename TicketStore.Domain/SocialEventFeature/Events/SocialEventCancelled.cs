namespace TicketStore.Domain.SocialEventFeature.Events;

public record SocialEventCancelled
{
    public Guid Id { get; init; }
}
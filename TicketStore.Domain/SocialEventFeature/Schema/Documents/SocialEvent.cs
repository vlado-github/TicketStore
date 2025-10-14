using TicketStore.Shared.Enums;

namespace TicketStore.Domain.SocialEventFeature.Schema.Documents;

public record SocialEvent
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public EventType Type { get; init; }
    public string Venue { get; init; }
    public DateTimeOffset StartTime { get; init; }
    public DateTimeOffset? EndTime { get; init; }
}
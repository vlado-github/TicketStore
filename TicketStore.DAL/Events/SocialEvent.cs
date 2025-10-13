using TicketStore.Shared.Enums;

namespace TicketStore.DAL.Events;

public record SocialEvent
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public EventType Type { get; init; }
    public string Venue { get; init; }
    public DateTimeOffset StartTime { get; init; }
    public DateTimeOffset? EndTime { get; init; }
}
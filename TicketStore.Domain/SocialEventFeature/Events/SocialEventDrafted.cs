using TicketStore.Domain.Shared.Enums;

namespace TicketStore.Domain.SocialEventFeature.Events;

public record SocialEventDrafted
{
    public string Title { get; init; }
    public EventType Type { get; init; }
    public EventStatus Status => EventStatus.Draft;
    public string Venue { get; init; }
    public DateTimeOffset StartTime { get; init; }
    public DateTimeOffset? EndTime { get; init; }
    public int TicketCirculationCount { get; init; }
    
}
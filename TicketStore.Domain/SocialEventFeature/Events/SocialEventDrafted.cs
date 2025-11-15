using TicketStore.Domain.Base;
using TicketStore.Domain.Shared.Enums;

namespace TicketStore.Domain.SocialEventFeature.Events;

public record SocialEventDrafted : EventBase
{
    public string Title { get; init; }
    public string Description { get; init; }
    public EventType Type { get; init; }
    public string Venue { get; init; }
    public DateTimeOffset StartTime { get; init; }
    public DateTimeOffset? EndTime { get; init; }
    public int TicketCirculationCount { get; init; }
    
}
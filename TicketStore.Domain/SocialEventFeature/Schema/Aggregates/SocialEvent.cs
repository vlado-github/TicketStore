using TicketStore.Domain.Shared.Enums;
using TicketStore.Domain.SocialEventFeature.Events;

namespace TicketStore.Domain.SocialEventFeature.Schema.Aggregates;

public class SocialEvent
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public EventType Type { get; private set; }
    public EventStatus Status { get; private set; } = EventStatus.Draft;
    public string Venue { get; private set; }
    public DateTimeOffset StartTime { get; private set; }
    public DateTimeOffset? EndTime { get; private set; }
    public int TicketCirculationCount { get; private set; }
    public DateTimeOffset PublishedAt { get; private set; }
    public DateTimeOffset CancelledAt { get; private set; }
    
    public static SocialEvent Create(SocialEventDrafted @event)
    {
        return new SocialEvent
        {
            Status = @event.Status,
            PublishedAt = DateTimeOffset.UtcNow
        };
    }
    
    public static SocialEvent Apply(SocialEvent current, SocialEventPublished @event)
    {
        current.Status = EventStatus.Published;
        current.PublishedAt = DateTimeOffset.UtcNow;
        return current;
    }

    public static SocialEvent Apply(SocialEvent current, SocialEventCancelled @event)
    {
        current.Status = EventStatus.Cancelled;
        current.CancelledAt = DateTimeOffset.UtcNow;
        return current;
    }
}
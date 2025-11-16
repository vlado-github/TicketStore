using Caravan.Domain.Shared.Enums;
using Caravan.Domain.Shared.ValueObjects;
using Caravan.Domain.SocialEventFeature.Events;

namespace Caravan.Domain.SocialEventFeature.Schema.Aggregates;

public class SocialEvent
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public EventType Type { get; private set; }
    public EventStatus Status { get; private set; } = EventStatus.Draft;
    public string Venue { get; private set; }
    public string City { get; private set; }
    public GeoLocation? Location { get; private set; } = null;
    public DateTimeOffset StartTime { get; private set; }
    public DateTimeOffset? EndTime { get; private set; } = null;
    public int TicketCirculationCount { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? PublishedAt { get; private set; } = null;
    public DateTimeOffset? CancelledAt { get; private set; } = null;
    public DateTimeOffset? ArchivedAt { get; private set; } = null;
    
    public static SocialEvent Create(SocialEventDrafted @event)
    {
        return new SocialEvent
        {
            Title = @event.Title,
            Description = @event.Description,
            Type = @event.Type,
            Status = EventStatus.Draft,
            Venue = @event.Venue,
            StartTime = @event.StartTime,
            EndTime = @event.EndTime,
            TicketCirculationCount = @event.TicketCirculationCount,
            CreatedAt = DateTimeOffset.UtcNow
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
    
    public static SocialEvent Apply(SocialEvent current, SocialEventArchived @event)
    {
        current.Status = EventStatus.Archived;
        current.ArchivedAt = DateTimeOffset.UtcNow;
        return current;
    }
    
    public static SocialEvent Apply(SocialEvent current, SocialEventRescheduled @event)
    {
        current.StartTime = @event.StartTime;
        current.EndTime = @event.EndTime;
        return current;
    }
}
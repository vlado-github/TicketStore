using JasperFx.Events;
using Marten.Events.Projections;
using TicketStore.Domain.Shared.Enums;
using TicketStore.Domain.SocialEventFeature.Events;

namespace TicketStore.Domain.SocialEventFeature.Schema.Projections;

public class SocialEventProfile: EventProjection
{
    public SocialEventProfileDetails Create(IEvent<SocialEventDrafted> input)
    {
        return new SocialEventProfileDetails
        {
            Id = input.Id, 
            Title = input.Data.Title,
            Type = input.Data.Type,
            Venue = input.Data.Venue,
            StartTime = input.Data.StartTime,
        };
    }
    
    public SocialEventProfileDetails Apply(SocialEventProfileDetails current, IEvent<SocialEventPublished> input)
    {
        current.Id = input.Id;
        current.Status = EventStatus.Published;
        return current;
    }
}

public class SocialEventProfileDetails
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public EventType Type { get; set; }
    public EventStatus Status { get; set; }
    public string Venue { get; set; }
    public DateTimeOffset StartTime { get; set; }
}
using JasperFx.Events;
using Marten.Events.Aggregation;
using Caravan.Domain.Shared.Enums;
using Caravan.Domain.SocialEventFeature.Events;

namespace Caravan.Domain.SocialEventFeature.Schema.Projections;

public class SocialEventProfile: SingleStreamProjection<SocialEventProfileDetails, Guid>
{
    public SocialEventProfileDetails Create(IEvent<SocialEventDrafted> input)
    {
        return new SocialEventProfileDetails
        {
            Id = input.Id, 
            Title = input.Data.Title,
            Description = input.Data.Description,
            Type = input.Data.Type,
            Venue = input.Data.Venue,
            StartTime = input.Data.StartTime,
            EndTime = input.Data.EndTime,
            TicketCirculationCount = input.Data.TicketCirculationCount
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
    public string Description { get; set; }
    public EventType Type { get; set; }
    public EventStatus Status { get; set; }
    public string Venue { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset? EndTime { get; set; } = null;
    public int TicketCirculationCount { get; set; }
}
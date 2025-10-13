using Marten;
using TicketStore.DAL.Events;
using TicketStore.Domain.EventFeature.Events;
using TicketStore.Shared.Enums;

namespace TicketStore.Domain.EventFeature.Commands;

public record CreateSocialEventCommand(
    string Title, 
    EventType Type,
    string Venue,
    DateTimeOffset StartTime,
    DateTimeOffset? EndTime);

public class CreateScheduledEventCommandHandler
{
    public static async Task<SocialEventCreated> Handle(CreateSocialEventCommand command, IDocumentSession session)
    {
        var socialEvent = new SocialEvent()
        {
            Id = Guid.NewGuid(),
            Title = command.Title,
            Type = command.Type,
            Venue = command.Venue,
            StartTime = command.StartTime,
            EndTime = command.EndTime
        };
        session.Store(socialEvent);
        await session.SaveChangesAsync();
        
        return new SocialEventCreated
        {
            Id = socialEvent.Id
        };
    }
}
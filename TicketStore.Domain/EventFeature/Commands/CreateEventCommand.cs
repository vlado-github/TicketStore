using TicketStore.DAL.DataAccess;
using TicketStore.DAL.Entities;
using TicketStore.Domain.EventFeature.Events;
using TicketStore.Shared.Enums;
using Wolverine.Attributes;

namespace TicketStore.Domain.EventFeature.Commands;

public record CreateEventCommand(
    string Title, 
    EventType Type,
    string Venue,
    DateTimeOffset StartTime,
    DateTimeOffset? EndTime);

public class CreateEventCommandHandler
{
    [Transactional]
    public static EventCreated Handle(CreateEventCommand command, TicketStoreContext dbContext)
    {
        //todo: use mapster
        var @event = new Event()
        {
            Title = command.Title,
            Type = command.Type,
            Venue = command.Venue,
            StartTime = command.StartTime,
            EndTime = command.EndTime
        };
        dbContext.Events.Add(@event);
        return new EventCreated
        {
            Id = @event.Id
        };
    }
}
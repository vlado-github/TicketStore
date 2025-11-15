using FluentValidation;
using Marten;
using TicketStore.Domain.Base;
using TicketStore.Domain.Shared.Enums;
using TicketStore.Domain.SocialEventFeature.Events;
using TicketStore.Domain.SocialEventFeature.Schema.Aggregates;

namespace TicketStore.Domain.SocialEventFeature.Commands;

public class CreateSocialEventCommandValidator : AbstractValidator<CreateSocialEventCommand>
{
    public CreateSocialEventCommandValidator()
    {
        RuleFor(x => x.Title).NotNull().NotEmpty();
        RuleFor(x => x.Type).NotNull();
        RuleFor(x => x.Venue).NotNull().NotEmpty();
        RuleFor(x => x.StartTime).NotNull();
    }
}

public record CreateSocialEventCommand(
    string Title, 
    string Description, 
    EventType Type,
    string Venue,
    DateTimeOffset StartTime,
    DateTimeOffset? EndTime,
    int TicketCirculationCount);

public class CreateSocialEventCommandHandler
{
    public static async Task<CommandResult> Handle(CreateSocialEventCommand command, IDocumentStore store)
    {
        await using var session = store.LightweightSession();

        var draftEvent = new SocialEventDrafted
        {
            Title = command.Title,
            Description = command.Description,
            Type = command.Type,
            Venue = command.Venue,
            StartTime = command.StartTime,
            EndTime = command.EndTime,
            TicketCirculationCount = command.TicketCirculationCount
        };
        var stream = session.Events.StartStream<SocialEvent>(draftEvent);
        await session.SaveChangesAsync();
        
        return new CommandResult(stream.Id);
    }
}
using FluentValidation;
using Marten;
using Caravan.Domain.Base;
using Caravan.Domain.Shared.Enums;
using Caravan.Domain.SocialEventFeature.Events;
using Caravan.Domain.SocialEventFeature.Schema.Aggregates;

namespace Caravan.Domain.SocialEventFeature.Commands;

public class CreateSocialEventCommandValidator : AbstractValidator<CreateSocialEventCommand>
{
    public CreateSocialEventCommandValidator()
    {
        RuleFor(x => x.Title).NotNull().NotEmpty();
        RuleFor(x => x.Type).NotNull();
        RuleFor(x => x.Venue).NotNull().NotEmpty();
        RuleFor(x => x.StartTime).NotNull();
        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime)
            .When(x => x.EndTime.HasValue);
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
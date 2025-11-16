using Caravan.Domain.Shared.Enums;
using Caravan.Domain.SocialEventFeature.Events;
using Caravan.Domain.SocialEventFeature.Schema.Aggregates;
using FluentValidation;
using Wolverine.Marten;

namespace Caravan.Domain.SocialEventFeature.Commands;


public class RescheduleSocialEventCommandValidator : AbstractValidator<RescheduleSocialEventCommand>
{
    public RescheduleSocialEventCommandValidator()
    {
        RuleFor(x => x.SocialEventId).NotEmpty();
        RuleFor(x => x.StartTime)
            .NotEmpty()
            .Must(x => x.ToUniversalTime() > DateTime.UtcNow)
            .WithMessage("Start time must be in the future");
        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime)
            .When(x => x.EndTime.HasValue);
    }
}

public record RescheduleSocialEventCommand(Guid SocialEventId, DateTimeOffset StartTime, DateTimeOffset? EndTime = null);

public static class RescheduleSocialEventCommandHandler
{
    [AggregateHandler]
    public static IEnumerable<object> Handle(RescheduleSocialEventCommand command, [WriteAggregate] SocialEvent socialEvent)
    {
        if (socialEvent.Status is EventStatus.Cancelled or EventStatus.Archived)
            throw new InvalidOperationException("Can't reschedule cancelled or archived event");

        yield return new SocialEventRescheduled()
        {
            Id = command.SocialEventId, 
            StartTime =  command.StartTime, 
            EndTime = command.EndTime
        };
    }
}
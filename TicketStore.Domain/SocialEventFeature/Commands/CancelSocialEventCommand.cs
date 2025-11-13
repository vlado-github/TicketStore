using FluentValidation;
using Marten;
using TicketStore.Domain.Base;
using TicketStore.Domain.Shared.Enums;
using TicketStore.Domain.SocialEventFeature.Events;
using TicketStore.Domain.SocialEventFeature.Schema.Aggregates;
using Wolverine.Marten;

namespace TicketStore.Domain.SocialEventFeature.Commands;

public class CancelSocialEventCommandValidator : AbstractValidator<CancelSocialEventCommand>
{
    public CancelSocialEventCommandValidator()
    {
        RuleFor(x => x.SocialEventId).NotEmpty();
    }
}

public record CancelSocialEventCommand(Guid SocialEventId);

public static class CancelSocialEventCommandHandler
{
    [AggregateHandler]
    public static IEnumerable<object> Handle(CancelSocialEventCommand command, SocialEvent socialEvent)
    {
        if (socialEvent.Status == EventStatus.Archived)
            throw new InvalidOperationException("Can't cancel archived event");

        yield return new SocialEventCancelled { Id = socialEvent.Id };
    }
}
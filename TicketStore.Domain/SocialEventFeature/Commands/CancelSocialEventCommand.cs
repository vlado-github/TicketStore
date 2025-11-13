using FluentValidation;
using Marten;
using TicketStore.Domain.Base;
using TicketStore.Domain.Shared.Enums;
using TicketStore.Domain.SocialEventFeature.Events;
using TicketStore.Domain.SocialEventFeature.Schema.Aggregates;

namespace TicketStore.Domain.SocialEventFeature.Commands;

public class CancelSocialEventCommandValidator : AbstractValidator<CancelSocialEventCommand>
{
    public CancelSocialEventCommandValidator()
    {
        RuleFor(x => x.EventId).NotEmpty();
    }
}

public record CancelSocialEventCommand(Guid EventId);

public class CancelSocialEventCommandHandler
{
    public static IEnumerable<object> Handle(CancelSocialEventCommand command, SocialEvent socialEvent)
    {
        if (socialEvent.Status == EventStatus.Archived)
            throw new InvalidOperationException("Can't cancel archived event");

        yield return new SocialEventCancelled { Id = socialEvent.Id };
    }
}
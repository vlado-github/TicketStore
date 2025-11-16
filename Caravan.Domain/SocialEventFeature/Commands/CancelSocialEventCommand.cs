using FluentValidation;
using Marten;
using Caravan.Domain.Base;
using Caravan.Domain.Shared.Enums;
using Caravan.Domain.SocialEventFeature.Events;
using Caravan.Domain.SocialEventFeature.Schema.Aggregates;
using Wolverine.Marten;

namespace Caravan.Domain.SocialEventFeature.Commands;

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
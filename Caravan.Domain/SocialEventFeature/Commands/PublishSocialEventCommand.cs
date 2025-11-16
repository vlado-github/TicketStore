using FluentValidation;
using Caravan.Domain.Shared.Enums;
using Caravan.Domain.SocialEventFeature.Events;
using Caravan.Domain.SocialEventFeature.Schema.Aggregates;
using Wolverine.Marten;

namespace Caravan.Domain.SocialEventFeature.Commands;

public class PublishSocialEventCommandValidator : AbstractValidator<PublishSocialEventCommand>
{
    public PublishSocialEventCommandValidator()
    {
        RuleFor(x => x.SocialEventId).NotEmpty();
    }
}

public record PublishSocialEventCommand(Guid SocialEventId);

public static class PublishSocialEventCommandHandler
{
    [AggregateHandler]
    public static IEnumerable<object> Handle(PublishSocialEventCommand command, [WriteAggregate] SocialEvent socialEvent)
    {
        if (socialEvent.Status != EventStatus.Draft)
            throw new InvalidOperationException("Can only publish draft events");

        yield return new SocialEventPublished { Id = command.SocialEventId };
    }
}
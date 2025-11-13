using FluentValidation;
using Marten;
using TicketStore.Domain.Base;
using TicketStore.Domain.Shared.Enums;
using TicketStore.Domain.SocialEventFeature.Events;
using TicketStore.Domain.SocialEventFeature.Schema.Aggregates;

namespace TicketStore.Domain.SocialEventFeature.Commands;

public class PublishSocialEventCommandValidator : AbstractValidator<PublishSocialEventCommand>
{
    public PublishSocialEventCommandValidator()
    {
        RuleFor(x => x.EventId).NotEmpty();
    }
}

public record PublishSocialEventCommand(Guid EventId);

public class PublishSocialEventCommandHandler
{
    public static IEnumerable<object> Handle(PublishSocialEventCommand command, SocialEvent socialEvent)
    {
        if (socialEvent.Status != EventStatus.Draft)
            throw new InvalidOperationException("Can only publish draft events");

        yield return new SocialEventPublished { Id = socialEvent.Id };
    }
}
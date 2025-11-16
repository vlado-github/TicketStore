using Caravan.Domain.Shared.Consts;
using Caravan.Domain.SocialEventFeature.Events;
using Caravan.Domain.SocialEventFeature.Schema.Aggregates;
using FluentValidation;
using Wolverine.Marten;

namespace Caravan.Domain.SocialEventFeature.Commands;

public class ArchiveSocialEventCommandValidator : AbstractValidator<ArchiveSocialEventCommand>
{
    public ArchiveSocialEventCommandValidator()
    {
        RuleFor(x => x.SocialEventId).NotEmpty();
    }
}

public record ArchiveSocialEventCommand(Guid SocialEventId);

public static class ArchiveSocialEventCommandHandler
{
    [AggregateHandler]
    public static IEnumerable<object> Handle(ArchiveSocialEventCommand command, SocialEvent socialEvent)
    {
        if (socialEvent.EndTime.HasValue)
        {
            if (socialEvent.EndTime.Value.ToUniversalTime() <
                DateTimeOffset.UtcNow.AddDays(Defaults.RequiredPassedTimeForArchiveInDays))
            {
                throw new InvalidOperationException(
                    $"Can't archived event unless {Defaults.RequiredPassedTimeForArchiveInDays} days are passed");
            }
        }
        else
        {
            if (socialEvent.StartTime.ToUniversalTime() <
                DateTimeOffset.UtcNow.AddDays(Defaults.RequiredPassedTimeForArchiveInDays))
            {
                throw new InvalidOperationException(
                    $"Can't archived event unless {Defaults.RequiredPassedTimeForArchiveInDays} days are passed");
            }
        }

        yield return new SocialEventArchived() { Id = socialEvent.Id };
    }
}
using Alba;
using Bogus;
using Marten;
using Marten.Pagination;
using Microsoft.Extensions.DependencyInjection;
using TicketStore.Domain.Base;
using TicketStore.Domain.Shared.Enums;
using TicketStore.Domain.SocialEventFeature.Events;
using TicketStore.Domain.SocialEventFeature.Schema.Aggregates;
using TicketStore.Domain.SocialEventFeature.Schema.Projections;
using TicketStore.Tests.Base;

namespace TicketStore.Tests.IntegrationTests;

public class GetSocialEventsListTests : IClassFixture<IntegrationTestFixture>
{
    private readonly IAlbaHost _host;
    private readonly DataSeeder _seeder;
    
    public GetSocialEventsListTests(IntegrationTestFixture fixture)
    {
        _host = fixture.Host;
        _seeder = fixture.Seeder;
    }
    
    [Fact]
    public async Task GetEventsList_Should_Succeed()
    {
        //Arrange
        var numberOfDraftEvents = 5;
        var numberOfPublishedEvents = 5;
        var pageNumber = 1;
        var pageSize = 3;

        await Seed(numberOfDraftEvents, EventStatus.Draft);
        await Seed(numberOfPublishedEvents, EventStatus.Published);
        
        //Act
        var response = await _host.Scenario(config =>
        {
            config.Get.Url($"/socialevent/list/{pageNumber}/{pageSize}");
            config.StatusCodeShouldBeOk();
        });

        //Assert
        var result = await response.ReadAsJsonAsync<PagedResult<SocialEventProfileDetails>>();
        Assert.NotNull(result);
        Assert.Equal(pageSize, result.PageSize);
        Assert.True(result.IsFirstPage);
        Assert.False(result.HasPreviousPage);
        Assert.True(result.HasNextPage);
        Assert.All(result.Items, item =>  Assert.NotEqual(Guid.Empty, item.Id));
        Assert.All(result.Items, item =>  Assert.NotEmpty(item.Title));
        Assert.All(result.Items, item =>  Assert.NotEmpty(item.Description));
        Assert.All(result.Items, item =>  Assert.NotEqual(DateTimeOffset.MinValue, item.StartTime));
    }

    private async Task Seed(int number, EventStatus status)
    {
        switch (status)
        {
            case EventStatus.Draft:
            {
                for(var i = 0; i < number; i++)
                {
                    var streamId = Guid.NewGuid();
                    await _seeder.Seed<SocialEvent>(streamId);
                }
                
                break;
            }
            case EventStatus.Published:
            {
                for(var i = 0; i < number; i++)
                {
                    var streamId = Guid.NewGuid();
                    await _seeder.Seed<SocialEvent>(streamId, new List<EventBase>()
                    {
                        new SocialEventPublished()
                        {
                            Id = streamId
                        }
                    });
                }
                break;
            }
            
        }
    }
}
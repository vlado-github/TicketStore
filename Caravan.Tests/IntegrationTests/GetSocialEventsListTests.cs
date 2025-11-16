using Alba;
using Caravan.Tests.Base;
using Caravan.Domain.Base;
using Caravan.Domain.Shared.Enums;
using Caravan.Domain.SocialEventFeature.Events;
using Caravan.Domain.SocialEventFeature.Schema.Aggregates;
using Caravan.Domain.SocialEventFeature.Schema.Projections;

namespace Caravan.Tests.IntegrationTests;

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
    public async Task Get_EventsList_Should_Succeed()
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
using System.Net;
using Alba;
using Bogus;
using Marten;
using TicketStore.Domain.Base;
using TicketStore.Domain.Shared.Enums;
using TicketStore.Domain.SocialEventFeature.Commands;
using TicketStore.Domain.SocialEventFeature.Events;
using TicketStore.Domain.SocialEventFeature.Schema.Aggregates;
using TicketStore.Domain.SocialEventFeature.Schema.Projections;
using TicketStore.Tests.Base;

namespace TicketStore.Tests.IntegrationTests;

public class CancelSocialEventCommandTests : IClassFixture<IntegrationTestFixture>
{
    private readonly IAlbaHost _host;
    private readonly DataSeeder _seeder;
    
    public CancelSocialEventCommandTests(IntegrationTestFixture fixture)
    {
        _host = fixture.Host;
        _seeder = fixture.Seeder;
    }
    
    [Fact]
    public async Task Cancel_PublishedSocialEvent_Should_Succeed()
    {
        //Arrange
        var streamId = Guid.NewGuid();
        await _seeder.Seed<SocialEvent>(streamId, new List<EventBase>()
        {
            new SocialEventPublished()
            {
                Id = streamId
            }
        });
        var aggregate = await _seeder.GetStream<SocialEvent>(streamId);
        var command = new CancelSocialEventCommand(streamId);
        
        //Act
        await _host.Scenario(config =>
        {
            config.Put.Json(command).ToUrl("/socialevent/cancel");
            config.StatusCodeShouldBeOk();
        });

        var getResponse = await _host.Scenario(config =>
        {
            config.Get.Url($"/socialevent/{streamId}");
            config.StatusCodeShouldBeOk();
        });

        //Assert
        var result = await getResponse.ReadAsJsonAsync<SocialEventProfileDetails>();
        Assert.NotNull(result);
        Assert.Equal(aggregate.Title, result.Title);
        Assert.Equal(aggregate.Description, result.Description);
        Assert.Equal(aggregate.Type, result.Type);
        Assert.Equal(aggregate.StartTime, result.StartTime);
        Assert.Equal(aggregate.EndTime, result.EndTime);
        Assert.Equal(aggregate.Venue, result.Venue);
        Assert.Equal(EventStatus.Cancelled, result.Status);
        Assert.Equal(aggregate.TicketCirculationCount, result.TicketCirculationCount);
    }
    
    [Fact]
    public async Task Cancel_ArchivedSocialEvent_Should_Fail()
    {
        //Arrange
        var streamId = Guid.NewGuid();
        await _seeder.Seed<SocialEvent>(streamId, new List<EventBase>()
        {
            new SocialEventArchived()
            {
                Id = streamId
            }
        });
        var command = new CancelSocialEventCommand(streamId);
        
        //Act & Assert
        await _host.Scenario(config =>
        {
            config.Put.Json(command).ToUrl("/socialevent/cancel");
            config.StatusCodeShouldBe(HttpStatusCode.BadRequest);
        });
    }
}
using System.Net;
using Alba;
using Bogus;
using Caravan.Tests.Base;
using Marten;
using Caravan.Domain.Base;
using Caravan.Domain.Shared.Enums;
using Caravan.Domain.SocialEventFeature.Commands;
using Caravan.Domain.SocialEventFeature.Events;
using Caravan.Domain.SocialEventFeature.Schema.Aggregates;
using Caravan.Domain.SocialEventFeature.Schema.Projections;

namespace Caravan.Tests.IntegrationTests;

public class ArchiveSocialEventCommandTests : IClassFixture<IntegrationTestFixture>
{
    private readonly IAlbaHost _host;
    private readonly DataSeeder _seeder;
    
    public ArchiveSocialEventCommandTests(IntegrationTestFixture fixture)
    {
        _host = fixture.Host;
        _seeder = fixture.Seeder;
    }
    
    [Fact]
    public async Task Archive_SocialEvent_Should_Succeed()
    {
        //Arrange
        var streamId = Guid.NewGuid();
        await _seeder.Seed<SocialEvent>(streamId);
        var aggregate = await _seeder.GetStream<SocialEvent>(streamId);
        var command = new ArchiveSocialEventCommand(streamId);
        
        //Act
        await _host.Scenario(config =>
        {
            config.Put.Json(command).ToUrl("/socialevent/archive");
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
        Assert.Equal(EventStatus.Archived, result.Status);
        Assert.Equal(aggregate.TicketCirculationCount, result.TicketCirculationCount);
    }
    
    [Fact]
    public async Task Archive_SocialEvent_WithEndDate_SoonerThenRequired_Should_Fail()
    {
        //Arrange
        var streamId = Guid.NewGuid();
        await _seeder.Seed<SocialEvent>(streamId, new List<EventBase>()
        {
            new SocialEventRescheduled()
            {
                Id = streamId,
                StartTime = DateTimeOffset.UtcNow,
                EndTime = DateTimeOffset.UtcNow.AddHours(5),
            }
        });
        var command = new ArchiveSocialEventCommand(streamId);
        
        //Act & Assert
        await _host.Scenario(config =>
        {
            config.Put.Json(command).ToUrl("/socialevent/archive");
            config.StatusCodeShouldBe(HttpStatusCode.BadRequest);
        });
    }
    
    [Fact]
    public async Task Archive_SocialEvent_WithoutEndDate_SoonerThenRequired_Should_Fail()
    {
        //Arrange
        var streamId = Guid.NewGuid();
        await _seeder.Seed<SocialEvent>(streamId, new List<EventBase>()
        {
            new SocialEventRescheduled()
            {
                Id = streamId,
                StartTime = DateTimeOffset.UtcNow,
                EndTime = null,
            }
        });
        var command = new ArchiveSocialEventCommand(streamId);
        
        //Act & Assert
        await _host.Scenario(config =>
        {
            config.Put.Json(command).ToUrl("/socialevent/archive");
            config.StatusCodeShouldBe(HttpStatusCode.BadRequest);
        });
    }
}
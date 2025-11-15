using Alba;
using Bogus;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using TicketStore.Domain.Shared.Enums;
using TicketStore.Domain.SocialEventFeature.Events;
using TicketStore.Domain.SocialEventFeature.Schema.Aggregates;
using TicketStore.Domain.SocialEventFeature.Schema.Projections;
using TicketStore.Tests.Base;

namespace TicketStore.Tests.IntegrationTests;

public class GetSocialEventByIdTests : IClassFixture<IntegrationTestFixture>
{
    private readonly IAlbaHost _host;
    private readonly Faker _faker;
    private readonly DataSeeder _seeder;
    
    public GetSocialEventByIdTests(IntegrationTestFixture fixture)
    {
        _host = fixture.Host;
        _faker = new Faker();
        _seeder = fixture.Seeder;
    }
    
    [Fact]
    public async Task GetSocialEvent_ById_Should_Succeed()
    {
        //Arrange
        var streamId = Guid.NewGuid();
        var streamContext = _seeder.NewStream(streamId);
        var aggregate = await streamContext.Start<SocialEvent>();
        
        //Act
        var response = await _host.Scenario(config =>
        {
            config.Get.Url($"/socialevent/{streamId}");
            config.StatusCodeShouldBeOk();
        });

        //Assert
        var result = await response.ReadAsJsonAsync<SocialEventProfileDetails>();
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(aggregate.Title, result.Title);
        Assert.Equal(aggregate.Description, result.Description);
        Assert.Equal(aggregate.Type, result.Type);
        Assert.Equal(aggregate.StartTime, result.StartTime);
        Assert.Equal(aggregate.EndTime, result.EndTime);
        Assert.Equal(aggregate.Venue, result.Venue);
        Assert.Equal(aggregate.TicketCirculationCount, result.TicketCirculationCount);
    }
}
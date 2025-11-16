using Alba;
using Caravan.Tests.Base;
using Caravan.Domain.SocialEventFeature.Schema.Aggregates;
using Caravan.Domain.SocialEventFeature.Schema.Projections;

namespace Caravan.Tests.IntegrationTests;

public class GetSocialEventByIdTests : IClassFixture<IntegrationTestFixture>
{
    private readonly IAlbaHost _host;
    private readonly DataSeeder _seeder;
    
    public GetSocialEventByIdTests(IntegrationTestFixture fixture)
    {
        _host = fixture.Host;
        _seeder = fixture.Seeder;
    }
    
    [Fact]
    public async Task GetSocialEvent_ById_Should_Succeed()
    {
        //Arrange
        var streamId = Guid.NewGuid();
        await _seeder.Seed<SocialEvent>(streamId);
        var aggregate = await _seeder.GetStream<SocialEvent>(streamId);
        
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
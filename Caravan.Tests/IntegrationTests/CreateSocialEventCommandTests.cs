using Alba;
using Bogus;
using Caravan.Tests.Base;
using Caravan.Domain.Base;
using Caravan.Domain.Shared.Enums;
using Caravan.Domain.SocialEventFeature.Commands;
using Caravan.Domain.SocialEventFeature.Schema.Aggregates;
using Caravan.Domain.SocialEventFeature.Schema.Projections;

namespace Caravan.Tests.IntegrationTests;

public class CreateSocialEventCommandTests : IClassFixture<IntegrationTestFixture>
{
    private readonly IAlbaHost _host;
    private readonly Faker _faker;
    
    public CreateSocialEventCommandTests(IntegrationTestFixture fixture)
    {
        _host = fixture.Host;
        _faker = new Faker();
    }
    
    [Fact]
    public async Task CreateSocialEvent_Should_Succeed()
    {
        //Arrange
        var command = new CreateSocialEventCommand(
            Title: _faker.Lorem.Sentence(3),
            Description: _faker.Lorem.Paragraph(),
            Type: EventType.OnSite,
            StartTime: DateTimeOffset.UtcNow.AddDays(10),
            EndTime: DateTimeOffset.UtcNow.AddDays(10).AddHours(2),
            Venue: _faker.Address.FullAddress(),
            TicketCirculationCount: 100
        );
        
        //Act
        var createResponse = await _host.Scenario(config =>
        {
            config.Post.Json(command).ToUrl("/socialevent");
            config.StatusCodeShouldBeOk();
        });
        
        //Assert
        var createResult = await createResponse.ReadAsJsonAsync<CommandResult>();
        Assert.NotNull(createResult);
        Assert.NotEqual(Guid.Empty, createResult.Id);
        
        //Act
        var getResponse = await _host.Scenario(config =>
        {
            config.Get.Url($"/socialevent/{createResult.Id}");
            config.StatusCodeShouldBeOk();
        });

        //Assert
        var result = await getResponse.ReadAsJsonAsync<SocialEventProfileDetails>();
        Assert.NotNull(result);
        Assert.Equal(command.Title, result.Title);
        Assert.Equal(command.Description, result.Description);
        Assert.Equal(command.Type, result.Type);
        Assert.Equal(command.StartTime, result.StartTime);
        Assert.Equal(command.EndTime, result.EndTime);
        Assert.Equal(command.Venue, result.Venue);
        Assert.Equal(command.TicketCirculationCount, result.TicketCirculationCount);
    }
}
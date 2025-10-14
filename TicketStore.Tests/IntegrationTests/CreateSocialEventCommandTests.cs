using Alba;
using Bogus;
using TicketStore.Domain.Base;
using TicketStore.Domain.SocialEventFeature.Commands;
using TicketStore.Shared.Enums;
using TicketStore.Tests.Base;

namespace TicketStore.Tests.IntegrationTests;

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
    public async Task CreateAccount_Should_Succeed()
    {
        //Arrange
        var command = new CreateSocialEventCommand(
            Title: _faker.Rant.Locale,
            Type: EventType.Live,
            StartTime: DateTimeOffset.UtcNow.AddDays(10),
            EndTime: DateTimeOffset.UtcNow.AddDays(10).AddHours(2),
            Venue: _faker.Address.FullAddress()
        );
        
        //Act
        var response = await _host.Scenario(config =>
        {
            config.Post.Json(command).ToUrl("/socialevent");
            config.StatusCodeShouldBeOk();
        });

        //Assert
        var result = await response.ReadAsJsonAsync<CommandResult>();
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
    }
}
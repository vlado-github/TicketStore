using Alba;
using Alba.Security;
using DotNet.Testcontainers.Builders;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Testcontainers.PostgreSql;

namespace TicketStore.Tests.Base;

public class IntegrationTestFixture : IAsyncLifetime
{
    public IAlbaHost Host = null!;
    private readonly PostgreSqlContainer _postgres;

    public IntegrationTestFixture()
    {
        _postgres = new PostgreSqlBuilder()
            .WithImage("postgres:15-alpine")
            .WithHostname("localhost")
            .WithDatabase("ticketstore")
            .WithUsername("postgres")
            .WithPassword("admin")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilDatabaseIsAvailable(NpgsqlFactory.Instance))
            .WithReuse(true)
            .WithPortBinding(5432, 5432)
            .Build();
    }
    
    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();
        Host = await AlbaHost.For<global::Program>(x =>
        {
            x.ConfigureServices((context, services) =>
            {
                //add mocks or stubs
            });
        });
    }

    public async Task DisposeAsync()
    {
        await Host.DisposeAsync();
        await _postgres.StopAsync();
        await _postgres.DisposeAsync();
    }
}
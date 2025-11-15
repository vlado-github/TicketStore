using Alba;
using DotNet.Testcontainers.Builders;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Testcontainers.PostgreSql;

namespace TicketStore.Tests.Base;

public class IntegrationTestFixture : IAsyncLifetime
{
    public IAlbaHost Host = null!;
    private readonly PostgreSqlContainer _postgres;
    public DataSeeder Seeder { get; private set; }

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
        Seeder = new DataSeeder(Host.Services.GetRequiredService<IDocumentStore>());
    }

    public async Task DisposeAsync()
    {
        await Host.CleanAllMartenDataAsync();
        await Host.DisposeAsync();
        await _postgres.StopAsync();
        await _postgres.DisposeAsync();
    }
}
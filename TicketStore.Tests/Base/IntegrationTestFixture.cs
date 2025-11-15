using Alba;
using DotNet.Testcontainers.Builders;
using JasperFx;
using JasperFx.Events;
using JasperFx.Events.Daemon;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Testcontainers.PostgreSql;
using TicketStore.Domain.SocialEventFeature.Schema.Indexes;
using TicketStore.Domain.SocialEventFeature.Schema.Projections;
using Wolverine.Marten;

namespace TicketStore.Tests.Base;

public class IntegrationTestFixture : IAsyncLifetime
{
    public IAlbaHost Host = null!;
    public DataSeeder Seeder { get; private set; }
    
    // public static readonly PostgreSqlContainer Postgres =
    //     new PostgreSqlBuilder()
    //         .WithImage("postgres:15-alpine")
    //         .WithDatabase("ticketstore")
    //         .WithUsername("postgres")
    //         .WithPassword("admin")
    //         .Build();
    
    public async Task InitializeAsync()
    {
        //await Postgres.StartAsync();
        Host = await AlbaHost.For<global::Program>(x =>
        {
            x.ConfigureServices((context, services) =>
            {
                
            });
        });
        Seeder = new DataSeeder(Host.Services.GetRequiredService<IDocumentStore>());
    }

    public async Task DisposeAsync()
    {
        await Seeder.DisposeAsync();
        await Host.DisposeAsync();
    }
}
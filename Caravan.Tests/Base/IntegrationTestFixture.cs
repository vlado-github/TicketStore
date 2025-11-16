using Alba;
using Marten;
using Microsoft.Extensions.DependencyInjection;

namespace Caravan.Tests.Base;

public class IntegrationTestFixture : IAsyncLifetime
{
    public IAlbaHost Host = null!;
    public DataSeeder Seeder { get; private set; }
    
    public async Task InitializeAsync()
    {
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
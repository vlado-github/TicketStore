using Bogus;
using Marten;
using TicketStore.Domain.Base;
using TicketStore.Domain.Shared.Enums;
using TicketStore.Domain.Shared.Exceptions;
using TicketStore.Domain.SocialEventFeature.Events;
using TicketStore.Domain.SocialEventFeature.Schema.Aggregates;

namespace TicketStore.Tests.Base;


public class DataSeeder : IAsyncDisposable
{
    private readonly IDocumentStore _store;
    
    public DataSeeder(IDocumentStore store)
    {
        _store = store;
    }
    
    public StreamContext NewStream(Guid streamId)
    {
        return new StreamContext(streamId, _store);
    }

    public async ValueTask DisposeAsync()
    {
        await _store.Advanced.Clean.DeleteAllDocumentsAsync();
        await _store.Advanced.Clean.DeleteAllEventDataAsync();
    }
}

public class StreamContext
{
    private readonly IDocumentStore _store;
    private readonly Faker _faker;
    private readonly Guid _streamId;
    
    public StreamContext(Guid streamId, IDocumentStore store)
    {
        _store = store;
        _streamId = streamId;
        _faker = new Faker();
    }
    
    public async Task<TAggregateRoot> Start<TAggregateRoot>() 
        where TAggregateRoot : class
    {
        if (typeof(TAggregateRoot) == typeof(SocialEvent))
        {
            var drafted = new SocialEventDrafted
            {
                Title = _faker.Lorem.Sentence(3),
                Description = _faker.Lorem.Paragraph(),
                Type = EventType.OnSite,
                StartTime = DateTimeOffset.UtcNow.AddDays(10),
                EndTime = DateTimeOffset.UtcNow.AddDays(10).AddHours(2),
                Venue = _faker.Address.FullAddress(),
                TicketCirculationCount = _faker.Random.Int(50, 5000),
            };
            await CreateAsync<SocialEvent, SocialEventDrafted>(drafted);
            return await GetAsync<TAggregateRoot>();
        }
        
        throw new ArgumentOutOfRangeException(typeof(TAggregateRoot).ToString());
    }

    public async Task<TAggregateRoot> Append<TAggregateRoot, TEvent>(TEvent @event)
        where TAggregateRoot : class
        where TEvent : EventBase
    {
        await ApplyAsync<TEvent>(@event);
        return await GetAsync<TAggregateRoot>();
    }
    
    private async Task CreateAsync<TAggregateRoot, TEvent>(TEvent @event) 
        where TAggregateRoot : class
        where TEvent : EventBase
    {
        await using var session = _store.LightweightSession();
        session.Events.StartStream<TAggregateRoot>(_streamId, @event);
        await session.SaveChangesAsync();
    }
    
    private async Task ApplyAsync<TEvent>(TEvent @event) 
        where TEvent : EventBase
    {
        await using var session = _store.LightweightSession();
        session.Events.Append(_streamId,  @event);
        await session.SaveChangesAsync();
    }
    
    private async Task<TAggregateRoot> GetAsync<TAggregateRoot>() 
        where TAggregateRoot : class
    {
        await using var session = _store.QuerySession();
        var aggregate = await session.Events.AggregateStreamAsync<TAggregateRoot>(_streamId);
        if (aggregate is null)
        {
            throw new RecordNotFoundException(_streamId);
        }
        return aggregate;
    }
}
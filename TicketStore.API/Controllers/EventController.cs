using Marten.Pagination;
using Microsoft.AspNetCore.Mvc;
using TicketStore.DAL.Events;
using TicketStore.Domain.EventFeature.Commands;
using TicketStore.Domain.EventFeature.Events;
using TicketStore.Domain.EventFeature.Queries;
using Wolverine;

namespace TicketStore.API.Controllers;

[ApiController]
[Route("[controller]")]
public class EventController : ControllerBase
{
    private readonly IMessageBus _bus;
    private readonly ISocialEventQuery  _query;
    
    public EventController(IMessageBus bus, ISocialEventQuery query)
    {
        _bus = bus;
        _query = query;
    }
    
    [HttpPost]
    public async Task<SocialEventCreated> CreateSocialEvent(CreateSocialEventCommand command)
    {
        return await _bus.InvokeAsync<SocialEventCreated>(command);
    }
    
    [HttpGet("{id}")]
    public async Task<SocialEvent> GetSocialEvent([FromRoute] Guid id)
    {
        return await _query.GetById(id);
    }
    
    [HttpGet("list/{pageNumber}/{pageSize}")]
    public async Task<IPagedList<SocialEvent>> GetSocialEvent([FromRoute] int pageNumber, [FromRoute] int pageSize)
    {
        return await _query.List(pageNumber, pageSize);
    }
}
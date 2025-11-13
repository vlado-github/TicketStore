using Marten.Pagination;
using Microsoft.AspNetCore.Mvc;
using TicketStore.Domain.Base;
using TicketStore.Domain.SocialEventFeature.Commands;
using TicketStore.Domain.SocialEventFeature.Queries;
using TicketStore.Domain.SocialEventFeature.Schema.Aggregates;
using TicketStore.Domain.SocialEventFeature.Schema.Projections;
using Wolverine;

namespace TicketStore.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SocialEventController : ControllerBase
{
    private readonly IMessageBus _bus;
    private readonly ISocialEventQuery  _query;
    
    public SocialEventController(IMessageBus bus, ISocialEventQuery query)
    {
        _bus = bus;
        _query = query;
    }
    
    [HttpPost]
    public async Task<CommandResult> CreateSocialEvent(CreateSocialEventCommand command)
    {
        return await _bus.InvokeAsync<CommandResult>(command);
    }
    
    [HttpPut("publish")]
    public async Task<CommandResult> PublishSocialEvent(Guid socialEventId)
    {
        return await _bus.InvokeAsync<CommandResult>(new PublishSocialEventCommand(socialEventId));
    }
    
    [HttpPut("cancel")]
    public async Task<CommandResult> CancelSocialEvent(Guid socialEventId)
    {
        return await _bus.InvokeAsync<CommandResult>(new CancelSocialEventCommand(socialEventId));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<SocialEvent> GetSocialEvent([FromRoute] Guid id)
    {
        return await _query.GetById(id);
    }
    
    [HttpGet("list/{pageNumber}/{pageSize}")]
    public async Task<IPagedList<SocialEventProfileDetails>> GetSocialEvents([FromRoute] int pageNumber, [FromRoute] int pageSize)
    {
        return await _query.List(pageNumber, pageSize);
    }
}